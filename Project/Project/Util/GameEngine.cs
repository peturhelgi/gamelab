using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using Project.GameObjects;
using Project.Util.Collision;

using Microsoft.Xna.Framework;

namespace Project.Util
{

    public class GameEngine
    {
        public enum Action { walk_right, walk_left, jump, interact, collect };

        public GameState GameState;
        private CollisionDetector CollisionDetector;

        int[] CurrentMiner = { 0, 1 };
        public static Vector2 GRAVITY = new Vector2(0, 1000);
        public TimeSpan gameTimeSpan;

        public GameEngine() { }

        public void Initialize(GameState gameState)
        {
            GameState = gameState;
            CollisionDetector = new CollisionDetector();
        }

        public void HandleInput(int player, Action action, float value,
                GameTime gameTime)
        {
            Miner miner = GameState.Actors.ElementAt(CurrentMiner[player]);

            switch(action)
            {
                case (Action.walk_right):
                    CalculateAndSetNewPosition(miner, new Vector2(5, 0),
                        gameTime);
                    break;

                case (Action.walk_left):
                    CalculateAndSetNewPosition(miner, new Vector2(-5, 0),
                        gameTime);
                    break;
                case (Action.jump):
                    TryToJump(miner, new Vector2(0, -800));
                    break;

                case (Action.interact):
                    TryToInteract(miner);
                    break;

                default:
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {

            // TODO make this with all miners

            Miner miner = GameState.Actors.ElementAt(CurrentMiner[0]);
            // What simon calls gameTime, I call gameTimeSpan.
            // He implemented it using the TimeSpan gameTime
            // we only need to update this, if some time has passed since the 
            // last update.
            if(miner.lastUpdated != gameTimeSpan)
            {
                CalculateAndSetNewPosition(miner, Vector2.Zero, gameTime);
            }
        }

        void TryToInteract(GameObject obj)
        {
            List<GameObject> collisions = CollisionDetector.FindCollisions(
                obj.BBox, GameState.Collectibles);
            foreach(GameObject c in collisions)
            {
                // TODO: interaction should probably be more then changing the visiblity
                c.Visible = false;
                Debug.WriteLine(c.Image.Path);
                Debug.WriteLine(c.Position);
            }
        }

        void CalculateAndSetNewPosition(Miner actor, Vector2 direction,
            GameTime gameTime)
        {

            gameTimeSpan = gameTime.TotalGameTime;

            // 1. calulate position without any obstacles
            if(actor.Falling)
            {
                actor.Velocity += GRAVITY
                    * (float)(gameTimeSpan - actor.lastUpdated).TotalSeconds;
            }
            direction += actor.Velocity
                * (float)(gameTimeSpan - actor.lastUpdated).TotalSeconds;

            // 2. check for collisions in the X-axis, the Y-axis (falling and jumping against something) and the intersection of the movement
            AxisAlignedBoundingBox xBox, yBox;


            // What if direction.X == 0?
            if(direction.X > 0) // we are moving right
            {
                xBox = new AxisAlignedBoundingBox(
                    new Vector2(actor.BBox.Max.X, actor.BBox.Min.Y),
                    new Vector2(actor.BBox.Max.X + direction.X, actor.BBox.Max.Y)
                    );
            }
            else
            {
                xBox = new AxisAlignedBoundingBox(
                    new Vector2(actor.BBox.Min.X + direction.X, actor.BBox.Min.Y),
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y)
                    );
            }


            // What if direction.Y == 0?
            if(direction.Y > 0) // we are moving downwards
            {
                yBox = new AxisAlignedBoundingBox(
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y),
                    new Vector2(actor.BBox.Max.X, actor.BBox.Max.Y + direction.Y)
                    );
            }
            else
            {
                yBox = new AxisAlignedBoundingBox(
                   new Vector2(actor.BBox.Min.X, actor.BBox.Min.Y + direction.Y),
                   new Vector2(actor.BBox.Max.X, actor.BBox.Min.Y)
                   );
            }


            // 3. check, if there are any collisions in the X-axis and correct position
            List<GameObject> collisions = CollisionDetector.FindCollisions(xBox, GameState.Solids);
            if(collisions.Count > 0)
            {
                Debug.WriteLine("collided with x-axis");
                direction.X = 0;
            }


            // We only need to check things in y-axis (including intersection), if we are actually moving in it
            if(actor.Falling)
            {
                collisions = CollisionDetector.FindCollisions(yBox, GameState.Solids);
                if(collisions.Count > 0)
                {
                    Debug.WriteLine("collided with y-axis");

                    float lowestPoint = float.MaxValue;
                    foreach(GameObject collision in collisions)
                    {
                        // Shouldn't this be the max of those values since the
                        // y-axis points downwards?
                        lowestPoint = Math.Min(lowestPoint, collision.BBox.Min.Y);
                    }

                    actor.Velocity = Vector2.Zero;
                    if(direction.Y > 0) // hitting floor
                    {
                        direction.Y = (lowestPoint - actor.BBox.Max.Y) - 0.1f;
                        actor.Falling = false;
                    }

                }

            }
            actor.Position += direction;


            if(!actor.Falling)
            {
                // Next, we need to check if we are falling, i.e. walking over an edge to store it to the character, to calculate the difference in height for the next iteration
                AxisAlignedBoundingBox tempBox = new AxisAlignedBoundingBox(
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y),
                    new Vector2(actor.BBox.Max.X, actor.BBox.Max.Y + 0.5f)
                    );

                collisions = CollisionDetector.FindCollisions(tempBox, GameState.Solids);
                if(collisions.Count == 0)
                {
                    actor.Falling = true;
                }
            }
            actor.lastUpdated = gameTimeSpan;

            actor.Update(gameTime);
        }

        void TryToJump(Miner miner, Vector2 speed)
        {
            if(!miner.Falling)
            {
                miner.Jump(speed);
                miner.Falling = true;
            }
        }
    }
}
