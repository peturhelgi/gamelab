using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;

namespace Project.Controls {
    public class MenuControls : GameController {

        enum things {
            Select = Buttons.A,
            Back = Buttons.B,
            Up = Buttons.DPadUp,
            UpStick = Buttons.LeftThumbstickUp,
            Down = Buttons.LeftThumbstickUp,
            DownStick = Buttons.LeftThumbstickDown
        };
        public MenuControls() {

        }

        internal override void HandleUpdate(GameTime gameTime) {
            GameEngine.Instance.gameTimeSpan = gameTime.TotalGameTime;
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

        private void HandleKeyboard(KeyboardState state, GameTime gameTime) {
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

        private void HandleGamePad(GamePadState gs, GameTime gameTime) {
            var i = Buttons.LeftThumbstickUp;
            gs.IsButtonDown((Buttons)things.Up);
            if(gs.Buttons.Back == ButtonState.Pressed) {
                // TODO: Add a changed GameState, to escape the game
                //Exit();
                return;
            }

            // pressing A makes the character jump
            if(gs.IsButtonDown(Buttons.A)) {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.jump, 0, gameTime);
            }

            if(gs.IsButtonDown(Buttons.RightTrigger)) {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.interact, 0, gameTime);
            }

            // Left thumbstick controls
            if(gs.ThumbSticks.Left.X < -0.5f)
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.walk_left, 0, gameTime);
            if(gs.ThumbSticks.Left.X > 0.5f)
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.walk_right, 0, gameTime);
        }
    }
}
