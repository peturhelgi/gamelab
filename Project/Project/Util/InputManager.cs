using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.Screens;

using Microsoft.Xna.Framework.Input;
namespace Project.Util {
    public class InputManager {
        KeyboardState currenKeyState, prevKeyState;
        GamePadState currentPadState, prevPadState;

        private static InputManager instance;
        public static InputManager Instance {
            get {
                if(instance == null) {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        public void Update() {
            prevKeyState = currenKeyState;
            if(!ScreenManager.Instance.InTranstition) {
                currenKeyState = Keyboard.GetState();
                currentPadState = GamePad.GetState(0);
            }
        }

       /*public bool KeyPressed(Buttons buttons) {
            foreach(ButtonState button in buttons) {
                if(button == ButtonState.Pressed
                    && prevPadState.IsButtonUp(button)) {
                    return true;
                }
            }
            return false;
        }*/

        public bool KeyPressed(params Keys[] keys) {
            foreach(Keys key in keys) {
                if(currenKeyState.IsKeyDown(key)
                    && prevKeyState.IsKeyUp(key))
                {
                    prevKeyState = currenKeyState;
                    return true;
                }
            }
            return false;
        }

        public bool KeyDown(params Keys[] keys) {
            foreach(Keys key in keys) {
                if(currenKeyState.IsKeyDown(key)
                    && prevKeyState.IsKeyDown(key)) {
                    return true;
                }
            }
            return false;
        }

        public bool KeyReleased(params Keys[] keys) {
            foreach(Keys key in keys) {
                if(currenKeyState.IsKeyUp(key)
                    && prevKeyState.IsKeyDown(key)) {
                    return true;
                }
            }
            return false;
        }
    }
}
