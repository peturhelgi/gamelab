using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class RockHook : GameObject
    {

        // public string SecondTextureString;
        public HangingRope Rope;
        public bool isRope;
        //public override Vector2 Position
        //{
        //    set
        //    {
        //        //if (Rope != null)
        //        //    Rope.Position = value + new Vector2(120.0f / 282.0f * SpriteSize.X, 153.0f / 168.0f * SpriteSize.Y);
        //        Position = value;
        //    }
        //    get
        //    {
        //        return Position;
        //    }
        //}
        //public override Vector2 SpriteSize
        //{
        //    set
        //    {
        //        if (Rope != null)
        //            Rope.Position = Position + new Vector2(120.0f / 282.0f * value.X, 153.0f / 168.0f * value.Y);
        //        SpriteSize = value;
        //    }
        //    get
        //    {
        //        return SpriteSize;
        //    }
        //}

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

        public void HangOrTakeRope(GameState gs)
        {
            if (!isRope) gs.Add(Rope, GameState.Handling.None);
            else gs.Remove(Rope, GameState.Handling.None);
            isRope = !isRope;
        }
    }

    class HangingRope : GameObject
    {
        public HangingRope(Vector2 position, Vector2 spriteSize, string textureString)
            : base(position, spriteSize)
        {
            {
                Speed = new Vector2(0);
                Mass = 10;
                Visible = true;
                Moveable = false;
                TextureString = textureString;
            }
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
    }
}
