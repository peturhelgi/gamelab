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

        public GameController() => this.Camera = null;

        internal virtual void HandleUpdate(GameTime gameTime) {
            HandleMouse(Mouse.GetState());
        }

        private void HandleMouse(MouseState ms) {
            if(ms.LeftButton == ButtonState.Pressed) {
                Debug.Write(ms.Position.X + ", ");
                Debug.WriteLine(ms.Position.Y);
            }
        }
    }
}