using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.GameLogic.GameObjects;
using Project.LevelManager;
using System;

namespace Project.GameLogic
{
    class GameManager
    {
        GameController _controller;
        MapLoader _mapLoader;

        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;


        // TODO remove these
        Texture2D _background;
        Texture2D _exitSign;
        Texture2D _debugBox;


        public GameManager(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics) {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _mapLoader = new MapLoader(content);

        }
      

        public void LoadLevel(String path) {
            if (_controller != null)
            {
                UnloadContent();
            }
            _controller = new GameController(new GameEngine(_mapLoader.InitMap(path)), new Camera(0.8f, Vector2.Zero));
            LoadContent();
        }

        void LoadContent()
        {


            _mapLoader.LoadMapContent(_controller.GameEngine.GameState);

            // TODO remove these
            _background = _mapLoader.getBackground();
            _exitSign = _content.Load<Texture2D>("Sprites/Backgrounds/ExitSign_2");
            _debugBox = _content.Load<Texture2D>("Sprites/Misc/box");

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
            _graphicsDevice.Clear(Color.White);

            // TODO: integrate the Debug-View into the Renderer and the GameController
            bool DebugView = Keyboard.GetState().IsKeyDown(Keys.P);
            if (DebugView)
            {
                //To view the bounding boxes
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, null, _controller.Camera.view);
            }
            else
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _controller.Camera.view);
            }


            // TODO move to the MapLoader/GameState 
            _spriteBatch.Draw(_background, new Rectangle(0, 0, width, height), Color.White);
            _spriteBatch.Draw(_background, new Rectangle(0, 0, width * 3 / 2, height * 3 / 2), Color.White);
            _spriteBatch.Draw(_exitSign, new Rectangle(1430, 300, _exitSign.Width / 5, _exitSign.Height / 5), Color.White);


            // TODO move to Renderer
            RasterizerState state = new RasterizerState
            {
                FillMode = FillMode.WireFrame
            };

            _spriteBatch.GraphicsDevice.RasterizerState = state;

            foreach (GameObject obj in _controller.GameEngine.GameState.GetAll())
            {
                if (obj.Visible)
                {
                    if (DebugView)
                    {
                        _spriteBatch.Draw(_debugBox, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.SpriteSize.X, (int)obj.SpriteSize.Y), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(obj.Texture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.SpriteSize.X, (int)obj.SpriteSize.Y), Color.White);
                    }
                }
            }

            _spriteBatch.End();

        }

    }
}
