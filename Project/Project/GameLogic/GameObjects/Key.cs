using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TheGreatEscape.LevelManager;

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

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "key";
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = -1;
            obj.SecondTexture = "-1";
            obj.Tool = "-1";
            obj.Id = Id;
            obj.Requirement = false;
            obj.RopeLength = -1f;
            return obj;
        }
    }

}
