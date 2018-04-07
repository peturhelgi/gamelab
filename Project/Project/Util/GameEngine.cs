using Project.GameObjects;
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

        void TryToMakeMovement(Miner miner, Vector2 direction)
        {
            //if (!HasCollided(obj, direction)) {
            //}
            miner.Position += direction;
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
