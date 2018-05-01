using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameLogic.Collision;

namespace Project.GameLogic.GameObjects
{
    class Pickaxe : Tool
    {
        private CollisionDetector CollisionDetector = new CollisionDetector();
        public override void Use(Miner.Miner user, GameState gamestate)
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
