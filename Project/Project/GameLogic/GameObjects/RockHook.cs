using Microsoft.Xna.Framework;
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
                Moveable = false;
                TextureString = textureString;
                isRope = false;

                if (ropeLength == 0)
                    ropeLength = 200;

                _ropeLength = ropeLength;



                Rope = new HangingRope(position + new Vector2(120.0f / 282.0f * spriteSize.X, 153.0f / 168.0f * spriteSize.Y),
                    new Vector2(44, ropeLength), secondTextureString)
                { Active = true };
            }
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
        public void HangOrTakeRope(GameState gs)
        {
            if (!isRope)
            {
                gs.Add(Rope, GameState.Handling.None);
            }
            else gs.Remove(Rope, GameState.Handling.None);
            Rope.SwapTextures();
            isRope = !isRope;
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
                Speed = new Vector2(0);
                Mass = 10;
                Visible = true;
                Moveable = false;
                TextureString = textureString;
                SecondTextureString = "Sprites/Misc/Rope_transparent";
                _ropeLength = spriteSize.Y;
            }
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
