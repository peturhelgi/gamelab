using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic.GameObjects
{
    class Rock : GameObject
    {
        public Rock(Vector2 position, Vector2 spriteSize, string textureString)
        :base(position, spriteSize, textureString){

           // TextureString = textureString;
           // Position = position;
           // SpriteSize = spriteSize;

            Speed = new Vector2(0);
            Mass = 10;
            Visible = true;
        }
    }
}
