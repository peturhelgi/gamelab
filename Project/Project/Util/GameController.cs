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
        internal void HandleInput()
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



        }

        private void HandleMouse(MouseState ms) {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine(ms.Position.X);
                Debug.WriteLine(ms.Position.Y);
            }
        }

        private void HandleKeyboard()
        {

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
