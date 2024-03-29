﻿using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

using TheGreatEscape.Util;
using TheGreatEscape.Menu;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Pickaxe : Tool
    {

        public static Texture2D ToolSprite;
        public int PickaxeStrength;
        public Pickaxe() {

            PickaxeStrength = 1;
            UsesLeft = 100;
        }
        private CollisionDetector CollisionDetector = new CollisionDetector();

        public override void Use(Miner user, GameState gamestate)
        {
            if(UsesLeft <= 0)
            {
                return;
            }

            List<GameObject> SolidAndInteracts = gamestate.GetObjects(GameState.Handling.Solid,
                GameState.Handling.Interact);
            List<GameObject> collisions = CollisionDetector.FindCollisions(
                user.InteractionBox(),
                SolidAndInteracts);

            foreach (GameObject c in collisions)
            {
                if (c is Rock)
                {
                    c.Mass -= PickaxeStrength;
                    --UsesLeft;
                    if (c.Mass <= 0)
                        gamestate.Remove(c);
                }
            }
            CanUseAgain = UsesLeft > 0;
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
