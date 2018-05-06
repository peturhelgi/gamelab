﻿using Microsoft.Xna.Framework;
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
using System.Linq;

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
        public GameObject AuxiliaryObject;
        public bool CurrentIsNewObject;
        public bool ObjectPickerOpen;
        public Rectangle InitialRectangle;

        public SortedList<String, CircularSelector> ObjectsSelector;
        public CircularSelector CircularSelector;
        public Dictionary<string, Dictionary<string, Texture2D>> GameObjectTextures;

        public bool ObjectsAreSelected
        {
            get
            {
                return ((CurrentObjects != null) || (AuxiliaryObject != null));
            }
        }
        public EditorManager(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _mapLoader = new MapLoader(content);
            CursorPosition = Vector2.Zero;
            CursorSize = new Vector2(10);
            InitialRectangle = new Rectangle(0, 0, 2000, 2000);


            _oldKeyboardState = Keyboard.GetState();
            _oldGamePadState = GamePad.GetState(PlayerIndex.One);

            GameObjectTextures = new Dictionary<string, Dictionary<string, Texture2D>>();
            ObjectsSelector = new SortedList<string, CircularSelector>();
            LoadAllGameObjects();
            CircularSelector = ObjectsSelector.First().Value;

        }

        public void LoadAllGameObjects()
        {
            List<String> DirNames = new List<String> {
                "Interactables", "Grounds", "Rocks"
            };

            GameObjectFactory factory = new GameObjectFactory();

            foreach (String dirName in DirNames)
            {
                Dictionary<string, Texture2D> objectTextures = TextureContent.LoadListContent
                    <Texture2D>(_content, dirName);
                List<GameObject> ObjectTemplates = new List<GameObject>();

                GameObjectTextures.Add(dirName, objectTextures);

                foreach (var gameObj in objectTextures)
                {
                    // the type of the object to create is extracted from the 
                    // sprite name
                    string objType = gameObj.Key.Split('/').Last();
                    // for more objects of the same type, the delimitor used is underscore
                    objType = objType.Split('_').First().ToLower();

                    Obj gobj = new Obj
                    {
                        Type = objType,
                        Position = Vector2.Zero,
                        SpriteSize = new Vector2(150, 100)
                    };

                    // the dummy instance of the key shouldn't affect
                    // the total number of keys in the GameState
                    if (objType == "key")
                    {
                        gobj.Id = -1;
                        GameObjectFactory.currentKey--;
                    }
                    GameObject gameObject = factory.Create(gobj);
                    gameObject.Texture = gameObj.Value;
                    ObjectTemplates.Add(gameObject);

                    //if (objType == "key")
                    //    gameObj.ToString();
                }

                CircularSelector circSelector = new CircularSelector(_content, dirName);
                circSelector.SetObjects(ObjectTemplates);
                ObjectsSelector.Add(dirName, circSelector);
            }

        }

        public List<GameObject> GetAllObjectsOfType(Type ObjType)
        {
            List<GameObject> desiredObjects = new List<GameObject>();
            foreach (GameObject gameObj in _engine.GameState.GetAll())
            {
                if (gameObj.GetType() == ObjType)
                {
                    desiredObjects.Add(gameObj);
                }
            }
            return desiredObjects;
        }
        public void GetNextSelector()
        {
            int indexOfNext = ObjectsSelector.IndexOfKey(CircularSelector.SelectableObjects) + 1;
            indexOfNext %= ObjectsSelector.Count;
            CircularSelector = ObjectsSelector.Values[indexOfNext];
        }

        public void GetPrevSelector()
        {
            int indexOfPrev = ObjectsSelector.IndexOfKey(CircularSelector.SelectableObjects) - 1;
            if (indexOfPrev < 0)
                indexOfPrev += ObjectsSelector.Count;
            CircularSelector = ObjectsSelector.Values[indexOfPrev];
        }

        public void DeselectCurrentObjects()
        {
            if (CurrentObjects != null && !CurrentIsNewObject)
            {
                foreach (var obj in CurrentObjects)
                {
                    if (obj is Miner)
                        continue;
                    _engine.GameState.Add(obj);
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
                    _engine.GameState.Remove(obj);
                }
            }
            CurrentObjects = null;
        }

        // Called only when placing an object from the PickerWheel
        public void CreateNewGameObject(GameObject newObject)
        {
            CurrentObjects = new List<GameObject>
            {
                GameObject.Clone(newObject)
            };

            CurrentIsNewObject = true;
            MovingStartPosition = Vector2.Zero;

        }

        public void CreateDoorKey()
        {
            Obj gobj = new Obj
            {
                Type = "key",
                Position = CursorPosition,
                SpriteSize = new Vector2(150, 100),
                Id = GameObjectFactory.currentKey
            };
            GameObjectFactory factory = new GameObjectFactory();
            AuxiliaryObject = factory.Create(gobj);
            GameObjectTextures.TryGetValue("Interactables", out Dictionary<string, Texture2D> keyTexDict);
            AuxiliaryObject.Texture = keyTexDict["key"];

            (CurrentObjects.First() as Door).AddKey((AuxiliaryObject as Key).Id);
        }

        public void PlaceCurrentObjects()
        {
            if (CurrentObjects != null)
            {
                if (CurrentObjects.First() is Door)
                {
                    CreateDoorKey();
                    Door door = CurrentObjects.First() as Door;
                    door.Position += (CursorPosition - MovingStartPosition);
                    MovingStartPosition = CursorPosition;
                    _engine.GameState.Add(door);
                    CurrentObjects = null;
                    return;

                }
                foreach (GameObject obj in CurrentObjects)
                {
                    obj.Position += (CursorPosition - MovingStartPosition);
                    if (!(obj is Miner))
                        _engine.GameState.Add(obj);
                }
                CurrentObjects = null;
            }
        }

        public void PlaceAuxiliaryObject()
        {
            if (AuxiliaryObject != null)
            {
                AuxiliaryObject.Position += (CursorPosition - MovingStartPosition);
                _engine.GameState.Add(AuxiliaryObject);
            }
            AuxiliaryObject = null;
        }

        public void RemoveAuxiliaryObject()
        {
            List<GameObject> doors = GetAllObjectsOfType(typeof(Door));
            foreach (Door door in doors)
            {
                if (door.KeyId == (AuxiliaryObject as Key).Id)
                    door.RemoveKey(door.KeyId);
            }
            AuxiliaryObject = null;
        }

        public void DuplicateObjectUnderCursor()
        {
            List<GameObject> collisions = _engine.CollisionDetector.FindCollisions
                (new AxisAllignedBoundingBox(CursorPosition, CursorPosition + CursorSize), _engine.GameState.GetAll());
            if (collisions.Count > 0)
            {
                CurrentObjects = new List<GameObject>();
                CursorPosition = new Vector2(float.MaxValue);
                foreach (GameObject obj in collisions)
                {
                    if (!(obj is Miner))
                        CurrentObjects.Add(GameObject.Clone(obj));
                    CursorPosition = Vector2.Min(CursorPosition, obj.Position);
                }
                MovingStartPosition = CursorPosition;
                CurrentIsNewObject = true;
            }
        }

        public void PickObjectUnderCursor()
        {
            List<GameObject> collisions = _engine.CollisionDetector.FindCollisions(
                new AxisAllignedBoundingBox(CursorPosition, CursorPosition + CursorSize), _engine.GameState.GetAll());
            if (collisions.Count > 0)
            {
                CurrentObjects = collisions;
                CursorPosition = new Vector2(float.MaxValue);
                foreach (GameObject obj in collisions)
                {
                    CursorPosition = Vector2.Min(CursorPosition, obj.Position);
                    if (!(obj is Miner))
                        _engine.GameState.Remove(obj);
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
            _oldGamePadState = GamePad.GetState(0);
        }

        public void Draw(GameTime gameTime, int width, int height)
        {

            _gameRenderer.Draw(gameTime, width, height, 
                Keyboard.GetState().IsKeyDown(Keys.P) ? 
                GameRenderer.Mode.DebugView : 
                GameRenderer.Mode.Normal, _editorController.Camera);

            if (Editing)
            {
                GameManager.RenderDark = false;
                _editorRenderer.Draw(gameTime, width, height, _editorController.Camera.view);
            }

        }

        public void CheckCursorInsideScreen(Vector2 cursorDisplacement, Vector2 cursorPosition)
        {
            if (!InitialRectangle.Contains(cursorPosition))
            {
                InitialRectangle.Offset(cursorDisplacement);
                _camera.SetCameraToRectangle(InitialRectangle);
            }
        }

    }
}
