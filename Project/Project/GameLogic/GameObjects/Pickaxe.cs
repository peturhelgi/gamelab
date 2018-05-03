﻿using System.Collections.Generic;

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
    }
}
