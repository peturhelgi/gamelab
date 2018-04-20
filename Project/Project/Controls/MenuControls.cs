using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;

namespace Project.Controls
{
    public class MenuControls : GameController
    {
        Menu _menu;
        public int PlayerIndex;
        public enum Instructions
        {
            Select = Buttons.A,
            Back = Buttons.B,
            Up = Buttons.DPadUp,
            UpStick = Buttons.LeftThumbstickUp,
            Down = Buttons.DPadDown,
            DownStick = Buttons.LeftThumbstickDown,
            RightStick = Buttons.LeftThumbstickRight,
            Right = Buttons.DPadRight,
            Left = Buttons.DPadLeft,
            LeftStick = Buttons.LeftThumbstickLeft
        };
        public MenuControls()
        {
            PlayerIndex = 0;
        }

        public override void Initialize(ref Object obj)
        {
            _menu = (Menu)obj;
        }
        internal override void HandleUpdate(GameTime gameTime)
        {
            base.HandleUpdate(gameTime);

            if(CurrentState.IsConnected)
            {
                HandleGamePad(CurrentState, gameTime);
            }
            HandleKeyboard(Keyboard.GetState(), gameTime);
        }

        protected override void HandleKeyboard(KeyboardState state, GameTime gameTime)
        {
            // START Handle GameAction
            if(state.IsKeyDown(Keys.Right))
            {
                GameEngine.Instance.HandleInput(
                    0, GameEngine.GameAction.walk_right, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.Left))
            {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.walk_left, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.I))
            {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.interact, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.Space))
            {
                GameEngine.Instance.HandleInput(0, GameEngine.GameAction.jump, 0, gameTime);
            }

            // END Handle GameAction
        }


        protected override void HandleGamePad(GamePadState gs, GameTime gameTime)
        {

            if(CurrentState.IsButtonDown((Buttons)Instructions.Select))
            {
                // TODO: Add a changed GameState, to escape the game
                //Exit();
                return;
            }
        }
    }
}
