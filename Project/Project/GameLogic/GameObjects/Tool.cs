using Microsoft.Xna.Framework.Graphics;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public abstract class Tool
    {
        public bool CanUseAgain = true;
        public int UsesLeft = 0;
        abstract public void Use(Miner user, GameState gamestate);
        abstract public Texture2D GetTexture();
    }
}