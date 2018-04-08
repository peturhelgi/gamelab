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

        Texture2D background;
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
            
            sprite_batch = new SpriteBatch(GraphicsDevice);
            background = mapLoader.getBackground();
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
            controller.HandleUpdate(gameTime);
            base.Update(gameTime);
        }

        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            sprite_batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, controller.Camera.view);

            //To view the bounding boxes
            //sprite_batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, null, controller.Camera.view);


            sprite_batch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            sprite_batch.Draw(exitSign, new Rectangle(1100, 300, exitSign.Width/5, exitSign.Height/5), Color.White);

            RasterizerState state = new RasterizerState();
            state.FillMode = FillMode.WireFrame;
            sprite_batch.GraphicsDevice.RasterizerState = state;

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
