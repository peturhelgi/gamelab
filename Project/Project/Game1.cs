using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D sheep;
        private Song music;
        int restPoseX, restPoseY, curPoseY, curPoseX;
        double curSpeed;
        private VideoPlayer player;

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
            // TODO: Add your initialization logic here
            restPoseY = 210;
            restPoseX = 250;
            curPoseY = restPoseY;
            curPoseX = restPoseX;
            curSpeed = 0.0;
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
            sheep = Content.Load<Texture2D>("Miner");
            music = Content.Load<Song>("caveMusic");

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
                if (gamePadState.Buttons.A == ButtonState.Pressed)
                    curSpeed = -300.0;
            }
            // Keyboard input otherwise
            else
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Space))
                    curSpeed = -300.0;
                if (state.IsKeyDown(Keys.Right))
                {
                    curSpeed = 5.0;
                    curPoseX += (int)(curSpeed);
                }
                if (state.IsKeyDown(Keys.R))
                {
                    curPoseX = restPoseX;
                    curPoseY = restPoseY;
                }

            }

            if ((int)curPoseY < restPoseY || curSpeed != 0.0)
            {
                if (curSpeed > 0.0 && (int)curPoseY >= restPoseY)
                    curSpeed = 0.0;
                else
                {
                    double dt = 0.01;
                    double g = 1000;
                    curPoseY += (int)(curSpeed * dt);
                    curSpeed += g * dt;
                }
            }
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
            spriteBatch.Draw(sheep, new Rectangle((int)curPoseX, (int)curPoseY, 490, 600), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
