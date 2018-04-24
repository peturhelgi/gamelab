using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project.GameLogic;
using Project.GameLogic.Collision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EditorLogic
{
    class EditorController
    {
        public GameEngine GameEngine;

        public Camera Camera;

        EditorManager _manager;


        public EditorController(GameEngine gameEngine, Camera camera, EditorManager manager)
        {
            GameEngine = gameEngine;
            Camera = camera;
            _manager = manager;
        }


        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse(Mouse.GetState());
            GamePadState PlayerOneState = GamePad.GetState(PlayerIndex.One);

            HandleGamePad(PlayerOneState, 0);

              


            HandleKeyboard(Keyboard.GetState());

            GameEngine.Update();

            


        }

        private void HandleMouse(MouseState ms)
        {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine(ms.Position.X);
                Debug.WriteLine(ms.Position.Y);
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
            if (state.IsKeyDown(Keys.Right)) _manager.CursorPosition += new Vector2(20,0);
            if (state.IsKeyDown(Keys.Left)) _manager.CursorPosition += new Vector2(-20, 0);
            if (state.IsKeyDown(Keys.Down)) _manager.CursorPosition += new Vector2(0, 20);
            if (state.IsKeyDown(Keys.Up)) _manager.CursorPosition += new Vector2(0, -20);

            



        }

        private void HandleGamePad(GamePadState gamePadState, int player)
        {
            if(player != 0 )
            {
                return;
            }

           
            if (gamePadState.IsButtonDown(Buttons.DPadLeft)) Camera.HandleAction(Camera.CameraAction.left);
            if (gamePadState.IsButtonDown(Buttons.DPadRight)) Camera.HandleAction(Camera.CameraAction.right);
            if (gamePadState.IsButtonDown(Buttons.DPadUp)) Camera.HandleAction(Camera.CameraAction.up);
            if (gamePadState.IsButtonDown(Buttons.DPadDown)) Camera.HandleAction(Camera.CameraAction.down);

            // START Handle GameAction
            _manager.CursorPosition += (new Vector2(100, 0) * gamePadState.ThumbSticks.Left.X);
            _manager.CursorPosition += (new Vector2(0, -100) * gamePadState.ThumbSticks.Left.Y);
            
            // END Handle GameAction

            if (gamePadState.IsButtonDown(Buttons.Back))
                // TODO: Add a changed GameState, to escape the game
                //Exit();
                return;
        }

    }
}
