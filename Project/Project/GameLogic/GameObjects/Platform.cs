using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Platform : GameObject
    {
        public bool Activate;
        public int ActivationId;
        public float Displacement;
        public Vector2 DisplacementStep;
        public float MaxHeight;
        public float MinHeight;
        private bool _movingInYdir;
        public PlatformBackground Background;

        public Platform(Vector2 position, Vector2 spriteSize, string textureString, 
            float displacement, string dir, int actId, string secondTextureString)
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
            Displacement = displacement;
            ActivationId = actId;

            if (dir is null)
            {
                dir = "y";
                secondTextureString = "Sprites/Misc/platform_mechanismy";
                Displacement = 200;
            }

            if (dir == "y")
            {
                _movingInYdir = true;
                DisplacementStep = new Vector2(0.0f, 5.0f);
                MaxHeight = Position.Y - Displacement;
                MinHeight = Position.Y;
                Background = new PlatformBackground(new Vector2(Position.X, MaxHeight + SpriteSize.Y * 0.8f),
                    new Vector2(SpriteSize.X, Displacement),
                    secondTextureString)
                { Active = true };
            }
            else if (dir == "x")
            {
                _movingInYdir = false;
                DisplacementStep = new Vector2(-5.0f, 0.0f);
                MaxHeight = Position.X + Displacement;
                MinHeight = Position.X;
                Background = new PlatformBackground(new Vector2(Position.X, Position.Y + SpriteSize.Y * 0.5f),
                    new Vector2(Displacement + SpriteSize.X, SpriteSize.Y),
                    secondTextureString)
                { Active = true };
            }


        }

        public override AxisAllignedBoundingBox BBox
        {
            get
            {
                return new AxisAllignedBoundingBox(
                    new Vector2(Position.X, Position.Y + SpriteSize.Y * 0.9f), Position + SpriteSize);
            }
        }

        public bool IsMovingInY() { return _movingInYdir; }
    }

    public class PlatformBackground : GameObject
    {
        public PlatformBackground(Vector2 position, Vector2 spriteSize, string textureString)
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
        }
    }
}

