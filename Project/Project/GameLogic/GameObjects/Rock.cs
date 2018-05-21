using Microsoft.Xna.Framework;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Rock : GameObject
    {
        private Vector2 _spriteSize;
        public override Vector2 SpriteSize
        {
            get { return _spriteSize; }
            set
            {
                _spriteSize = value;
                Mass = _spriteSize.X * _spriteSize.Y / 750;
            }
        }
        public Rock(Vector2 position, Vector2 spriteSize)
        : base(position, spriteSize)
        {
            {
                Speed = new Vector2(0);
                Mass = 10;
                Visible = true;
                Movable = true;
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
            obj.TextureString = TextureString;
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
