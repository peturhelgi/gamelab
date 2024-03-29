﻿using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Collision;
using Microsoft.Xna.Framework;
using TheGreatEscape.Util;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Rope : Tool
    {

        public static Texture2D ToolSprite;

        private CollisionDetector CollisionDetector = new CollisionDetector();

        public Rope()
        {
            UsesLeft = 1;
        }

        public override void Use(Miner user, GameState gamestate)
        {
            if (UsesLeft <= 0)
            {
                return;
            }
            List<GameObject> hooks = new List<GameObject>();
            foreach (GameObject q in gamestate.GetObjects(GameState.Handling.Solid))
                if(q is RockHook) hooks.Add(q);

            Vector2 offset = new Vector2(100, 0);
            AxisAllignedBoundingBox interactionBox = new AxisAllignedBoundingBox(
                new Vector2(user.Position.X, user.Position.Y - 750) - offset,
                user.Position + user.SpriteSize + offset);
                
            List<GameObject> collisions = CollisionDetector.FindCollisions(interactionBox, hooks);

            foreach (GameObject c in collisions)
            {
                bool used = (c as RockHook).HangOrTakeRope(gamestate);
                CanUseAgain = !((c as RockHook).isRope);
                if (used && !CanUseAgain)
                {
                    --UsesLeft;
                    break;
                }
            }
            CanUseAgain = UsesLeft > 0;
        }
        public override Texture2D GetTexture() 
        {
            return ToolSprite;
        }

        public override string ToString() {
            return "rope";
        }
    }
}
