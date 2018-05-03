using Microsoft.Xna.Framework;
using System;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Platform : GameObject
    {
        public bool Activate;
        public int ActivationId;
        public float DisplacementY;
        private float _displacementStep = 5.0f;
        private float _maxHeight;
        private float _minHeight;


        public Platform(Vector2 position, Vector2 spriteSize, string textureString, float displacementY, int ActivationId)
            : base(position, spriteSize)
        {

            TextureString = textureString;
            Position = position;
            SpriteSize = spriteSize;
            Falling = false;

            Speed = Vector2.Zero;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Moveable = false;
            Activate = false;
            DisplacementY = displacementY;
            _maxHeight = Position.Y - DisplacementY;
            _minHeight = Position.Y;
        }

        public bool MoveUp()
        {
            if (!Activate) return false;
            if (Position.Y > _maxHeight)
            {
                Position = new Vector2(Position.X, Position.Y - _displacementStep);
                return true;
            }
            else return false;
        }
    }
}

