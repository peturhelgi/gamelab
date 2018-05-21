using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using Project.LeveManager;
using System;
using System.IO;

using System.IO.IsolatedStorage;

using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.GameLogic.Util;
using TheGreatEscape.LevelManager;

namespace EditorLogic
{
    class EditorController
    {
        public GameEngine GameEngine;

        public Camera Camera;

        EditorManager _manager;

        JsonUtil<Level> _saver;

        Vector2 _initCursorSize, _deltaCursorSize, _resizeDelta;

        GamePadState _oldGamePadState, _currGamePadState;
        KeyboardState _oldKeyboardState, _currKeyboardState;
        MouseState _mouseState;

        enum Command
        {
            Copy,
            Deselect,
            Exit,
            NextItem,
            Place,
            PreviousItem,
            Remove,
            Select,
            ToggleMode,
            TogglePicker,
            ZoomIn,
            ZoomOut,
            ShowHelp
        }


        Dictionary<Command, List<Buttons>> _buttons = new Dictionary<Command, List<Buttons>>();

        public EditorController(GameEngine gameEngine, Camera camera, EditorManager manager)
        {
            _buttons[Command.Select] = new List<Buttons> { Buttons.A };
            _buttons[Command.Place] = new List<Buttons> { Buttons.A, Buttons.X };
            _buttons[Command.ToggleMode] = new List<Buttons> { Buttons.B };
            _buttons[Command.TogglePicker] = new List<Buttons> { Buttons.Y };
            _buttons[Command.Copy] = new List<Buttons> { Buttons.X };
            _buttons[Command.ZoomIn] = new List<Buttons> { Buttons.DPadUp };
            _buttons[Command.ZoomOut] = new List<Buttons> { Buttons.DPadDown };

            _buttons[Command.Remove] = new List<Buttons> { Buttons.RightTrigger };
            _buttons[Command.Deselect] = new List<Buttons> { Buttons.LeftTrigger };
            _buttons[Command.NextItem] = new List<Buttons> { Buttons.RightShoulder };
            _buttons[Command.PreviousItem] = new List<Buttons> { Buttons.LeftShoulder };
            _buttons[Command.ShowHelp] = new List<Buttons> { Buttons.Start };

            _buttons[Command.Exit] = new List<Buttons> { Buttons.Back };


            GameEngine = gameEngine;
            Camera = camera;
            _manager = manager;

            _saver = new JsonUtil<Level>();


            _initCursorSize = new Vector2(10);
            _deltaCursorSize = new Vector2(50, -50);
            _resizeDelta = new Vector2(5, -5);
            _mouseState = Mouse.GetState();
            _currKeyboardState = Keyboard.GetState();
            _currGamePadState = GamePad.GetState(PlayerIndex.One);
        }


        internal void HandleUpdate(GameTime gameTime)
        {

            GameEngine.gameTime = gameTime.TotalGameTime;

            _oldGamePadState = _currGamePadState;
            _currGamePadState = GamePad.GetState(PlayerIndex.One);

            _oldKeyboardState = _currKeyboardState;
            _currKeyboardState = Keyboard.GetState();

            _mouseState = Mouse.GetState();

            HandleGamePad();
            HandleKeyboard();
            HandleMouse();

            GameEngine.Update();

        }

        private void HandleMouse()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                MyDebugger.Write("(" + _mouseState.Position.X + ", ", true);
                MyDebugger.WriteLine(_mouseState.Position.Y + ")", true);
            }
        }

        private void HandleSave(string levelname)
        {
            string path = "Levels/" + levelname + ".json";
            Level level = GameEngine.GameState.GetPureLevel();
            _saver.Save(path, level);
        }

        private void HandleKeyboard()
        {
            // Handle camera
            if (_currKeyboardState.IsKeyDown(Keys.A)) Camera.HandleAction(Camera.CameraAction.left);
            if (_currKeyboardState.IsKeyDown(Keys.D)) Camera.HandleAction(Camera.CameraAction.right);
            if (_currKeyboardState.IsKeyDown(Keys.W)) Camera.HandleAction(Camera.CameraAction.up);
            if (_currKeyboardState.IsKeyDown(Keys.S)) Camera.HandleAction(Camera.CameraAction.down);

            // Handle Cursor movement
            if (_currKeyboardState.IsKeyDown(Keys.Right)) _manager.CursorPosition += new Vector2(20, 0);
            if (_currKeyboardState.IsKeyDown(Keys.Left)) _manager.CursorPosition += new Vector2(-20, 0);
            if (_currKeyboardState.IsKeyDown(Keys.Down)) _manager.CursorPosition += new Vector2(0, 20);
            if (_currKeyboardState.IsKeyDown(Keys.Up)) _manager.CursorPosition += new Vector2(0, -20);

            // Handle saving changes
            if (_currKeyboardState.IsKeyDown(Keys.J)) HandleSave(GameEngine.GameState.levelname);

        }

        private bool KeyPressed(Command command)
        {
            if (!_buttons.ContainsKey(command)) return false;
            foreach (var button in _buttons[command])
            {
                if (_oldGamePadState.IsButtonUp(button)
                    && _currGamePadState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        private bool KeyReleased(Command command)
        {
            if (!_buttons.ContainsKey(command)) return false;
            foreach (var button in _buttons[command])
            {
                if (_oldGamePadState.IsButtonDown(button)
                    && _currGamePadState.IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }

        private bool KeyDown(Command command)
        {
            if (!_buttons.ContainsKey(command)) return false;
            foreach (var button in _buttons[command])
            {
                if (_currGamePadState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        private void HandleGamePad()
        {
            // Handle camera with DPad
            if (KeyDown(Command.ZoomOut))
            {
                Camera.HandleAction(Camera.CameraAction.zoom_out);
            }
            if (KeyDown(Command.ZoomIn))
            {
                Camera.HandleAction(Camera.CameraAction.zoom_in);
            }

            Vector2 leftThumb = _currGamePadState.ThumbSticks.Left,
                rightThumb = _currGamePadState.ThumbSticks.Right;


            if (_manager.ObjectPickerOpen)
            {

                if (leftThumb.Length() > 0.5)
                {
                    _manager.CircularSelector.Update(leftThumb);
                }

                // go to the next category of GameObjects
                if (KeyPressed(Command.NextItem))
                {
                    _manager.GetNextSelector();
                }

                // go to the previous category of GameObjects
                if (KeyPressed(Command.PreviousItem))
                {
                    _manager.GetPrevSelector();
                }
            }
            else
            {
                // Handle Cursor movement
                Vector2 cursorDisplacement = _deltaCursorSize * leftThumb;
                _manager.CursorPosition += cursorDisplacement;
                _manager.CheckCursorInsideScreen(cursorDisplacement, _manager.CursorPosition);
            }

            // On pressing Y, toggle between displaying ObjectPicker
            if (KeyPressed(Command.TogglePicker))
            {
                _manager.ObjectPickerOpen = !_manager.ObjectPickerOpen;
            }

            if (KeyPressed(Command.Select))
            {
                // when closing the ObjectPicker, choose the last selected GameObject
                if (_manager.ObjectPickerOpen)
                {
                    int itemNumber = _manager.CircularSelector.SelectedElement %
                           _manager.CircularSelector.NumberOfObjects();
                    _manager.CreateNewGameObject(_manager.CircularSelector[itemNumber]);
                    _manager.ObjectPickerOpen = false;
                    return;
                }
            }

            // enable object size changes, if we only select one object
            if (_manager.CurrentObjects != null)
            {
                // Only call this method if the right thumb is actually used
                int minSizeX = 10, minSizeY = 10;
                if (_manager.CurrentObjects.Count == 1 && rightThumb.Length() > 0.25f)
                {
                    _manager.CurrentObjects[0].SpriteSize
                        += rightThumb * _resizeDelta;
                    float x = _manager.CurrentObjects[0].SpriteSize.X,
                        y = _manager.CurrentObjects[0].SpriteSize.Y;
                    //make sure that object doesn't get negative size
                    if (x < minSizeX) { x = minSizeX; }
                    if (y < minSizeY) { y = minSizeY; }
                    _manager.CurrentObjects[0].SpriteSize =
                        new Vector2(x, y);
                }
            }

 
            if (_manager.AuxiliaryObject != null)
            {
                _manager.AuxiliaryObject.SpriteSize += rightThumb * _resizeDelta;
                // Remove the key
                if (KeyPressed(Command.Remove))
                {
                    _manager.RemoveAuxiliaryObject();
                }
                else if (KeyPressed(Command.Place))
                {
                    _manager.PlaceAuxiliaryObject();
                }
            }
            else
            {
                // let A pressed => place Object or Pick Object(s)
                if (KeyPressed(Command.Select))
                {

                    if (_manager.ObjectsAreSelected)
                    {
                        _manager.PlaceCurrentObjects();
                    }
                    else
                    {
                        _manager.PickObjectUnderCursor();
                        _manager.CursorSize = _initCursorSize;
                    }
                }

                // let X pressed => place Object or duplicate Object(s)
                if (KeyPressed(Command.Copy))
                {
                    if (_manager.ObjectsAreSelected)
                    {
                        _manager.PlaceCurrentObjects();
                    }

                    else
                    {
                        _manager.DuplicateObjectUnderCursor();
                        _manager.CursorSize = _initCursorSize;
                    }
                }
            }

            if (KeyDown(Command.Place))
            {
                if (_manager.CurrentObjects == null)
                {
                    // We are in selector mode
                    _manager.CursorSize += (_deltaCursorSize * leftThumb);
                    _manager.CursorPosition -= (_deltaCursorSize * leftThumb);
                }
            }

            // remove current selection
            if (KeyPressed(Command.Deselect))
            {
                _manager.DeselectCurrentObjects();
            }

            // delete current object
            if (KeyPressed(Command.Remove))
            {
                _manager.DeleteCurrentObject();
            }

            if (KeyPressed(Command.Exit))
                // TODO: Ask the user to save the level
                //Exit();
                return;
        }
    }
}
