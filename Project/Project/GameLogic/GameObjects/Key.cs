using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Key : GameObject
    {

        public int Id;
        public Key(Vector2 position, Vector2 spriteSize)
            : base(position, spriteSize)
        {
            Movable = false;
            Visible = true;
        }

        public void Collect(List<GameObject> interactables)
        {
            foreach(var it in interactables)
            {
                if(it is Door && (it as Door).KeyId == Id)
                {
                    (it as Door).Unlock();
                }
            }
        }

        public override string ToString()
        {
            return "key";
        }
    }

}
