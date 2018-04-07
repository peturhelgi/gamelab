using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project.GameObjects.Miner;
using Project.GameObjects;
using System.Collections.Generic;
using Project.Util;
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

        Texture2D ground_1, ground_2;

        GameObject miner;

        private Vector2 screenCenter;
        private Vector2 groundOrigin_1, groundOrigin_2;
        private Vector2 minerOrigin;

        private Song music;
        private VideoPlayer player;
        private List<GameObject> gameObjects;
        private MapLoader mapLoader;
        private GameState gameState;

        private Camera camera;
        private GameController controller;

        string lvlName = "samplelvl";



        public GreatEscape()
        {
            graphics = new GraphicsDeviceManager(this);

            // this is supposed to change the window resolution, but I can't really get it to work
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();


            Content.RootDirectory = "Content";
            gameObjects = new List<GameObject>();

            mapLoader = new MapLoader(gameObjects, Content);

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
            this.IsMouseVisible = true;
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mapLoader.loadMapContent(gameState);



            // initialize Camera
            camera = new Camera(0.5f, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f));
            controller = new GameController();

            font = Content.Load<SpriteFont>("font");

            // Create a new SpriteBatch, which can be used to draw textures.
            sprite_batch = new SpriteBatch(GraphicsDevice);

            ground_1 = Content.Load<Texture2D>("Ground"); // 1000x482px
            ground_2 = Content.Load<Texture2D>("Ground"); // 1000x482px

            groundOrigin_1 = new Vector2(ground_1.Width / 2f, ground_1.Height / 2f);
            groundOrigin_2 = new Vector2(ground_2.Width / 2f, ground_2.Height / 2f);


            miner = gameState.getMiner1();


            /* Ground */

            //groundBody_1 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(512f), ConvertUnits.ToSimUnits(128f), 1f, groundPosition_1);
            //groundBody_2 = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(512f), ConvertUnits.ToSimUnits(128f), 1f, groundPosition_2);
            //groundBody_1.BodyType = groundBody_2.BodyType = BodyType.Static;
            //groundBody_1.Restitution = groundBody_2.Restitution = 0.3f;
            //groundBody_1.Friction = groundBody_2.Friction = 0.5f;

            // TODO: use this.Content to load your game content here
            //music = Content.Load<Song>("caveMusic");

            //MediaPlayer.Play(music);
            //player = new VideoPlayer();
        }

        private void RestartGame()
        {
            Initialize();
            LoadContent();
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
            controller.HandleInput();
            
            base.Update(gameTime);
        }

        
        // The camera is still handled here, as it is for testing only (at least for know)
        private void HandleCamera(KeyboardState state)
        {

            // Move camera
            if (state.IsKeyDown(Keys.A))
            {
                camera.Translate(new Vector2(-1, 0));
            }
            if (state.IsKeyDown(Keys.D)) { camera.Translate(new Vector2(1, 0)); }

            if (state.IsKeyDown(Keys.W))
            {
                camera.Translate(new Vector2(0, -1));
            }

            if (state.IsKeyDown(Keys.S))
            {
                camera.Translate(new Vector2(0, 1 ));
            }

          
            if (state.IsKeyDown(Keys.Z)) // Press 'z' to zoom out.
                camera.ZoomOut();
            else if (state.IsKeyDown(Keys.X)) // Press 'x' to zoom in.
                camera.ZoomIn();
           
        }

        private void HandleKeyboard()
        {

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            HandleCamera(state);

            Miner miner = gameState.getMiner1();

            if (state.IsKeyDown(Keys.Home))
                RestartGame();

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

            if (state.IsKeyDown(Keys.Left) && oldKeyState.IsKeyUp(Keys.Left))
                //miner.Body.ApplyLinearImpulse(new Vector2(0, 10));
                miner.Move(new Vector2(-2, 0));
            if (state.IsKeyDown(Keys.Right) && oldKeyState.IsKeyUp(Keys.Right))
            {
                miner.Move(new Vector2(2, 0));


            }

            //if (!miner.IsAirborne()) {
            //    miner.Halt();
            //}


            // cannot integrate this IsKeyUp for Space, I do not know why yet
            if (state.IsKeyDown(Keys.Space)) // && oldKeyState.IsKeyUp(Keys.Space))
                miner.Jump();

            if (state.IsKeyDown(Keys.T))
            {
                //miner.Position = startingPosition;
                miner.Halt();
            }
            if (state.IsKeyDown(Keys.Q))
            {
                miner.UseTool(this.gameObjects);
            }

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
            oldKeyState = state;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sprite_batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.view);

            foreach (GameObject obj in gameObjects)
            {
                if (obj is Miner)
                {
                    sprite_batch.Draw(obj.Texture, obj.Position, Color.White);
                    continue;
                }

                if (obj is Ground)
                {
                    sprite_batch.Draw(obj.Texture, obj.Position, Color.White);
                    continue;
                }
            }


            sprite_batch.End();

            base.Draw(gameTime);
        }
    }
}
