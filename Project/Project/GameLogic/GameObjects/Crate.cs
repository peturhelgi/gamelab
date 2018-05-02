using Microsoft.Xna.Framework;
using System;

namespace TheGreatEscape.GameLogic.GameObjects {
    class Crate : GameObject
    {

        public Crate(Vector2 position, Vector2 spriteSize, string textureString)
            :base(position, spriteSize)
        {

            TextureString = textureString;
            Position = position;
            SpriteSize = spriteSize;
            Falling = true;

            Speed = Vector2.Zero;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Moveable = true;
        }
    }
}
