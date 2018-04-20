using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.GameObjects;
using Project.Util;
using Project.Manager;

namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GreatEscape : Game
    {
        GameManager _manager;
        GraphicsDeviceManager _graphics;


        public GreatEscape()
        {
            _graphics = new GraphicsDeviceManager(this)
            {

                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                IsFullScreen = false
            };
            _graphics.ApplyChanges();
        }
        
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";

            _manager = new GameManager(Content, GraphicsDevice, _graphics);
            _manager.LoadLevel("more_platforms");
            
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _manager.LoadContent();
        }

        protected override void UnloadContent()
        {
            _manager.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _manager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _manager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
