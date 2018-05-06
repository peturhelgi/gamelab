using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.Util;

namespace EditorLogic {
    class EditorController
    {
        public GameEngine GameEngine;

        public Camera Camera;

        EditorManager _manager;

        GamePadState _oldGamePadState;
        KeyboardState _oldKeyboardState;

        public EditorController(GameEngine gameEngine, Camera camera, EditorManager manager)
        {
            GameEngine = gameEngine;
            Camera = camera;
            _manager = manager;

            _oldKeyboardState = Keyboard.GetState();
            _oldGamePadState = GamePad.GetState(PlayerIndex.One);
        }


        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse(Mouse.GetState());


            HandleGamePad(GamePad.GetState(PlayerIndex.One));

            HandleKeyboard(Keyboard.GetState());

            _oldKeyboardState = Keyboard.GetState();
            _oldGamePadState = GamePad.GetState(PlayerIndex.One);

            GameEngine.Update();

        }

        private void HandleMouse(MouseState ms)
        {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                MyDebugger.WriteLine(ms.Position.X);
                MyDebugger.WriteLine(ms.Position.Y);
            }
        }

        private void HandleKeyboard(KeyboardState state)
        {
            // Handle camera
            if (state.IsKeyDown(Keys.A)) Camera.HandleAction(Camera.CameraAction.left);
            if (state.IsKeyDown(Keys.D)) Camera.HandleAction(Camera.CameraAction.right);
            if (state.IsKeyDown(Keys.W)) Camera.HandleAction(Camera.CameraAction.up);
            if (state.IsKeyDown(Keys.S)) Camera.HandleAction(Camera.CameraAction.down);

            // Handle Cursor movement
            if (state.IsKeyDown(Keys.Right)) _manager.CursorPosition += new Vector2(20, 0);
            if (state.IsKeyDown(Keys.Left)) _manager.CursorPosition += new Vector2(-20, 0);
            if (state.IsKeyDown(Keys.Down)) _manager.CursorPosition += new Vector2(0, 20);
            if (state.IsKeyDown(Keys.Up)) _manager.CursorPosition += new Vector2(0, -20);

        }

        private void HandleGamePad(GamePadState gamePadState)
        {
            // Handle camera with DPad
            //if (gamePadState.IsButtonDown(Buttons.DPadLeft)) Camera.HandleAction(Camera.CameraAction.left);
            //if (gamePadState.IsButtonDown(Buttons.DPadRight)) Camera.HandleAction(Camera.CameraAction.right);
            //if (gamePadState.IsButtonDown(Buttons.DPadUp)) Camera.HandleAction(Camera.CameraAction.up);
            //if (gamePadState.IsButtonDown(Buttons.DPadDown)) Camera.HandleAction(Camera.CameraAction.down);


            // when Y is pressed, open the picker wheel
            if (_manager.ObjectPickerOpen && gamePadState.ThumbSticks.Left.Length() > 0.5)
            {
                _manager.CircularSelector.Update(gamePadState.ThumbSticks.Left);
            }

            if (_manager.ObjectPickerOpen)
            {
                // go to the next category of GameObjects
                if (gamePadState.IsButtonDown(Buttons.RightShoulder) && _oldGamePadState.IsButtonUp(Buttons.RightShoulder))
                {
                    _manager.GetNextSelector();
                }

                // go to the previous category of GameObjects
                if (gamePadState.IsButtonDown(Buttons.LeftShoulder) && _oldGamePadState.IsButtonUp(Buttons.LeftShoulder))
                {
                    _manager.GetPrevSelector();
                }

                if (gamePadState.IsButtonDown(Buttons.RightTrigger) && _oldGamePadState.IsButtonUp(Buttons.RightTrigger))
                    _manager.ObjectPickerOpen = false;
                    
            }

            // On pressing Y, toggle between displaying ObjectPicker
            if (gamePadState.IsButtonDown(Buttons.Y) && _oldGamePadState.IsButtonUp(Buttons.Y))
            {
                _manager.ObjectPickerOpen = !_manager.ObjectPickerOpen;

                // when closing the ObjectPicker, choose the last selected GameObject
                if (_manager.ObjectPickerOpen == false)
                {
                    int itemNumber = _manager.CircularSelector.SelectedElement %
                           _manager.CircularSelector.NumberOfObjects();
                    _manager.CreateNewGameObject(_manager.CircularSelector.GetObjectAtIndex(itemNumber));
                }
            }
            else
            {
                // Handle Cursor movement
                Vector2 cursorDisplacement = new Vector2(50, -50) * 
                    new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
                _manager.CursorPosition += cursorDisplacement;

                _manager.CheckCursorInsideScreen(cursorDisplacement, _manager.CursorPosition);


                // enable object size changes, if we only select one object
                if (_manager.CurrentObjects != null)
                {
                    if (_manager.CurrentObjects.Count == 1)
                    {
                        _manager.CurrentObjects[0].SpriteSize += gamePadState.ThumbSticks.Right * new Vector2(5, -5);
                    }
                }


                // let A go > place Object or Pick Object(s)
                if (gamePadState.IsButtonUp(Buttons.A) && _oldGamePadState.IsButtonDown(Buttons.A))
                {
                    if (_manager.CurrentObjects != null)
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
                if (gamePadState.IsButtonUp(Buttons.X) && _oldGamePadState.IsButtonDown(Buttons.X))
                {
                    if (_manager.CurrentObjects != null)
                    {
                        _manager.PlaceCurrentObjects();
                    }
                    else
                    {
                        _manager.DuplicateObjectUnderCursor();
                        _manager.CursorSize = new Vector2(10);
                    }
                }


                if (gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.X))
                {
                    if (_manager.CurrentObjects == null)
                    {
                        // We are in selector mode

                        _manager.CursorSize += (new Vector2(50, 0) * gamePadState.ThumbSticks.Left.X);
                        _manager.CursorSize += (new Vector2(0, -50) * gamePadState.ThumbSticks.Left.Y);

                        _manager.CursorPosition -= (new Vector2(50, 0) * gamePadState.ThumbSticks.Left.X);
                        _manager.CursorPosition -= (new Vector2(0, -50) * gamePadState.ThumbSticks.Left.Y);

                    }

                }

                // remove current selection
                if (gamePadState.IsButtonDown(Buttons.LeftTrigger) && _oldGamePadState.IsButtonUp(Buttons.LeftTrigger))
                {
                    _manager.DeselectCurrentObjects();
                }

                // delete current object
                if (gamePadState.IsButtonDown(Buttons.RightTrigger) && _oldGamePadState.IsButtonUp(Buttons.RightTrigger))
                {
                    _manager.DeleteCurrentObject();
                }
            }

            ////on release of Y, pick the current object of the selector
            //if (gamePadState.IsButtonDown(Buttons.Y) && _oldGamePadState.IsButtonUp(Buttons.Y)
            //    && _manager.ObjectPickerOpen == true)
            //{
            //    _manager.ObjectPickerOpen = false;
            //    int itemNumber = _manager.CircularSelector.SelectedElement %
            //                _manager.CircularSelector.NumberOfObjects();
            //    _manager.CreateNewGameObject(_manager.CircularSelector.GetObjectAtIndex(itemNumber));
            //}





            if (gamePadState.IsButtonDown(Buttons.Back))
                // TODO: Ask the user to save the level
                //Exit();
                return;
        }

    }
}
