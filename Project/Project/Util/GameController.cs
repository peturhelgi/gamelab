using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project.Util {
    class GameController {
        //public GameEngine GameEngine;
        public Camera Camera;

        public GameController(Camera camera) {
            this.Camera = camera;
        }

        internal virtual void HandleUpdate(GameTime gameTime) {
            GameEngine.Instance.gameTimeSpan = gameTime.TotalGameTime;
            HandleMouse(Mouse.GetState());
            GamePadState PlayerOneState = GamePad.GetState(PlayerIndex.One);
            if(PlayerOneState.IsConnected) {
                HandleGamePad(PlayerOneState, gameTime);

                // There can only be a second player, if there is a first one
                GamePadState PlayerTwoState = GamePad.GetState(PlayerIndex.Two);
                if(PlayerTwoState.IsConnected) {
                    HandleGamePad(PlayerTwoState, gameTime);
                }
            }

            HandleKeyboard(Keyboard.GetState(), gameTime);
        }

        public virtual void HandleMouse(MouseState ms) {
            if(ms.LeftButton == ButtonState.Pressed) {
                Debug.Write(ms.Position.X + ", " );
                Debug.WriteLine(ms.Position.Y);
            }
        }

        public virtual void HandleKeyboard(KeyboardState state, GameTime gameTime) {
            // START Handle camera
            if(state.IsKeyDown(Keys.A)) Camera.HandleAction(Camera.CameraAction.left);
            if(state.IsKeyDown(Keys.D)) Camera.HandleAction(Camera.CameraAction.right);
            if(state.IsKeyDown(Keys.W)) Camera.HandleAction(Camera.CameraAction.up);
            if(state.IsKeyDown(Keys.S)) Camera.HandleAction(Camera.CameraAction.down);
            if(state.IsKeyDown(Keys.Z)) Camera.HandleAction(Camera.CameraAction.zoom_out);
            if(state.IsKeyDown(Keys.X)) Camera.HandleAction(Camera.CameraAction.zoom_in);
            // END Handle camera

            // START Handle GameAction
            if(state.IsKeyDown(Keys.Right)) {
                GameEngine.Instance.HandleInput(
                    0, GameEngine.GameAction.walk_right, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.Left)) {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.walk_left, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.I)) {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.interact, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.Space)) {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.jump, 0, gameTime);
            }

            // END Handle GameAction
        }

        public virtual void HandleGamePad(GamePadState gs, GameTime gameTime) {
            if(gs.Buttons.Back == ButtonState.Pressed) {
                // TODO: Add a changed GameState, to escape the game
                //Exit();
                return;
            }

            // pressing A makes the character jump
            if (gs.IsButtonDown(Buttons.A))
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.jump, 0, gameTime);

            if (gs.IsButtonDown(Buttons.RightTrigger))
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.interact, 0, gameTime);

            // Left thumbstick controls
            if (gs.ThumbSticks.Left.X < -0.5f)
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.walk_left, 0, gameTime);
            if (gs.ThumbSticks.Left.X > 0.5f)
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.walk_right, 0, gameTime);

        }

    }
}
