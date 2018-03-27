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
    public class GreatEscape : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Song music;
        private VideoPlayer player;
        private Vector2 startingPosition;
        Miner miner;
        Vector2 gravity = new Vector2(0, -1000);
        Vector2 direction, up, down, left, right;

        public GreatEscape()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            startingPosition = new Vector2(210, 250);
            up    = new Vector2( 0, -150);
            down  = new Vector2( 0,  150);
            left  = new Vector2(-150,  0);
            right = new Vector2( 150,  0);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            miner = new Miner( startingPosition, new Vector2(0.0f), 80.0, new BoundingBox()) ;
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
            miner.Texture = Content.Load<Texture2D>("Miner");

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
                // This is very incomplete...

                KeyboardState state = Keyboard.GetState();

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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(miner.Texture, new Rectangle((int)miner.Position.X, (int)miner.Position.Y, 490, 600), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
