using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace Project.GameObjects
{
    class Crate : GameObject
    {
        public Crate()
        {
            // Default values for crates
            this.Movable = true;
            this.Visible = true;
        }

        void Respawn()
        {
            // TODO: set variables to initial values

        }

        void Destroy()
        {
            if (!this.Destroyable) return;

            // TODO: add destoy logic
        }

        void Move()
        {
            if (!this.Movable) return;

            //TODO: add move logic
        }

        public override void LoadContent()
        {
            //TODO: Implement
        }
        public override void UnloadContent()
        {
            //TODO: Implement
        }
        public virtual void Update(GameTime gameTime)
        {
            //TODO: Implement
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //TODO: Implement
        }
    }
}
