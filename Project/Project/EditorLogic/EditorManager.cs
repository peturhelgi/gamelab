using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.GameLogic;
using Project.GameLogic.Collision;
using Project.GameLogic.GameObjects;
using Project.GameLogic.GameObjects.Miner;
using Project.GameLogic.Renderer;
using Project.LevelManager;
using System;
using System.Collections.Generic;

namespace EditorLogic
{
    class EditorManager
    {
        EditorController _editorController;
        GameController _gameController;
        MapLoader _mapLoader;

        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;
        GameRenderer _gameRenderer;
        EditorRenderer _editorRenderer;
        GameEngine _engine;
        Camera _camera;

        GamePadState _oldGamePadState;
        KeyboardState _oldKeyboardState;

        // Editor State
        public Vector2 CursorPosition;
        public bool Editing = true;
        public GameObject CurrentObject;


       

        public EditorManager(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _mapLoader = new MapLoader(content);
            CursorPosition = Vector2.Zero;

            _oldKeyboardState = Keyboard.GetState();
            _oldGamePadState = GamePad.GetState(PlayerIndex.One);
        }


        public void ExchangeCurrentObject(Object newObject)
        {
            CurrentObject = new Ground(Vector2.Zero, new Vector2(640, 318), "Sprites/Rocks/Ground1");
            CurrentObject.Texture = _content.Load<Texture2D>(CurrentObject.TextureString);
        }

        public void PlaceCurrentObject() {
            if (CurrentObject != null) {
                CurrentObject.Position = CursorPosition;
                _engine.GameState.AddSolid(CurrentObject);
                CurrentObject = null;
            }
        }

        public void PickObjectUnderCursor() {
            List<GameObject> collisions = _engine.CollisionDetector.FindCollisions(new AxisAllignedBoundingBox(CursorPosition, CursorPosition+new Vector2(10)), _engine.GameState.Solids);
            if (collisions.Count > 0) {
                CurrentObject = collisions[0];
                CursorPosition = CurrentObject.Position;
            }
        }

        public void LoadLevel(String path)
        {
            _engine = new GameEngine(_mapLoader.InitMap(path));
            _camera = new Camera(0.2f, Vector2.Zero, new Vector2(_graphicsDevice.PresentationParameters.BackBufferWidth, _graphicsDevice.PresentationParameters.BackBufferHeight));
            _camera.SetCameraToRectangle(new Rectangle(0, 0, 2000, 2000));

            _editorController = new EditorController(_engine, _camera, this);
            _gameController = new GameController(_engine, _camera);
            LoadContent();
        }

        void LoadContent()
        {
            _mapLoader.LoadMapContent(_engine.GameState);
            // The render instance is create per level: Like this we don't need to worry about resetting global variables in the renderer (e.g. lightning)
            _gameRenderer = new GameRenderer(_graphicsDevice, _engine.GameState, _content);
            _editorRenderer = new EditorRenderer(_graphicsDevice, _engine.GameState, _content, this);
        }


        public void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            GamePad.GetState(PlayerIndex.One);
            if ((gamePadState.IsButtonDown(Buttons.B) && _oldGamePadState.IsButtonUp(Buttons.B))
                || (keyboardState.IsKeyDown(Keys.Tab) && keyboardState.IsKeyUp(Keys.Tab)))
            {
                Editing = !Editing;
                if (Editing)
                {
                    _camera.SetCameraToRectangle(new Rectangle(0, 0, 2000, 2000));
                }
            }
            if (Editing)
            {
                _editorController.HandleUpdate(gameTime);
            }
            else
            {
                _gameController.HandleUpdate(gameTime);
            }

            _oldKeyboardState = Keyboard.GetState();
            _oldGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public void Draw(GameTime gameTime, int width, int height)
        {
            if (Editing)
            {
                GameManager.RenderDark = false;
            }

            _gameRenderer.Draw(gameTime, width, height, Keyboard.GetState().IsKeyDown(Keys.P) ? GameRenderer.Mode.DebugView : GameRenderer.Mode.Normal, _editorController.Camera.view);

            if (Editing)
            {
                _editorRenderer.Draw(gameTime, width, height, _editorController.Camera.view);
            }
            
        }

    }
}
