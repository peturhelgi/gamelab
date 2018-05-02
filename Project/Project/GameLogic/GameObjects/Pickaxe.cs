using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGreatEscape.GameLogic.Collision;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Pickaxe : Tool
    {
        private CollisionDetector CollisionDetector = new CollisionDetector();
        public override void Use(Miner user, GameState gamestate)
        {
            List<GameObject> collisions = CollisionDetector.FindCollisions(user.InteractionBox(), gamestate.GetSolids());
            foreach (GameObject c in collisions)
            {
                if (c is Rock)
                {
                    c.Visible = false;
                    gamestate.RemoveSolid(c);
                }
            }
        }
    }
}
