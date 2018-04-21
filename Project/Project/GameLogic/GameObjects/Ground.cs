using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic.GameObjects
{
    class Ground : GameObject
    {
        public Ground(Vector2 position, Vector2 spriteSize, string TextureString) {
            Position = position;
            Speed = new Vector2(0);
            Mass = 1000;
            SpriteSize = spriteSize;
            Visible = true;
            this.TextureString = TextureString;

        }
    }
}
