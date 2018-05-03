using EditorLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic;
using TheGreatEscape.Menu;

namespace TheGreatEscape
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GreatEscape : Game
    {
        GameManager _gameManager;
        EditorManager _editorManager;
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
            _editorManager = new EditorManager(Content, GraphicsDevice, _graphics);
            _menu = new MenuManager(Content, GraphicsDevice, _graphics, _gameManager, _editorManager);
            
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
            _menu.Draw(
                gameTime, 
                GraphicsDevice.PresentationParameters.BackBufferWidth, 
                GraphicsDevice.PresentationParameters.BackBufferHeight);
            base.Draw(gameTime);
        }
    }
}
