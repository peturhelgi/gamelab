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
        public Rock(Vector2 position, Vector2 dimension, string textureString) {

            TextureString = textureString;
            Position = position;
            Dimension = dimension;

            Speed = new Vector2(0);
            Mass = 10;
            Visible = true;
        }
    }
}
