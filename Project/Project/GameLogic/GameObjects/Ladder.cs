using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGreatEscape.GameLogic.Collision;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Ladder : GameObject
    {

        public Ladder(Vector2 position, Vector2 spriteSize, string textureString): base(position, spriteSize)
        {

            TextureString = textureString;
            Position = position;
            SpriteSize = spriteSize;
            Falling = true;

            Speed = Vector2.Zero;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Moveable = false;
        }

        public override AxisAllignedBoundingBox BBox
        {
            get
            {
                return new AxisAllignedBoundingBox(
                    new Vector2(Position.X + SpriteSize.X*0.1f, Position.Y),
                    new Vector2(Position.X + SpriteSize.X * 0.75f, Position.Y + SpriteSize.Y));
            }
        }
    }
}
