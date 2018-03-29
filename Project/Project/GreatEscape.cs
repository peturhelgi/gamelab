using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project.GameObjects.Miner;
using Project.GameObjects;
using System.Collections.Generic;
using Project.Util;
using System;

namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GreatEscape : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Song music;
        private VideoPlayer player;
        private List<GameObject> gameObjects;
        private MapLoader mapLoader;
        private GameState gameState;
        Vector2 gravity = new Vector2(0, -1000);
        Vector2 direction, up, down, left, right;

        string lvlName = "samplelvl";

        // Simple camera controls
        private Vector2 _viewCenter;
        private float _viewZoom;
        private Matrix view;
        private Vector2 cameraPosition;


        public GreatEscape()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameObjects = new List<GameObject>();
            mapLoader = new MapLoader(gameObjects, Content);
            
            up    = new Vector2( 0, -150);
            down  = new Vector2( 0,  150);
            left  = new Vector2(-150,  0);
            right = new Vector2( 150,  0);
        }

        public float ViewZoom {
            get { return _viewZoom; }
            set {
                _viewZoom = value;
                Resize();
            }
        }

        public Vector2 ViewCenter {
            get { return _viewCenter; }
            set {
                _viewCenter = value;
                Resize();
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameState = mapLoader.initMap(lvlName);
            ResetView();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            music = Content.Load<Song>("caveMusic");

            mapLoader.loadMapContent(gameState);

            MediaPlayer.Play(music);
            player = new VideoPlayer();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Miner miner = gameState.getMiner1();
            Vector2 startingPosition = new Vector2(210, 250);

            // TODO: Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Check for input on controller
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.IsConnected)
            {

            }
            // Keyboard input otherwise
            else
            {
                // This is very incomplete...


                KeyboardState state = Keyboard.GetState();

                HandleCameraControls(state);

                if (state.IsKeyDown(Keys.B)) {
                    if (miner.IsStanding() || miner.IsLying())
                        miner.Crouch();
                    else if (miner.IsCrouching())
                        miner.StandUp();
                }
                    
                if (state.IsKeyDown(Keys.X))
                {
                    if(!miner.IsAirborne())
                        miner.Run();
                }
                else // either the player wasn't running, or they just quit
                {
                    if (miner.IsRunning())
                    {
                        miner.Walk();
                    }
                }
                if (state.IsKeyDown(Keys.Space))
                {
                    if(!miner.IsAirborne())
                        miner.Jump();
                }
                if (state.IsKeyDown(Keys.Right))
                {
                    miner.Move(new Vector2(right.X, miner.Speed.Y));
                }
                
                else if (state.IsKeyDown(Keys.Left))
                {
                    miner.Move(new Vector2(left.X, miner.Speed.Y));
                }
                else if(!miner.IsAirborne())
                {
                    miner.Halt();
                }

                if (state.IsKeyDown(Keys.R))
                {
                    miner.Position = startingPosition;
                    miner.Halt();
                }
                if (state.IsKeyDown(Keys.Q))
                {
                    miner.UseTool(this.gameObjects);
                }
            }

            // TODO make this with the physics engine and with bounding boxes!
            // if character is jumping

            if (miner.IsAirborne())
            {
                if (miner.Position.Y > startingPosition.Y)
                {
                    miner.Halt();
                    miner.Position = new Vector2(miner.Position.X, startingPosition.Y);
                }
                else
                {
                    miner.Speed -= (float)gameTime.ElapsedGameTime.TotalSeconds * gravity;
                }
            }

            miner.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * miner.Speed;

            base.Update(gameTime);
        }

        private void HandleCameraControls(KeyboardState state) {

            if (state.IsKeyDown(Keys.Z)) // Press 'z' to zoom in.
                ViewZoom = Math.Min(1.1f * ViewZoom, 20.0f);
            else if (state.IsKeyDown(Keys.X)) // Press 'x' to zoom out.
                ViewZoom = Math.Max(0.9f * ViewZoom, 0.02f);
            else if (state.IsKeyDown(Keys.R)) // Press 'r' to reset.
                ResetView();

            if (state.IsKeyDown(Keys.A)) // Press left to pan left.
                ViewCenter = new Vector2(ViewCenter.X - 2f, ViewCenter.Y);
            else if (state.IsKeyDown(Keys.D)) // Press right to pan right.
                ViewCenter = new Vector2(ViewCenter.X + 2f, ViewCenter.Y);
            else if (state.IsKeyDown(Keys.S)) // Press down to pan down.
                ViewCenter = new Vector2(ViewCenter.X, ViewCenter.Y + 2f);
            else if (state.IsKeyDown(Keys.W)) // Press up to pan up.
                ViewCenter = new Vector2(ViewCenter.X, ViewCenter.Y - 2f);

        }
        private void Resize() {
            view = Matrix.CreateTranslation(new Vector3(-ViewCenter.X, -ViewCenter.Y, 0)) * Matrix.CreateScale(ViewZoom);
        }

        private void ResetView() {
            _viewZoom = 0.8f;
            _viewCenter = Vector2.Zero;
            //_viewCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            Resize();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, view);
            foreach (GameObject obj in gameObjects) {
                if (obj.visible) spriteBatch.Draw(obj.Texture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, obj.Texture.Width, obj.Texture.Height), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
