﻿using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Project.GameObjects.Miner;
using Project.Util.Collision;

namespace Project.Util
{

    class GameEngine
    {
        public GameState GameState;
        public enum GameAction { walk_right, walk_left, jump, interact, collect };
        private CollisionDetector CollisionDetector;

        int[] CurrentMiner = {0,1};
        public static Vector2 GRAVITY = new Vector2(0, 1000);
        public TimeSpan gameTime;

        public GameEngine(GameState gameState) {
            GameState = gameState;
            CollisionDetector = new CollisionDetector();
        }

       


        public void HandleInput(int player, GameAction action, float value) {
            Miner miner = GameState.Actors.ElementAt(CurrentMiner[player]);

            switch (action) {
                case (GameAction.walk_right):
                    CalculateAndSetNewPosition(miner, new Vector2(5, 0));
                    break;

                case (GameAction.walk_left):
                    CalculateAndSetNewPosition(miner, new Vector2(-5, 0));
                    break;
                case (GameAction.jump):
                    TryToJump(miner, new Vector2(0,-800));
                    break;

                case (GameAction.interact):
                    TryToInteract(miner);
                    break;

                default:
                    break;
            }
        }

        public void Update() {

            // TODO make this with all miners

            Miner miner = GameState.Actors.ElementAt(CurrentMiner[0]);

            // we only need to update this, if some time has passed since the last update
            if (miner.lastUpdated != gameTime) {
                CalculateAndSetNewPosition(miner, Vector2.Zero);
            }
        }

        void TryToInteract(GameObject obj) {
            List<GameObject> collisions = CollisionDetector.FindCollisions(obj.BBox, GameState.Collectibles);
            foreach (GameObject c in collisions) {
                c.Visible = false;
                Debug.WriteLine(c.TextureString);
                Debug.WriteLine(c.Position);
            }
        }
        

        void CalculateAndSetNewPosition(Miner actor, Vector2 direction)
        {

            // 1. calulate position without any obstacles
            if (actor.Falling)
            {
                actor.Speed += GRAVITY * (float)(gameTime -actor.lastUpdated).TotalSeconds;
            }
            direction += actor.Speed * (float)(gameTime - actor.lastUpdated).TotalSeconds;


            // 2. check for collisions in the X-axis, the Y-axis (falling and jumping against something) and the intersection of the movement
            AxisAllignedBoundingBox xBox, yBox;

            if (direction.X > 0) // we are moving right
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(actor.BBox.Max.X, actor.BBox.Min.Y), 
                    new Vector2(actor.BBox.Max.X + direction.X, actor.BBox.Max.Y)
                    );
            }
            else
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(actor.BBox.Min.X + direction.X, actor.BBox.Min.Y),
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y)
                    );
            }

            
            if (direction.Y > 0) // we are moving downwards
            {
                yBox = new AxisAllignedBoundingBox(
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y),
                    new Vector2(actor.BBox.Max.X, actor.BBox.Max.Y + direction.Y)
                    );
            }
            else
            {
                yBox = new AxisAllignedBoundingBox(
                   new Vector2(actor.BBox.Min.X, actor.BBox.Min.Y + direction.Y),
                   new Vector2(actor.BBox.Max.X, actor.BBox.Min.Y)
                   );
            }

            
            // 3. check, if there are any collisions in the X-axis and correct position
            List<GameObject> collisions = CollisionDetector.FindCollisions(xBox, GameState.Solids);
            if (collisions.Count > 0) {
                Debug.WriteLine("collided with x-axis");
                direction.X = 0;
            }


            // We only need to check things in y-axis (including intersection), if we are actually moving in it
            if (actor.Falling)
            {
                collisions = CollisionDetector.FindCollisions(yBox, GameState.Solids);
                if (collisions.Count > 0)
                {
                    Debug.WriteLine("collided with y-axis");

                    float lowestPoint = float.MaxValue;
                    foreach (GameObject collision in collisions)
                    {
                        lowestPoint = Math.Min(lowestPoint, collision.BBox.Min.Y);
                    }

                    actor.Speed = Vector2.Zero;
                    if (direction.Y > 0) // hitting floor
                    {
                        direction.Y = (lowestPoint - actor.BBox.Max.Y)-0.1f;
                        actor.Falling = false;
                    }

                }
                
            }
            actor.Position += direction;


            if (!actor.Falling) {
                // Next, we need to check if we are falling, i.e. walking over an edge to store it to the character, to calculate the difference in height for the next iteration
                AxisAllignedBoundingBox tempBox = new AxisAllignedBoundingBox(
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y), 
                    new Vector2(actor.BBox.Max.X, actor.BBox.Max.Y + 0.5f)
                    );

                
                
                collisions = CollisionDetector.FindCollisions(tempBox, GameState.Solids);
                if (collisions.Count == 0)
                    actor.Falling = true;
            }
            actor.lastUpdated = gameTime;
        }

        void TryToJump(Miner miner, Vector2 speed) 
        {
            if (!miner.Falling) {
                miner.Jump(speed);
                miner.Falling = true;
            }
        }
    }
}
