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
            controller = new GameController(new GameEngine(mapLoader.InitMap(lvlName)), new Camera(0.5f, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f)));

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
            sprite_batch = new SpriteBatch(GraphicsDevice);
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

            foreach (GameObject obj in controller.GameEngine.GameState.GetAll())
            {
                if(obj.Visible) sprite_batch.Draw(obj.Texture, obj.Position, Color.White);
            }


            sprite_batch.End();

            base.Draw(gameTime);
        }
    }
}
