using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.GameLogic.GameObjects;
using Project.GameLogic.Renderer;
using Project.LevelManager;
using System;

using TheGreatEscape.GameLogic.Util;

namespace Project.GameLogic
{
    class GameManager
    {
        GameController _controller;
        MapLoader _mapLoader;

        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;
        GameRenderer _renderer;
        public static bool RenderDark;

        public GameManager(ContentManager content, GraphicsDevice graphicsDevice, 
                GraphicsDeviceManager graphics) {
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
            _controller = new GameController(
                new GameEngine(_mapLoader.InitMap(path)), 
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
            // The render instance is create per level: Like this we don't need to worry about resetting global variables in the renderer (e.g. lightning)
            _renderer = new GameRenderer(_graphicsDevice, 
                _controller.GameEngine.GameState, _content);
        }

        //TODO remove public
        public void UnloadContent()
        {
            //If we run into memory problems at some time, we will need to Dispose the textures loaded when unloading the game. But this will unlikely happen, as the content loader uses a dictionary to only load textures once. Remark: Dispose per texture doesn't work! One would have to use a seperate ContentManager (as there is global content which will never be unloaded) and unload the complete ContentManager
            //_mapLoader.UnloadMapContent(_controller.GameEngine.GameState);
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
