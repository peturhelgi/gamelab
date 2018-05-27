using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.Renderer;
using TheGreatEscape.LevelManager;
using System;
using TheGreatEscape.Util;

namespace TheGreatEscape.GameLogic {
    public class GameManager
    {
        GameController _controller;
        MapLoader _mapLoader;

        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;
        GameRenderer _renderer;
        public static bool RenderDark;
        public GameEngine GameEngine { get; private set; }
        
        public GameManager(
                ContentManager content, 
                GraphicsDevice graphicsDevice, 
                GraphicsDeviceManager graphics,
                InputManager manager) {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _mapLoader = new MapLoader(content);
            RenderDark = true;
        }

        public void LoadLevel(String path) {
            if (_controller != null)
            {
                UnloadContent();
            }
            GameEngine = new GameEngine(_mapLoader.InitMap(path));
            _controller = new GameController(
                GameEngine, 
                new Camera(0.8f, 
                    Vector2.Zero, 
                    new Vector2(
                        _graphicsDevice.PresentationParameters.BackBufferWidth, 
                        _graphicsDevice.PresentationParameters.BackBufferHeight)
                ));
            LoadContent();
        }

        void LoadContent()
        {
            _mapLoader.LoadMapContent(_controller.GameEngine.GameState);
            _controller.GameEngine.gameTime = new TimeSpan();
            _renderer = new GameRenderer(_graphicsDevice, 
                _controller.GameEngine.GameState, _content);
        }

        public void UnloadContent()
        {
            _controller = null;
        }

        public void Update(GameTime gameTime)
        {
            _controller.HandleUpdate(gameTime);
        }

        public void Draw(GameTime gameTime, int width, int height)
        {
            _renderer.Draw(gameTime, width, height,
                MyDebugger.IsActive ? 
                GameRenderer.Mode.DebugView : GameRenderer.Mode.Normal, 
                _controller.Camera); 
        }

    }
}
