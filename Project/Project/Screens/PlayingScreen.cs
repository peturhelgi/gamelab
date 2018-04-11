using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Project.GameObjects;


namespace Project.Screens
{
    public class PlayingScreen : GameScreen
    {
        Miner miner;
        Level level;
        public override void LoadContent()
        {
            base.LoadContent();

            DataManager<Miner> minerLoader = new DataManager<Miner>();
            DataManager<Level> levelLoader = new DataManager<Level>();
            miner = minerLoader.Load("Content/GamePlay/Miner");
            level = levelLoader.Load("Content/GamePlay/Levels/Level1");
            miner.LoadContent();
            level.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            miner.UnloadContent();
            level.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            miner.Update(gameTime);
            level.Update(gameTime, ref miner);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            level.Draw(spriteBatch, "Underlay");
            miner.Draw(spriteBatch);
            level.Draw(spriteBatch, "Overlay");
        }

        #region Old Code commented out. Click to Expand
        /*
        Texture2D image;
        public string Path { get; set; }
        public Vector2 Position;

        Vector2 gravity = new Vector2(0, -1000);
        private Song music;
        private VideoPlayer player;
        private List<GameObject> gameObjects;
        private MapLoader mapLoader;
        private GameState gameState;

        // TODO: Remove this hard coding
        Vector2 up    = new Vector2( 0, -150),
                down  = new Vector2( 0,  150),
                left  = new Vector2(-150,  0),
                right = new Vector2( 150,  0);
        //private ContentManager contentManager;
        

        // TODO: Add an Initializer to the class
        public override void LoadContent()
        {
            base.LoadContent();
            music = content.Load<Song>("caveMusic");
            mapLoader.loadMapContent(gameState);
            MediaPlayer.Play(music);
            player = new VideoPlayer();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            Miner miner = gameState.GetMiner(0);
            Vector2 startingPosition = new Vector2(210, 250);

            // TODO: Add your update logic here
           // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
           //     Keyboard.GetState().IsKeyDown(Keys.Escape))
           //     this.Exit();

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
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.Visible)
                    spriteBatch.Draw(
                        obj.Texture, 
                        new Rectangle(
                            (int)obj.Position.X, 
                            (int)obj.Position.Y, 
                            obj.Texture.Width, 
                            obj.Texture.Height), 
                        Color.White);
            }
        }
        */
        #endregion

    }
}
