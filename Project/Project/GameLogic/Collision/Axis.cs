using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic.Collision
{
    public class Axis
    {
        public Vector2 direction;
        public Axis(Vector2 direction) {
            direction.Normalize();
            this.direction = direction;
        }

        public bool Equals(Axis other)
        {
            if (this.direction == other.direction)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
