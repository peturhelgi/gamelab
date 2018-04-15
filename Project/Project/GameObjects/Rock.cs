using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Project.GameObjects {
    class Rock : GameObject {
        public Rock() {
            this.Mass = 10;
            this.Visible = true;            
        }

        void Respawn() {
            // TODO: set variables to initial values

        }

        void Destroy() {
            if(!this.Destroyable) { return; }

            // TODO: add destoy logic
        }

        void Move() {
            if(!this.Movable) { return; }

            //TODO: add move logic
        }
        public override void Update(GameTime gameTime) {
            Image.Update(gameTime);
        }

    }
}
