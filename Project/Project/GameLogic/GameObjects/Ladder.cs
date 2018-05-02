using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic.GameObjects
{
    class Ladder : GameObject
    {

        public Ladder(Vector2 position, Vector2 spriteSize, string textureString)
        {

            TextureString = textureString;
            Position = position;
            SpriteSize = spriteSize;
            Falling = true;

            Speed = Vector2.Zero;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Moveable = false;
        }
    }
}
