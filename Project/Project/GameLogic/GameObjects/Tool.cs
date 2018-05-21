using Microsoft.Xna.Framework.Graphics;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public abstract class Tool
    {
        public bool CanUseAgain = true;
        abstract public void Use(Miner user, GameState gamestate);
        abstract public Texture2D GetTexture();
    }
}