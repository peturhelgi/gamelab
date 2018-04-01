using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameObjects
{
    class Crate : GameObject
    {
        public Crate(Vector2 position, float width, float height, int mass = 10, bool destroyable = true, bool movable = false)
        {
            this.Box = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + width, position.Y + height, 0));
            this.Destroyable = destroyable;
            this.Mass = mass;
            this.Movable = movable;
            this.Position = position;
            this.Speed = new Vector2(0);
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
    }
}
