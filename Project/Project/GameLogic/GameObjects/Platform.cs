using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Util;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Platform : GameObject
    {
        public bool Activate;
        public readonly int ActivationId;
        public float Displacement;
        public Vector2 DisplacementStep;
        public float MaxHeight;
        public float MinHeight;
        private bool _movingInYdir;
        public PlatformBackground Background;

        public Platform(Vector2 position, Vector2 spriteSize, string textureString, float displacement, string dir, int actId, string secondTextureString)
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

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "platform";
            obj.Texture = TextureString;
            obj.Displacement = Displacement;
            if (_movingInYdir) obj.Direction = "y";
            else obj.Direction = "x";
            obj.ActivationKey = ActivationId;
            obj.SecondTexture = Background.TextureString;
            obj.Tool = "-1";
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = -1f;
            return obj;
        }
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

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "secondary";
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

