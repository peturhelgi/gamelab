using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.Collision;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Ground : GameObject
    {

        public Ground(Vector2 position, Vector2 spriteSize) :
            base(position, spriteSize)
        {
            Moveable = false;
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
    }
}
