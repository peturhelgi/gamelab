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
    public class GameController {
        public Camera Camera;
        protected GamePadState CurrentState, PrevState;
        
        public GameController(Camera camera = null) => this.Camera = camera;

        internal virtual void HandleUpdate(GameTime gameTime) {
            HandleMouse(Mouse.GetState());
            PrevState = CurrentState;
            CurrentState = GamePad.GetState(0);
        }

        private void HandleMouse(MouseState ms) {
            if(ms.LeftButton == ButtonState.Pressed) {
                Debug.Write(ms.Position.X + ", ");
                Debug.WriteLine(ms.Position.Y);
            }
        }

        public virtual void Initialize(ref Object obj) { }
        protected virtual void HandleKeyboard(KeyboardState state, GameTime gameTime) { }
        protected virtual void HandleGamePad(GamePadState gs, GameTime gameTime) { }

        public bool ButtonPressed(params Buttons[] buttons)
        {
            foreach(var button in buttons)
            {
                if(CurrentState.IsButtonDown(button)
                    && PrevState.IsButtonUp(button)
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonDown(params Buttons[] buttons)
        {
            foreach(var button in buttons)
            {
                if(CurrentState.IsButtonDown(button)
                    && PrevState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonReleased(params Buttons[] buttons)
        {
            foreach(var button in buttons)
            {
                if(CurrentState.IsButtonUp(button)
                    && PrevState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

    }
}