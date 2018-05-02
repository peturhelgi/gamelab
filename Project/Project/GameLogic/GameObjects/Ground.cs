using Microsoft.Xna.Framework;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Ground : GameObject
    {

        public Ground(Vector2 position, Vector2 spriteSize, string textureString) :
            base(position, spriteSize)
        {
            Moveable = false;
            Visible = true;
            this.TextureString = textureString;
        }
    }
}
