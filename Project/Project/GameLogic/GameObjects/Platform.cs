﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.Util;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Platform : GameObject
    {
        public bool Activate;
        public int ActivationId;
        public float Displacement;
        public Vector2 DisplacementStep;
        public float MaxHeight;
        public float MinHeight;
        public bool _movingInYdir;
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
            Movable = false;
            Activate = false;

            Displacement = displacement;
            ActivationId = actId;
            Speed = Vector2.Zero;

            Initialize();

            if (dir is null)
            {
                dir = "y";
                secondTextureString = "Sprites/Misc/platform_mechanismy";
                Displacement = 800;
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


        public override void Initialize()
        {
            base.Initialize();
            Falling = false;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Movable = false;
            Activate = false;
        }

        public void SwapDirections()
        {
            _movingInYdir = !_movingInYdir;
            SetPosition(Vector2.Zero);
        }
        public void SetPosition(Vector2 pos)
        {
            Position += pos;
            if (_movingInYdir)
            {
                MinHeight = Position.Y;
                MaxHeight = Position.Y - Displacement;
                DisplacementStep = new Vector2(0.0f, 5.0f);
            }
            else
            {
                MaxHeight = Position.X + Displacement;
                MinHeight = Position.X;
                DisplacementStep = new Vector2(-5.0f, 0.0f);
            }
            SetBackground();
        }

        private void SetBackground()
        {
            if (_movingInYdir)
            {
                Background.Position = new Vector2(Position.X, MaxHeight + SpriteSize.Y * 0.8f);
                Background.SpriteSize = new Vector2(SpriteSize.X, Displacement);
            }
            else
            {
                Background.Position = new Vector2(Position.X, Position.Y + SpriteSize.Y * 0.5f);
                Background.SpriteSize = new Vector2(Displacement + SpriteSize.X, SpriteSize.Y);
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
            obj.TextureString = TextureString;
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
            Movable = false;
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "secondary";
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

