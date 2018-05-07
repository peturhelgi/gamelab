using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.IO;

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

        GamePadState _oldGamePadState, _currGamePadState;
        KeyboardState _oldKeyboardState, _currKeyboardState;
        MouseState _mouseState;

        public EditorController(GameEngine gameEngine, Camera camera, EditorManager manager)
        {
            GameEngine = gameEngine;
            Camera = camera;
            _manager = manager;

            _mouseState = Mouse.GetState();
            _currKeyboardState = Keyboard.GetState();
            _currGamePadState = GamePad.GetState(PlayerIndex.One);
        }


        internal void HandleUpdate(GameTime gameTime)
        {

            GameEngine.gameTime = gameTime.TotalGameTime;

            _oldGamePadState = _currGamePadState;
            _currGamePadState = GamePad.GetState(0);

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
        private void HandleSave()
        {
            string path = "Content\\" + GameEngine.GameState.levelname + ".json";
            Level level = GameEngine.GameState.GetPureLevel();
            String text = JsonConvert.SerializeObject(level);

            MyDebugger.WriteLine(text, true);
            //TODO: Put the text into the file

            /*
            IAsyncResult result1 = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(result1);

            IAsyncResult result = device.BeginOpenContainer("LevelSaver", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Dispose();

            if (container.FileExists(filename)) container.DeleteFile(filename);
            Stream stream = container.CreateFile(filename);
            TextWriter writer = new StreamWriter(stream);
            writer.Write(level);

            writer.Dispose();
            stream.Dispose();
            container.Dispose();*/
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
            if (_currKeyboardState.IsKeyDown(Keys.J)) HandleSave();

        }

        private bool KeyPressed(params Buttons[] buttons)
        {
            foreach (var button in buttons)
            {
                if (_oldGamePadState.IsButtonUp(button)
                    && _currGamePadState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }
        private bool KeyReleased(params Buttons[] buttons)
        {
            foreach (var button in buttons)
            {
                if (_oldGamePadState.IsButtonDown(button)
                    && _currGamePadState.IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }

        private bool KeyDown(params Buttons[] buttons)
        {
            foreach (var button in buttons)
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
            if (KeyDown(Buttons.DPadDown))
            {
                Camera.HandleAction(Camera.CameraAction.zoom_out);
            }
            if (KeyDown(Buttons.DPadUp))
            {
                Camera.HandleAction(Camera.CameraAction.zoom_in);
            }

            Vector2 leftThumb = _currGamePadState.ThumbSticks.Left,
                rightThumb = _currGamePadState.ThumbSticks.Right;


            // when Y is pressed, open the picker wheel

            if (_manager.ObjectPickerOpen)
            {

                if (leftThumb.Length() > 0.5)
                {
                    _manager.CircularSelector.Update(leftThumb);
                }

                // go to the next category of GameObjects
                if (KeyPressed(Buttons.RightShoulder))
                {
                    _manager.GetNextSelector();
                }

                // go to the previous category of GameObjects
                if (KeyPressed(Buttons.LeftShoulder))
                {
                    _manager.GetPrevSelector();
                }

                if (KeyPressed(Buttons.RightTrigger))
                    _manager.ObjectPickerOpen = false;
            }

            // On pressing Y, toggle between displaying ObjectPicker
            if (KeyPressed(Buttons.Y))
            {
                _manager.ObjectPickerOpen = !_manager.ObjectPickerOpen;

                // when closing the ObjectPicker, choose the last selected GameObject
                if (_manager.ObjectPickerOpen == false)
                {
                    int itemNumber = _manager.CircularSelector.SelectedElement %
                           _manager.CircularSelector.NumberOfObjects();
                    _manager.CreateNewGameObject(_manager.CircularSelector[itemNumber]);
                }
            }

            if (!_manager.ObjectPickerOpen)
            {
                // Handle Cursor movement
                Vector2 cursorDisplacement = new Vector2(50, -50) * leftThumb;
                _manager.CursorPosition += cursorDisplacement;

                //_manager.CheckCursorInsideScreen(cursorDisplacement, _manager.CursorPosition);
            }


            // enable object size changes, if we only select one object
            if (_manager.CurrentObjects != null)
            {
                if (_manager.CurrentObjects.Count == 1)
                {
                    _manager.CurrentObjects[0].SpriteSize
                        += rightThumb * new Vector2(5, -5);
                    float x = _manager.CurrentObjects[0].SpriteSize.X,
                        y = _manager.CurrentObjects[0].SpriteSize.Y;
                    //make sure that object doesn't get negative size
                    if (x < 0) { x = 0; }
                    if (y < 0) { y = 0; }
                    _manager.CurrentObjects[0].SpriteSize =
                        new Vector2(x, y);
                }
            }


            if (_manager.AuxiliaryObject != null)
            {
                _manager.AuxiliaryObject.SpriteSize += rightThumb * new Vector2(5, -5);
                // Remove the key
                if (KeyPressed(Buttons.RightTrigger))
                {
                    _manager.RemoveAuxiliaryObject();
                }
                else if (KeyPressed(Buttons.X, Buttons.A))
                {
                    _manager.PlaceAuxiliaryObject();
                }

            }
            else
            {
                // let A go > place Object or Pick Object(s)
                if (KeyReleased(Buttons.A))
                {
                    if (_manager.ObjectsAreSelected)
                    {
                        _manager.PlaceCurrentObjects();
                    }
                    else
                    {
                        _manager.PickObjectUnderCursor();
                        _manager.CursorSize = new Vector2(10);
                    }
                }

                // let X go > place Object or duplicate Object(s)
                if (KeyReleased(Buttons.X))
                {
                    if (_manager.ObjectsAreSelected)
                    {
                        _manager.PlaceCurrentObjects();
                    }

                    else
                    {
                        _manager.DuplicateObjectUnderCursor();
                        _manager.CursorSize = new Vector2(10);
                    }
                }
            }

            if (KeyDown(Buttons.A, Buttons.X))
            {
                if (_manager.CurrentObjects == null)
                {
                    // We are in selector mode

                    _manager.CursorSize += (new Vector2(50, -50) * leftThumb);
                    _manager.CursorPosition -= (new Vector2(50, -50) * leftThumb);
                }

            }

            // remove current selection
            if (KeyPressed(Buttons.LeftTrigger))
            {
                _manager.DeselectCurrentObjects();
            }

            // delete current object
            if (KeyPressed(Buttons.RightTrigger))
            {
                _manager.DeleteCurrentObject();
            }


            if (KeyPressed(Buttons.Back))
                // TODO: Ask the user to save the level
                //Exit();
                return;
        }

    }
}
