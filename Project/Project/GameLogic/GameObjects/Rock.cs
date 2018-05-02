using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Rock : GameObject
    {
       public Rock(Vector2 position, Vector2 spriteSize)
        : base(position, spriteSize)
        {
            {
                Speed = new Vector2(0);
                Mass = 10;
                Visible = true;
                Moveable = false;
            }
        }
    }
}
