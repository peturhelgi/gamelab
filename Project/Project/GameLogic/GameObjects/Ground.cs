using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Ground : GameObject
    {

        public Ground(Vector2 position, Vector2 spriteSize) :
            base(position, spriteSize)
        {
            Movable = false;
            Visible = true;
            // this.TextureString = textureString;
        }

        public override AxisAllignedBoundingBox BBox
        {
            get
            {
                return new AxisAllignedBoundingBox(new Vector2(Position.X, Position.Y + 25), Position + SpriteSize);
            }
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "ground";
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
