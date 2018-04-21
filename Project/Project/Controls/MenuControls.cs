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

        internal override void Initialize(ref Menu menu)
        {
            _menu = menu;
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
  

            // END Handle GameAction
        }


        protected override void HandleGamePad(GamePadState gs, GameTime gameTime)
        {

            if (_menu.IsVertical())
            {
                // Go down one menu item
                if (gs.IsButtonDown((Buttons)Instructions.Down) & !PrevState.IsButtonDown((Buttons)Instructions.Down))
                {
                    _menu.NextItem();
                    Debug.WriteLine(_menu.ItemNumber.ToString() + " Controls");
                }
                if (gs.IsButtonDown((Buttons)Instructions.DownStick) & !PrevState.IsButtonDown((Buttons)Instructions.DownStick))
                {
                    _menu.NextItem();
                    Debug.WriteLine(_menu.ItemNumber.ToString() + " Controls");
                }

                // Go up one menu item
                if (gs.IsButtonDown((Buttons)Instructions.Up) & !PrevState.IsButtonDown((Buttons)Instructions.Up))
                {
                    _menu.PreviousItem();
                }
                if (gs.IsButtonDown((Buttons)Instructions.UpStick) & !PrevState.IsButtonDown((Buttons)Instructions.UpStick))
                {
                    _menu.PreviousItem();
                }
            }
            else if(_menu.IsHorizontal())
            {
                // Go right one menu item
                if (gs.IsButtonDown((Buttons)Instructions.Right) & !PrevState.IsButtonDown((Buttons)Instructions.Right))
                {
                    _menu.NextItem();
                }
                if (gs.IsButtonDown((Buttons)Instructions.RightStick) & !PrevState.IsButtonDown((Buttons)Instructions.RightStick))
                {
                    _menu.NextItem();
                }

                if (gs.IsButtonDown((Buttons)Instructions.Left) & !PrevState.IsButtonDown((Buttons)Instructions.Left))
                {
                    _menu.PreviousItem();
                }
                if (gs.IsButtonDown((Buttons)Instructions.LeftStick) & !PrevState.IsButtonDown((Buttons)Instructions.LeftStick))
                {
                    _menu.PreviousItem();
                }
            }

        }
    }
}
