using Microsoft.Xna.Framework.Graphics;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public abstract class Tool
    {
        abstract public void Use(Miner user, GameState gamestate);
        abstract public Texture2D GetTexture();
    }
}