using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.Collision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGreatEscape.GameLogic.Util;

namespace EditorLogic
{
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
            // START Handle camera
            if (state.IsKeyDown(Keys.A)) Camera.HandleAction(Camera.CameraAction.left);
            if (state.IsKeyDown(Keys.D)) Camera.HandleAction(Camera.CameraAction.right);
            if (state.IsKeyDown(Keys.W)) Camera.HandleAction(Camera.CameraAction.up);
            if (state.IsKeyDown(Keys.S)) Camera.HandleAction(Camera.CameraAction.down);
            // END Handle camera

            // START Handle Cursor movement
            // Player 1
            if (state.IsKeyDown(Keys.Right)) _manager.CursorPosition += new Vector2(20, 0);
            if (state.IsKeyDown(Keys.Left)) _manager.CursorPosition += new Vector2(-20, 0);
            if (state.IsKeyDown(Keys.Down)) _manager.CursorPosition += new Vector2(0, 20);
            if (state.IsKeyDown(Keys.Up)) _manager.CursorPosition += new Vector2(0, -20);





        }

        private void HandleGamePad(GamePadState gamePadState)
        {

            if (gamePadState.IsButtonDown(Buttons.DPadLeft)) Camera.HandleAction(Camera.CameraAction.left);
            if (gamePadState.IsButtonDown(Buttons.DPadRight)) Camera.HandleAction(Camera.CameraAction.right);
            if (gamePadState.IsButtonDown(Buttons.DPadUp)) Camera.HandleAction(Camera.CameraAction.up);
            if (gamePadState.IsButtonDown(Buttons.DPadDown)) Camera.HandleAction(Camera.CameraAction.down);


            _manager.CursorPosition += (new Vector2(50, 0) * gamePadState.ThumbSticks.Left.X);
            _manager.CursorPosition += (new Vector2(0, -50) * gamePadState.ThumbSticks.Left.Y);


            if (_manager.CurrentObjects != null)
            {
                if (_manager.CurrentObjects.Count == 1)
                {
                    _manager.CurrentObjects[0].SpriteSize += gamePadState.ThumbSticks.Right * new Vector2(5, -5);
                }
            }

            if (gamePadState.IsButtonDown(Buttons.Y) && _oldGamePadState.IsButtonUp(Buttons.Y))
            {
                _manager.CreateNewGameObject(null);
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


            if (gamePadState.IsButtonDown(Buttons.A))
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
            else
            {
                // only if A is not pressed
                // Press left trigger to deselect current selection
                if (gamePadState.IsButtonDown(Buttons.LeftTrigger) && _oldGamePadState.IsButtonUp(Buttons.LeftTrigger))
                {
                    _manager.DeselectCurrentObjects();
                }
            }

            if (gamePadState.IsButtonDown(Buttons.RightTrigger) && _oldGamePadState.IsButtonUp(Buttons.RightTrigger))
            {
                _manager.DeleteCurrentObject();
            }

            if (gamePadState.IsButtonDown(Buttons.Back))
                // TODO: Ask the user to save the level
                //Exit();
                return;
        }

    }
}
