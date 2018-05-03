using Microsoft.Xna.Framework.Graphics;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public abstract class Tool
    {
        public Texture2D ToolSprite;
        abstract public void Use(Miner user, GameState gamestate);
    }
}