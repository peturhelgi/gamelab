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

        public RockHook(Vector2 position, Vector2 spriteSize, string textureString, string secondTextureString)
         : base(position, spriteSize)
        {
            {
                Speed = new Vector2(0);
                Mass = 10;
                Visible = true;
                Moveable = false;
                TextureString = textureString;
                isRope = false;


                Rope = new HangingRope(position + new Vector2(120.0f / 282.0f * spriteSize.X, 153.0f / 168.0f * spriteSize.Y),
                    new Vector2(44, 748), secondTextureString)
                { Active = true };
            }
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
