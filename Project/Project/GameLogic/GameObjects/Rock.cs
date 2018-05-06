using Microsoft.Xna.Framework;
using TheGreatEscape.LevelManager;

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

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "rock";
            obj.Texture = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = -1;
            obj.SecondTexture = "-1";
            obj.Tool = "-1";
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = -1f;
            return obj;
        }
    }
}
