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
        
        //private List<GameObject> gameObjects;
        private MapLoader mapLoader;

        Vector2 gravity = new Vector2(0, -1000);
        Vector2 direction, up, down, left, right;
        Texture2D background;
        Texture2D ground_1, ground_2;
        Texture2D exitSign;

        private GameController controller;
        string lvlName = "samplelvl";

        public GreatEscape()
        {
            graphics = new GraphicsDeviceManager(this) {

                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                IsFullScreen = false
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            mapLoader = new MapLoader(Content);

        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            controller = new GameController(new GameEngine(mapLoader.InitMap(lvlName)), new Camera(1f, Vector2.Zero));

            IsMouseVisible = true;
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mapLoader.LoadMapContent(controller.GameEngine.GameState);
            
            // Create a new SpriteBatch, which can be used to draw textures.
            // TODO: use this.Content to load your game content here
            //music = Content.Load<Song>("caveMusic");
            //MediaPlayer.Play(music);
            //player = new VideoPlayer();

            sprite_batch = new SpriteBatch(GraphicsDevice);
            background = mapLoader.getBackground();
            ground_1 = Content.Load<Texture2D>("Sprites/Rocks/Ground1");
            ground_2 = Content.Load<Texture2D>("Sprites/Rocks/Ground3");
            exitSign = Content.Load<Texture2D>("Sprites/Backgrounds/ExitSign_2");
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

            controller.GameEngine.Update(0, gameTime);
            // TODO integrate these into the controller and remove them here
           // HandleKeyboard();
            
            base.Update(gameTime);
        }

        
       
        /*
        private void HandleKeyboard()
        {

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
                Exit();


            if (state.IsKeyDown(Keys.Home))
                RestartGame();


            if (state.IsKeyDown(Keys.Right))
                miner.Position += new Vector2(5, 0);

            if (state.IsKeyDown(Keys.Left))
                miner.Position += new Vector2(-5, 0);

            if (state.IsKeyDown(Keys.Up))
                miner.Position += new Vector2(0, -5);

            if (state.IsKeyDown(Keys.Down))
                miner.Position += new Vector2(0, 5);



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
        */
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            sprite_batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, controller.Camera.view);

            sprite_batch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            sprite_batch.Draw(exitSign, new Rectangle(1000, 300, exitSign.Width/5, exitSign.Height/5), Color.White);

                foreach (GameObject obj in controller.GameEngine.GameState.GetAll())
            {
                //if(obj.Visible) sprite_batch.Draw(obj.Texture, obj.Position, Color.White);
                if (obj.Visible) sprite_batch.Draw(obj.Texture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.SpriteSize.X, (int)obj.SpriteSize.Y), Color.White); 
            }

            sprite_batch.End();

            base.Draw(gameTime);
        }
    }
}
