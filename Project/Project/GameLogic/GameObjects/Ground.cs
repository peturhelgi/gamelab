﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Ground : GameObject
    {
        public Ground(Vector2 position, Vector2 spriteSize, string textureString):
            base(position, spriteSize){
            /*  
             *  Vector2 position, Vector2 spriteSize, string TextureString
             *  Position = position;
              */
              Visible = true;
              this.TextureString = textureString;
        }
    }
}
