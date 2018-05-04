using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Rope : Tool
    {

        public static Texture2D ToolSprite;

        private CollisionDetector CollisionDetector = new CollisionDetector();

        public override void Use(Miner user, GameState gamestate)
        {
            //throw new System.NotImplementedException();
        }
        public override Texture2D GetTexture() 
        {
            return ToolSprite;
        }
    }
}
