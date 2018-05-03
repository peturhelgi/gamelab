using Microsoft.Xna.Framework;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Rock : GameObject
    {
       public Rock(Vector2 position, Vector2 spriteSize)
        : base(position, spriteSize)
        {
            {
                Speed = new Vector2(0);
                Mass = 10;
                Visible = true;
                Moveable = false;
            }
        }
    }
}
