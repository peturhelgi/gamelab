using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project.GameObjects.Miner;


namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Song music;
        private VideoPlayer player;

        Miner test;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            test = new Miner( new Vector2(210, 250), new Vector2(0.0f), 80.0, new BoundingBox()) ;
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
            test.Texture = Content.Load<Texture2D>("Miner");

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
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Space))
                    test.Speed = new Vector2(0, -300);
                if (state.IsKeyDown(Keys.Right))
                {
                    test.Position += new Vector2(5, 0);
                }
                if (state.IsKeyDown(Keys.R))
                {
                    test.Position = new Vector2(210,250);
                }

            }


            Vector2 gravity = new Vector2(0, -1000);


            // TODO make this with the physics engine and with bounding boxes!
            // if character is jumping
            if (test.Speed.Y > 0.0) {

                // some hard coded bounding "box"
                if (test.Position.Y > 240)
                {
                    test.Speed = new Vector2(0, 0);
                }
                else {
                    test.Speed += (float)gameTime.ElapsedGameTime.TotalSeconds * gravity;
                }


            }

            test.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * test.Speed;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(test.Texture, new Rectangle((int)test.Position.X, (int)test.Position.Y, 490, 600), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
