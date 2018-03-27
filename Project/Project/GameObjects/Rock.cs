using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameObjects
{
    class Rock : GameObject
    {
        public Rock(Vector2 position, float width, float height) {
            Position = position;
            Speed = new Vector2(0);
            Mass = 10;
            Box = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + width, position.Y + height, 0));
            visible = true;
        }
    }
}
