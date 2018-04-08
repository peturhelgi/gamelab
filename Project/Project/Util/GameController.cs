using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project.Util
{
    class GameController
    {
        public GameEngine GameEngine;

        public Camera Camera;


        public GameController(GameEngine gameEngine, Camera camera) {
            GameEngine = gameEngine;
            Camera = camera;
        }

        internal void HandleUpdate(GameTime gameTime)
        {
            HandleMouse( Mouse.GetState());
            GamePadState PlayerOneState = GamePad.GetState(PlayerIndex.One);
            if (PlayerOneState.IsConnected)
            {
                HandleGamePad(PlayerOneState);
                
                // There can only be a second player, if there is a first one
                GamePadState PlayerTwoState = GamePad.GetState(PlayerIndex.Two);
                if (PlayerTwoState.IsConnected)
                    HandleGamePad(PlayerTwoState);
            }


            HandleKeyboard(Keyboard.GetState());

            GameEngine.Update(gameTime);
            
        }

        private void HandleMouse(MouseState ms) {
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
            if (state.IsKeyDown(Keys.Z)) Camera.HandleAction(Camera.CameraAction.zoom_out);
            if (state.IsKeyDown(Keys.X)) Camera.HandleAction(Camera.CameraAction.zoom_in);
            // END Handle camera

            // START Handle GameAction
            if (state.IsKeyDown(Keys.Right)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_right, 0);
            if (state.IsKeyDown(Keys.Left)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_left, 0);
            if (state.IsKeyDown(Keys.I)) GameEngine.HandleInput(0, GameEngine.GameAction.interact, 0);
            if (state.IsKeyDown(Keys.Space)) GameEngine.HandleInput(0, GameEngine.GameAction.jump, 0);

            // END Handle GameAction




        }

        private void HandleGamePad(GamePadState gs)
        {
            if (gs.Buttons.Back == ButtonState.Pressed)
                // TODO: Add a changed GameState, to escape the game
                //Exit();
                return;
        }
        
    }
}
