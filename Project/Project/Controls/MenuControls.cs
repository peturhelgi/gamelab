using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;
using Project.Screens;

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
            // A menu item is selected with the Enter or Space 
            if (state.IsKeyDown(Keys.Enter) || state.IsKeyDown(Keys.Space))
            {
                SelectScreen();
                return;
            }
            if (_menu.IsHorizontal())
            {
                if (InputManager.Instance.KeyPressed(Keys.Right))
                {
                    _menu.NextItem();
                }
                else if (InputManager.Instance.KeyPressed(Keys.Left))
                {
                    _menu.PreviousItem();
                }
            }
            else if (_menu.IsVertical())
            {
                if (InputManager.Instance.KeyPressed(Keys.Down))
                {
                    _menu.NextItem();
                }
                else if (InputManager.Instance.KeyPressed(Keys.Up))
                {
                    _menu.PreviousItem();
                }
            }
            // END Handle GameAction
        }


        protected override void HandleGamePad(GamePadState gs, GameTime gameTime)
        {
            // A menu item is selected with the A button
            if (gs.IsButtonDown(Buttons.A))
            {
                SelectScreen();
                return;
            }

            if (_menu.IsVertical())
            {
                // Go down one menu item
                if (gs.IsButtonDown((Buttons)Instructions.Down) && !PrevState.IsButtonDown((Buttons)Instructions.Down))
                {
                    _menu.NextItem();
                }
                if (gs.IsButtonDown((Buttons)Instructions.DownStick) && !PrevState.IsButtonDown((Buttons)Instructions.DownStick))
                {
                    _menu.NextItem();
                }

                // Go up one menu item
                if (gs.IsButtonDown((Buttons)Instructions.Up) && !PrevState.IsButtonDown((Buttons)Instructions.Up))
                {
                    _menu.PreviousItem();
                }
                if (gs.IsButtonDown((Buttons)Instructions.UpStick) && !PrevState.IsButtonDown((Buttons)Instructions.UpStick))
                {
                    _menu.PreviousItem();
                }
            }
            else if(_menu.IsHorizontal())
            {
                // Go right one menu item
                if (gs.IsButtonDown((Buttons)Instructions.Right) && !PrevState.IsButtonDown((Buttons)Instructions.Right))
                {
                    _menu.NextItem();
                }
                if (gs.IsButtonDown((Buttons)Instructions.RightStick) && !PrevState.IsButtonDown((Buttons)Instructions.RightStick))
                {
                    _menu.NextItem();
                }

                // Go left one menu item
                if (gs.IsButtonDown((Buttons)Instructions.Left) && !PrevState.IsButtonDown((Buttons)Instructions.Left))
                {
                    _menu.PreviousItem();
                }
                if (gs.IsButtonDown((Buttons)Instructions.LeftStick) && !PrevState.IsButtonDown((Buttons)Instructions.LeftStick))
                {
                    _menu.PreviousItem();
                }
            }

        }
        
        private void SelectScreen()
        {
            switch (_menu.Items[_menu.ItemNumber].LinkId.ToLower())
            {
                case "screen":
                    ScreenManager.Instance.ChangeScreen(
                        _menu.Items[_menu.ItemNumber].LinkType,
                        _menu.Items[_menu.ItemNumber].Link);
                    break;
                case "menu":
                    Debug.WriteLine("MenuControls selected a new menu. Not Implemented");
                    //InTransition = true;
                    //menu.Transition(1.0f);
                    //foreach (MenuItem item in menu.Items)
                    //{
                    //    item.Image.StoreEffects();
                    //    item.Image.ActivateEffect("FadeEffect");
                    //}
                    break;
                default:
                    break;
            }
        }
    }
}
