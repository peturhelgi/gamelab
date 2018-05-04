using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Pickaxe : Tool
    {

        public static Texture2D ToolSprite;
        public int PickaxeStrength;
        public Pickaxe() {

            PickaxeStrength = 5;
        }
        private CollisionDetector CollisionDetector = new CollisionDetector();

        public override void Use(Miner user, GameState gamestate)
        {
            List<GameObject> collisions = CollisionDetector.FindCollisions(user.InteractionBox(), gamestate.GetSolids());
            foreach (GameObject c in collisions)
            {
                if (c is Rock)
                {
                    c.Mass -= PickaxeStrength;
                    if (c.Mass <= 0)
                        gamestate.RemoveSolid(c);
                }
                if(c is Door)
                {
                    if((c as Door).Open())
                    {
                        if((c as Door).IsExit)
                        {
                            gamestate.Completed = true;
                        }
                        //TODO: load new scene if not exit
                    }
                    continue;
                }
            }
        }

        public override Texture2D GetTexture() 
        {
            return ToolSprite;
        }

        public override string ToString() {
            return "pickaxe";
        }
    }
}
