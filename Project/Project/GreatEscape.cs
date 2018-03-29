using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project.GameObjects.Miner;
using Project.GameObjects;
using System.Collections.Generic;
using Project.Util;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;
using System;
using System.Diagnostics;

namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GreatEscape : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch sprite_batch;
        private KeyboardState oldKeyState;
        private GamePadState oldPadState;
        private SpriteFont font;
        private readonly World world;

        Texture2D ground_1, ground_2;
        private Body minerBody;
        private Body groundBody_1, groundBody_2;

        GameObject miner;

        // Simple camera controls
        private Matrix view;
        private float _viewZoom;

        private Vector2 cameraPosition;
        private Vector2 screenCenter;
        private Vector2 groundOrigin_1, groundOrigin_2;
        private Vector2 minerOrigin;

        private Song music;
        private VideoPlayer player;
        private static List<GameObject> gameObjects;
        private MapLoader mapLoader;
        private GameState gameState;


        string lvlName = "samplelvl";

        public GreatEscape()
        {
            graphics = new GraphicsDeviceManager(this);

            // this is supposed to change the window resolution, but I can't really get it to work
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            // Create a world with gravity.
            //this.world = new World(new Vector2(0, 9.82f));
            Content.RootDirectory = "Content";
            gameObjects = new List<GameObject>();
            world = new World(new Vector2(0, 9.82f));
            mapLoader = new MapLoader(gameObjects, Content);
            
        }

        public float ViewZoom {
            get { return _viewZoom; }
            set {
                _viewZoom = value;
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
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mapLoader.loadMapContent(gameState);


            // Initialize camera controls
            view = Matrix.Identity;
            ResetView();
            //cameraPosition = Vector2.Zero;
            //screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);

            font = Content.Load<SpriteFont>("font");

            // Create a new SpriteBatch, which can be used to draw textures.
            sprite_batch = new SpriteBatch(GraphicsDevice);

            ground_1 = Content.Load<Texture2D>("Ground"); // 1000x482px
            ground_2 = Content.Load<Texture2D>("Ground"); // 1000x482px

            groundOrigin_1 = new Vector2(ground_1.Width / 2f, ground_1.Height / 2f);
            groundOrigin_2 = new Vector2(ground_2.Width / 2f, ground_2.Height / 2f);

            // Velcro Physics expects objects to be scaled to MKS (meters, kilos, seconds)
            // 1 meters equals 64 pixels here
            ConvertUnits.SetDisplayUnitToSimUnitRatio(128f);

            miner = gameState.getMiner1();
            minerOrigin = miner.Position;
            Vector2 minerPosition = ConvertUnits.ToSimUnits(screenCenter) + new Vector2(0, -1.5f);

            // The height is connected to the restitution when applying a linear impulse 
            minerBody = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(miner.Texture.Height / 2f), 1f, minerPosition, BodyType.Dynamic);
            minerBody.Restitution = 0.3f;
            minerBody.Friction = 1f;

            /* Ground */
            Vector2 groundPosition_1 = ConvertUnits.ToSimUnits(screenCenter) + new Vector2(0, 4.25f);
            Vector2 groundPosition_2 = ConvertUnits.ToSimUnits(screenCenter) + new Vector2(1.25f, 4.25f);

            groundBody_1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(512f), ConvertUnits.ToSimUnits(128f), 1f, groundPosition_1);
            groundBody_2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(512f), ConvertUnits.ToSimUnits(128f), 1f, groundPosition_2);
            groundBody_1.BodyType = groundBody_2.BodyType = BodyType.Static;
            groundBody_1.Restitution = groundBody_2.Restitution = 0.3f;
            groundBody_1.Friction = groundBody_2.Friction = 0.5f;

            // TODO: use this.Content to load your game content here
            //music = Content.Load<Song>("caveMusic");

            //MediaPlayer.Play(music);
            //player = new VideoPlayer();
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
            HandleGamePad();
            HandleKeyboard();

            //We update the world
            //mapLoader.world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            //mapLoader.world.Step((float)3);

            base.Update(gameTime);
        }

        private void ResetView() {
            _viewZoom = 0.5f;
            cameraPosition = Vector2.Zero;
            screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            Resize();
        }

        private void Resize() {
            view = Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0)) * Matrix.CreateScale(ViewZoom);
        }

        private void HandleGamePad() {
            //Check for input on controller
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.IsConnected)
            {
                if (gamePadState.Buttons.Back == ButtonState.Pressed)
                    Exit();
            }
        }

        private void HandleKeyboard() {

            KeyboardState state = Keyboard.GetState();


            if (state.IsKeyDown(Keys.Escape))
                Exit();

            Miner miner = gameState.getMiner1();
            Vector2 startingPosition = new Vector2(210, 250);

            // Move camera
            if (state.IsKeyDown(Keys.A))
                cameraPosition.X += 1.5f;

            if (state.IsKeyDown(Keys.D))
                cameraPosition.X -= 1.5f;

            if (state.IsKeyDown(Keys.W))
                cameraPosition.Y += 1.5f;

            if (state.IsKeyDown(Keys.S))
                cameraPosition.Y -= 1.5f;

            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.S))
                Resize();

            if (state.IsKeyDown(Keys.Z)) // Press 'z' to zoom out.
                ViewZoom = Math.Min(1.1f * ViewZoom, 5.0f);
            else if (state.IsKeyDown(Keys.X)) // Press 'x' to zoom in.
                ViewZoom = Math.Max(0.9f * ViewZoom, 0.02f);
            else if (state.IsKeyDown(Keys.R)) // Press 'r' to reset.
                ResetView();


            if (state.IsKeyDown(Keys.Left))
                miner.Body.ApplyLinearImpulse(new Vector2(0, 10));
                //miner.Move(new Vector2(-20, 0));
            if (state.IsKeyDown(Keys.Right))
                miner.Move(new Vector2(20, 0));

            //if (!miner.IsAirborne())
            //    miner.Jump();

            //if (state.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space))
            //    minerBody.ApplyLinearImpulse(new Vector2(0, -2));

            //oldKeyState = state;


            //if (state.IsKeyDown(Keys.B)) {
            //    if (miner.IsStanding() || miner.IsLying())
            //        miner.Crouch();
            //    else if (miner.IsCrouching())
            //        miner.StandUp();
            //}

            //if (state.IsKeyDown(Keys.X)) {
            //    if (!miner.IsAirborne())
            //        miner.Run();
            //}
            //else // either the player wasn't running, or they just quit
            //{
            //    if (miner.IsRunning()) {
            //        miner.Walk();
            //    }
            //}
            //if (state.IsKeyDown(Keys.Space)) {
            //    if (!miner.IsAirborne())
            //        miner.Jump();
            //}
            //if (state.IsKeyDown(Keys.Right)) {
            //    miner.Move(new Vector2(2, 0));
            //}

            //else if (state.IsKeyDown(Keys.Left)) {
            //    miner.Move(new Vector2(-2, 0));
            //}
            //            else if (!miner.IsAirborne())
            //            {
            //                miner.Halt();
            //            }

            //            if (state.IsKeyDown(Keys.R))
            //            {
            //                miner.Position = startingPosition;
            //                miner.Halt();
            //            }
            //            if (state.IsKeyDown(Keys.Q))
            //            {
            //                miner.UseTool(this.gameObjects);
            //            }

            //            // TODO make this with the physics engine and with bounding boxes!
            //            // if character is jumping
            //            if (miner.IsAirborne())
            //            {
            //                if (miner.Position.Y > startingPosition.Y)
            //                {
            //                    miner.Halt();
            //                    miner.Position = new Vector2(miner.Position.X, startingPosition.Y);
            //    }
            //                else
            //                {
            //                    miner.Speed -= (float) gameTime.ElapsedGameTime.TotalSeconds * gravity;
            //}
            //            }

            //            miner.Position += (float) gameTime.ElapsedGameTime.TotalSeconds * miner.Speed;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //sprite_batch.Begin();
            sprite_batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, view);

            foreach (GameObject obj in gameObjects) {
                if (obj is Miner) {
                    //Vector2 miner_pos = ConvertUnits.ToDisplayUnits(obj.Body.Position);
                    //sprite_batch.Draw(obj.Texture, new Rectangle((int)miner_pos.X, (int)miner_pos.Y, obj.Texture.Width, obj.Texture.Height), Color.White);
                    //sprite_batch.Draw(obj.Texture, ConvertUnits.ToDisplayUnits(obj.Body.Position), Color.White);
                    continue;
                }

                if (obj is Ground) {
                    //Vector2 ground_pos = ConvertUnits.ToDisplayUnits(obj.Body.Position);
                    //sprite_batch.Draw(obj.Texture, new Rectangle((int)ground_pos.X, (int)ground_pos.Y, obj.Texture.Width, obj.Texture.Height), Color.White);
                    continue;
                }
                if (obj.Visible) sprite_batch.Draw(obj.Texture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, obj.Texture.Width, obj.Texture.Height), Color.White);
            }

            sprite_batch.Draw(miner.Texture, ConvertUnits.ToDisplayUnits(minerBody.Position), null, Color.White, 0f, minerOrigin, 1f, SpriteEffects.None, 0f);

            sprite_batch.Draw(ground_1, ConvertUnits.ToDisplayUnits(groundBody_1.Position), null, Color.White, 0f, new Vector2(100f, 100f), 0.5f, SpriteEffects.None, 0f);
            sprite_batch.Draw(ground_2, ConvertUnits.ToDisplayUnits(groundBody_2.Position), null, Color.White, 0f, new Vector2(400f, 100f), 0.5f, SpriteEffects.None, 0f);
            //sprite_batch.Draw(ground_1, ConvertUnits.ToDisplayUnits(groundBody_1.Position), Color.White);
            //sprite_batch.Draw(ground_2, ConvertUnits.ToDisplayUnits(groundBody_2.Position), Color.White);
            sprite_batch.End();

         

            base.Draw(gameTime);
        }
    }
}
