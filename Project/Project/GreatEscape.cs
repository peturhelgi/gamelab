using Microsoft.Xna.Framework;
using Project.Manager;
using Project.Menu;

namespace Project
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GreatEscape : Game
    {
        GameManager _gameManager;
        MenuManager _menu;
        GraphicsDeviceManager _graphics;


        public GreatEscape()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
                IsFullScreen = false
            };
            _graphics.ApplyChanges();
        }
        
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";

            _gameManager = new GameManager(Content, GraphicsDevice, _graphics);
            _menu = new MenuManager(Content, GraphicsDevice, _graphics, _gameManager);
            
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _menu.LoadContent();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            _menu.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _menu.Draw(gameTime, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            base.Draw(gameTime);
        }
    }
}
