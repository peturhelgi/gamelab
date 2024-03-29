﻿using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.Collision;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class RockHook : GameObject
    {

        // public string SecondTextureString;
        public HangingRope Rope;
        public bool isRope;

        private float _ropeLength;

        public RockHook(Vector2 position, Vector2 spriteSize, string textureString, string secondTextureString, float ropeLength)
         : base(position, spriteSize)
        {
            {
                Speed = new Vector2(0);
                Mass = 10;
                Visible = true;
                Movable = false;
                TextureString = textureString;


                if (ropeLength == 0)
                    ropeLength = 200;

                _ropeLength = ropeLength;
                Vector2 scale = new Vector2(120.0f / 282.0f, 153.0f / 168.0f);

                Rope = new HangingRope(position + scale * spriteSize,
                    new Vector2(44, ropeLength), secondTextureString)
                { Active = true };
                Initialize();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            Visible = true;
            Movable = false;
            isRope = false;
            Rope.Initialize();
        }

        public void SetRope(HangingRope rope)
        {
            this.Rope = rope;
        }

        public void UpdateRope()
        {
            Rope.Position = Position + new Vector2(120.0f / 282.0f * SpriteSize.X, 153.0f / 168.0f * SpriteSize.Y);
        }

        public HangingRope GetRope()
        {
            return Rope;
        }
        public bool HangOrTakeRope(GameState gs)
        {
            if (!isRope)
            {
                gs.Add(Rope, GameState.Handling.None);
                Rope.SwapTextures();
                isRope = true;
                return true;
            }
            return false;
            //else gs.Remove(Rope, GameState.Handling.None);
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "rockandhook";
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = -1;
            obj.SecondTexture = Rope.TextureString;
            obj.Tool = "-1";
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = _ropeLength;
            return obj;
        }

    }

    class HangingRope : GameObject
    {
        private float _ropeLength;
        public string SecondTextureString;
        public Texture2D SecondTexture;

        public HangingRope(Vector2 position, Vector2 spriteSize, string textureString)
            : base(position, spriteSize)
        {
            {
                Speed = Vector2.Zero;
                Mass = 10;
                Visible = true;
                Movable = false;
                TextureString = textureString;
                SecondTextureString = "Sprites/Misc/Rope_transparent";
                Initialize();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            Visible = true;
            Movable = false;
            _ropeLength = SpriteSize.Y;
        }

        public void SwapTextures()
        {
            Texture2D temp = Texture;
            Texture = SecondTexture;
            SecondTexture = temp;
        }

        public override AxisAllignedBoundingBox BBox
        {
            get
            {
                return new AxisAllignedBoundingBox(
                    new Vector2(Position.X + SpriteSize.X * 0.25f, Position.Y + 210),
                    new Vector2(Position.X + SpriteSize.X * 0.75f, Position.Y + SpriteSize.Y));
            }
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "hangingRope";
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = -1;
            obj.SecondTexture = "-1";
            obj.Tool = "-1";
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = _ropeLength;
            return obj;
        }

    }
}
