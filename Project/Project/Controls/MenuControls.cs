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

        }


        protected override void HandleGamePad(GamePadState gs, GameTime gameTime)
        {

        }
    }
}
