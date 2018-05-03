using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.LevelManager;
using TheGreatEscape.GameLogic.Renderer;
using System;
using System.Collections.Generic;
using TheGreatEscape.EditorLogic.Util;

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
        public Vector2 CursorSize;
        public Vector2 MovingStartPosition;
        public bool Editing = true;
        public List<GameObject> CurrentObjects;
        public bool CurrentIsNewObject;
        public bool ObjectPickerOpen;

        public List<GameObject> ObjectTemplates;
        public CircularSelector CircularSelector;



        public EditorManager(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _mapLoader = new MapLoader(content);
            CursorPosition = Vector2.Zero;
            CursorSize = new Vector2(10);
            

            _oldKeyboardState = Keyboard.GetState();
            _oldGamePadState = GamePad.GetState(PlayerIndex.One);


            // TODO: Move to GameObject factory
            // Templates.Add(new Miner(Vector2.Zero, new Vector2(100, 150), Vector2.Zero, 80, "Sprites/Miners/MinerHandsInPants"));
            GameObjectFactory factory = new GameObjectFactory();
            Obj rock = new Obj
            {
                Type = "rock",
                Position = Vector2.Zero,
                SpriteSize = new Vector2(150, 100)
            };

            ObjectTemplates = new List<GameObject>();
            for(int i = 1; i <= 5; ++i)
            {
                rock.Texture = "Sprites/Rocks/BareRock" + i;
                ObjectTemplates.Add(factory.Create(rock));
            }
            foreach (GameObject obj in ObjectTemplates) {
                obj.Texture = content.Load<Texture2D>(obj.TextureString);
            }

            CircularSelector = new CircularSelector(_content);
            CircularSelector.SetObjects(ObjectTemplates);

        }

        public void DeselectCurrentObjects()
        {
            if (CurrentObjects != null && !CurrentIsNewObject)
            {
                foreach (var obj in CurrentObjects)
                {
                    _engine.GameState.Solids.Add(obj);
                }
            }
            CurrentObjects = null;
        }

        public void DeleteCurrentObject()
        {
            if (CurrentObjects != null)
            {
                foreach (var obj in CurrentObjects)
                {
                    _engine.GameState.Solids.Remove(obj);
                }
            }
            CurrentObjects = null;
        }

        public void CreateNewGameObject(GameObject newObject)
        {
            CurrentObjects = new List<GameObject>
            {
                GameObject.Clone(newObject)
            };
            
            CurrentIsNewObject = true;
            MovingStartPosition =  Vector2.Zero;

        }

        public void PlaceCurrentObjects() {
            if (CurrentObjects != null) {
                foreach (GameObject obj in CurrentObjects)
                {
                    obj.Position += (CursorPosition - MovingStartPosition);
                    _engine.GameState.SetObject(obj);
                }
                CurrentObjects = null;
            }
        }

        public void DuplicateObjectUnderCursor()
        {
            List<GameObject> collisions = _engine.CollisionDetector.FindCollisions(new AxisAllignedBoundingBox(CursorPosition, CursorPosition + CursorSize), _engine.GameState.Solids);
            if (collisions.Count > 0)
            {
                CurrentObjects = new List<GameObject>();
                CursorPosition = new Vector2(float.MaxValue);
                foreach (GameObject obj in collisions)
                {
                    CursorPosition = Vector2.Min(CursorPosition, obj.Position);
                    CurrentObjects.Add(GameObject.Clone(obj));
                }
                MovingStartPosition = CursorPosition;
                CurrentIsNewObject = true;
            }
        }

        public void PickObjectUnderCursor() {
            List<GameObject> collisions = _engine.CollisionDetector.FindCollisions(new AxisAllignedBoundingBox(CursorPosition, CursorPosition+CursorSize), _engine.GameState.Solids);
            if (collisions.Count > 0) {
                CurrentObjects = collisions;
                CursorPosition = new Vector2(float.MaxValue);
                foreach (GameObject obj in collisions) {
                    CursorPosition = Vector2.Min(CursorPosition, obj.Position);
                    _engine.GameState.Solids.Remove(obj);
                }
                MovingStartPosition = CursorPosition;
                CurrentIsNewObject = false;
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
                || (keyboardState.IsKeyDown(Keys.Tab) && _oldKeyboardState.IsKeyUp(Keys.Tab)))
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

            _gameRenderer.Draw(gameTime, width, height, Keyboard.GetState().IsKeyDown(Keys.P) ? GameRenderer.Mode.DebugView : GameRenderer.Mode.Normal, _editorController.Camera);

            if (Editing)
            {
                _editorRenderer.Draw(gameTime, width, height, _editorController.Camera.view);
            }
            
        }

    }
}
