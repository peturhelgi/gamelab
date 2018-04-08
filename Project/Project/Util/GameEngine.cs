﻿using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Project.GameObjects.Miner;

namespace Project.Util
{

    class GameEngine
    {
        public GameState GameState;
        public enum GameAction { walk_right, walk_left, jump, interact, collect };
        private CollisionDetector CollisionDetector;
        Vector2 gravity = new Vector2(0, -1000);

        int[] CurrentMiner = {0,1};
        

        public GameEngine(GameState gameState) {
            GameState = gameState;
            CollisionDetector = new CollisionDetector();
        }

        public void HandleInput(int player, GameAction action, float value) {
            Miner miner = GameState.Actors.ElementAt(CurrentMiner[player]);

            switch (action) {
                case (GameAction.walk_right):
                    TryToMakeMovement(miner, new Vector2(5, 0));
                    break;

                case (GameAction.walk_left):
                    TryToMakeMovement(miner, new Vector2(-5, 0));
                    break;
                case (GameAction.jump):
                    TryToJump(miner, new Vector2(0, -500));
                    break;

                case (GameAction.interact):
                    TryToInteract(miner);
                    break;

                default:
                    break;
            }
        }

        public void Update(int player, GameTime gameTime) {
            Miner miner = GameState.Actors.ElementAt(CurrentMiner[player]);
            if (!OnPlatform(miner)) {
                miner.Speed -= (float)gameTime.ElapsedGameTime.TotalSeconds * gravity;
            }
            else if (miner.IsAirborne())
                miner.Speed = miner.Speed / 2f;

            miner.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * miner.Speed;
        }

        void TryToInteract(GameObject obj) {
            List<GameObject> collisions = CollisionDetector.FindCollisions(obj.BBox, GameState.Collectibles);
            foreach (GameObject c in collisions) {
                c.Visible = false;
                Debug.WriteLine(c.TextureString);
                Debug.WriteLine(c.Position);
            }
        }

        Vector2 CalculateActualDirectionOfMovement(GameObject obj, Vector2 direction) {
            if (obj.Falling)
            {
                // TODO we need to consider the time passed, and not a fixed time
                float deltaTime = 0.1f;
                Vector2 gravity = new Vector2(0, 9.8f);

                obj.Speed += gravity * deltaTime;
                return direction + obj.Speed;
            }
            else {
                return direction;
            }
        }

        void TryToMakeMovement(GameObject obj, Vector2 direction)
        {
            Miner actor = (Miner)obj;
            actor.Position += direction;
            //BoundingBox oldBox = actor.BBox;
            //Vector2 OldPosition = actor.Position;

            //actor.Position += CalculateActualDirectionOfMovement(actor, direction);
            
            //BoundingBox tempBox = BoundingBox.CreateMerged(oldBox, actor.BBox);

            //List<GameObject> collisions = CollisionDetector.FindCollisions(tempBox, GameState.Solids);
            //if (collisions.Count > 0)
            //{
            //    //We somehow collided. Don't move!
            //    // TODO figure out, where the collision happened, if it is legit and where we will move instead
            //    actor.Position = OldPosition;
            //}

            //// Next, we need to check if we are "flying", i.e. walking over an edge to store it to the character, to calculate the difference in height for the next iteration
            //tempBox = actor.BBox;
            //tempBox.Min -= new Vector3(0.1f, 0.1f, 0);
            //tempBox.Max += new Vector3(0.1f, 0.1f, 0);


            //collisions = CollisionDetector.FindCollisions(tempBox, GameState.Solids);
            //if (collisions.Count > 0)
            //{
            //    //We somehow collided. Now we need to check, if it was with the floor > that would mean that we are pretty near to the floor, in which case we are NOT flying.
            //    // TODO implement this
            //}
            //else {
            //    actor.Falling = true;
            //    actor.Speed = Vector2.Zero;
            //}
        }

        void TryToJump(Miner miner, Vector2 direction) 
        {
            if (OnPlatform(miner)) {
                miner.Jump(direction);
            }
        }

        Boolean OnPlatform(GameObject obj) {

            List<GameObject> collisions = CollisionDetector.FindCollisions(obj.BBox, GameState.Solids);
            if (collisions.Count > 0)
                return true;
            return false;
        }

    }
}
