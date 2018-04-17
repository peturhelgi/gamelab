
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project.Screens;
using System.Collections.Generic;
using Project.Util;
using System;
using System.Diagnostics;
using System.Linq;

namespace Project {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GreatEscape : Game {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Texture2D exitSign;
        //Texture2D DebugBox;
        //DataManager<Level> levelLoader;

        public GreatEscape() {
            graphics = new GraphicsDeviceManager(this) {
                //TODO: How to get the size of the window?
                //HACK: Have the screen fullscreen and use the monitor's dimensions
                IsFullScreen = !true,
                PreferredBackBufferWidth = 1024,
                PreferredBackBufferHeight = 768
            };
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            //levelLoader = new DataManager<Level>();
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //exitSign = Content.Load<Texture2D>("Sprites/Backgrounds/ExitSign_2");
            //DebugBox = Content.Load<Texture2D>("Sprites/Misc/box");

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.Dimensions = new Vector2(
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(this.Content);
        }

        private void RestartGame() {
            UnloadContent();
            Initialize();
            LoadContent();
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            ScreenManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            ScreenManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.TransparentBlack);
            //spriteBatch.Begin();

            //Camera camera = new Camera(0.8f, Vector2.Zero);
            //            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.view);
            ScreenManager.Instance.Draw(spriteBatch);

            //TODO: Cleanup, consider using ScreenManager instead

            /*
            bool DebugView = Keyboard.GetState().IsKeyDown(Keys.P);
            if(DebugView){
                //To view the bounding boxes
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, null, controller.Camera.view);
            }
            else {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, controller.Camera.view);
            }
            */

            /*
            // TODO move to the MapLoader/GameState 
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.Draw(exitSign, new Rectangle(1100, 300, exitSign.Width/5, exitSign.Height/5), Color.White);
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth * 3/2,  graphics.PreferredBackBufferHeight * 3/2), Color.White);
            spriteBatch.Draw(exitSign, new Rectangle(1430, 300, exitSign.Width/5, exitSign.Height/5), Color.White);


            RasterizerState state = new RasterizerState();
            state.FillMode = FillMode.WireFrame;
            spriteBatch.GraphicsDevice.RasterizerState = state;

            foreach (GameObject obj in controller.GameEngine.GameState.GetAll())
            {
                if (obj.Visible)
                {
                    if (DebugView)
                    {
                        spriteBatch.Draw(DebugBox, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.SpriteSize.X, (int)obj.SpriteSize.Y), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(obj.Texture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.SpriteSize.X, (int)obj.SpriteSize.Y), Color.White);
                    }
                }
            }
            */

            // spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
