using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.GameLogic;
using Project.GameLogic.GameObjects;
using Project.GameLogic.Renderer;
using Project.LevelManager;
using System;

namespace EditorLogic
{
    class EditorManager
    {
        EditorController _controller;
        MapLoader _mapLoader;

        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;
        GameRenderer _gameRenderer;
        EditorRenderer _editorRenderer;

        // Editor State
        public Vector2 CursorPosition;



        public EditorManager(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _mapLoader = new MapLoader(content);
            CursorPosition = Vector2.Zero;
        }

        public void LoadLevel(String path)
        {
            _controller = new EditorController(
                new GameEngine(_mapLoader.InitMap(path)), 
                new Camera(0.2f, Vector2.Zero, new Vector2(_graphicsDevice.PresentationParameters.BackBufferWidth, _graphicsDevice.PresentationParameters.BackBufferHeight)),
                this);
            LoadContent();
        }

        void LoadContent()
        {
            _mapLoader.LoadMapContent(_controller.GameEngine.GameState);
            // The render instance is create per level: Like this we don't need to worry about resetting global variables in the renderer (e.g. lightning)
            _gameRenderer = new GameRenderer(_graphicsDevice, _controller.GameEngine.GameState, _content);
            _editorRenderer = new EditorRenderer(_graphicsDevice, _controller.GameEngine.GameState, _content, this);
        }


        public void Update(GameTime gameTime)
        {
            _controller.HandleUpdate(gameTime);
        }

        public void Draw(GameTime gameTime, int width, int height)
        {
            _gameRenderer.Draw(gameTime, width, height, Keyboard.GetState().IsKeyDown(Keys.P) ? GameRenderer.Mode.DebugView : GameRenderer.Mode.Normal, _controller.Camera.view);
            _editorRenderer.Draw(gameTime, width, height, _controller.Camera.view);
            

        }

    }
}
