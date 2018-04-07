using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Project.Util
{

    class GameEngine
    {
        public GameState GameState;
        public enum GameAction { walk_right, walk_left, jump, interact, collect };
        private CollisionDetector CollisionDetector;

        int[] CurrentMiner = {0,1};
        

        public GameEngine(GameState gameState) {
            GameState = gameState;
            CollisionDetector = new CollisionDetector();
        }

        public void HandleInput(int player, GameAction action, float value) {
            GameObject miner = GameState.Actors.ElementAt(CurrentMiner[player]);

            switch (action) {
                case (GameAction.walk_right):
                    TryToMakeMovement(miner, new Vector2(5, 0));
                    break;

                case (GameAction.walk_left):
                    TryToMakeMovement(miner, new Vector2(-5, 0));
                    break;
                case (GameAction.jump):
                    TryToJump(miner, new Vector2(0, -5));
                    break;

                case (GameAction.interact):
                    TryToInteract(miner);
                    break;

                default:
                    break;
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

        void TryToMakeMovement(GameObject obj, Vector2 direction)
        {
            obj.Position += direction;
        }

        void TryToJump(GameObject obj, Vector2 direction) 
        {
            obj.Position += direction;
        }

    }
}
