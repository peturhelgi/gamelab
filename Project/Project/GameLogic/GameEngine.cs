using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic
{

    class GameEngine
    {
        public const float WalkSpeed = 5.6f;
        public const float RunSpeed = 9.8f;
        public const float JumpForce = -800;
        public GameState GameState;
        public enum GameAction
        {
            walk_right,
            walk_left,
            jump,
            interact,
            collect,
            climb_up,
            climb_down,
            run_left,
            run_right
        };
        public CollisionDetector CollisionDetector;
        List<AxisAllignedBoundingBox> _attentions;

        int[] CurrentMiner = { 0, 1 };
        public static Vector2 GRAVITY = new Vector2(0, 2000);
        public TimeSpan gameTime;

        public GameEngine(GameState gameState)
        {
            GameState = gameState;
            CollisionDetector = new CollisionDetector();
            _attentions = new List<AxisAllignedBoundingBox>();

            for(var i = 0; i < GameState.Actors.Count; i++)
            {
                _attentions.Add(new AxisAllignedBoundingBox(Vector2.Zero, Vector2.Zero));
            }
        }

        public List<AxisAllignedBoundingBox> GetAttentions()
        {
            return _attentions;
        }

        public void HandleInput(int player, GameAction action, float value)
        {

            if(player < 0 || player >= GameState.Actors.Count)
            {
                return;
            }

            Miner miner = GameState.Actors.ElementAt(CurrentMiner[player]);
            Vector2 posDiff = Vector2.Zero;

            switch(action)
            {
                case (GameAction.walk_right):
                    posDiff = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(WalkSpeed, 0));
                    posDiff -= miner.Position;
                    if(miner.Holding && (posDiff.Length() > 1e-6))
                    {
                        CalculateAndSetNewPosition(miner.HeldObj, new Vector2(WalkSpeed, 0));
                    }
                    break;

                case (GameAction.walk_left):
                    posDiff = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(-WalkSpeed, 0));
                    posDiff -= miner.Position;
                    if(miner.Holding && (posDiff.Length() > 1e-6))
                    {
                        CalculateAndSetNewPosition(miner.HeldObj, new Vector2(-WalkSpeed, 0));
                    }
                    break;
                case (GameAction.jump):
                    TryToJump(miner, new Vector2(0, JumpForce));
                    break;
                case (GameAction.run_right):
                    posDiff = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(RunSpeed, 0));
                    posDiff -= miner.Position;
                    if(miner.Holding && (posDiff.Length() > 1e-6))
                    {
                        CalculateAndSetNewPosition(miner.HeldObj, new Vector2(RunSpeed, 0));
                    }
                    break;
                case (GameAction.run_left):
                    posDiff = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(-RunSpeed, 0));
                    posDiff -= miner.Position;
                    if(miner.Holding && (posDiff.Length() > 1e-6))
                    {
                        CalculateAndSetNewPosition(miner.HeldObj, new Vector2(-RunSpeed, 0));
                    }
                    break;
                case (GameAction.interact):
                    TryToInteract(miner);
                    break;
                case (GameAction.climb_up):
                    TryToClimb(miner, new Vector2(0, -8));
                    break;
                case (GameAction.climb_down):
                    TryToClimb(miner, new Vector2(0, 8));
                    break;
                default:
                    break;
            }
        }

        public void Update()
        {
            List<GameObject> allObjects = GameState.GetAll();

            foreach(GameObject c in allObjects)
            {
                if(c.Moveable)
                {
                    if(c.LastUpdated != gameTime)
                    {
                        CalculateAndSetNewPosition(c, Vector2.Zero);
                        if(c.Position.Y > GameState.OutOfBounds)
                        {
                            GameState.SetObject(c, GameState.Action.Remove);
                        }
                    }
                }
            }

            for(var i = 0; i < _attentions.Count; i++)
            {
                _attentions[i] = GameState.Actors[CurrentMiner[i]].BBox;
            }
        }


        void TryToInteract(Miner obj)
        {
            if(obj.Holding)
            {
                obj.InteractWithCrate(GameState);
            }
            else
            {
                bool worked = obj.InteractWithCrate(GameState);
                if(!worked) obj.UseTool(GameState);
            }
        }


        void TryToJump(Miner miner, Vector2 speed)
        {
            if(!miner.Falling && !miner.Climbing)
            {
                miner.Speed = speed;
                miner.Falling = true;
                if(miner.Holding)
                {
                    miner.HeldObj.Speed = speed;
                    miner.HeldObj.Falling = true;
                }
            }
        }

        void TryToClimb(Miner miner, Vector2 direction)
        {
            List<GameObject> ladders = new List<GameObject>();
            foreach(GameObject c in GameState.NonSolids)
            {
                if(c is Ladder) ladders.Add(c);
            }
            if(ladders.Count == 0) return;

            AxisAllignedBoundingBox Box = new AxisAllignedBoundingBox(
                   new Vector2(miner.BBox.Min.X, miner.BBox.Max.Y),
                   new Vector2(miner.BBox.Max.X, miner.BBox.Max.Y + direction.Y)
                   );

            List<GameObject> onLadders = CollisionDetector.FindCollisions(Box, ladders);
            if(onLadders.Count > 0)
            {
                miner.Speed = Vector2.Zero;
                miner.Climbing = true;
                miner.Falling = false;
                CalculateAndSetNewPosition(miner, direction);
            }
            else miner.Climbing = false;
        }


        void CalculateAndSetNewPosition(GameObject obj, Vector2 direction)
        {

            // 1. calulate position without any obstacles
            if(obj.Falling)
            {
                obj.Speed += GRAVITY * (float)(gameTime - obj.LastUpdated).TotalSeconds;
            }
            direction += obj.Speed * (float)(gameTime - obj.LastUpdated).TotalSeconds;

            // 2. check for collisions in the X-axis, the Y-axis (falling and jumping against something) and the intersection of the movement
            AxisAllignedBoundingBox xBox, yBox;

            // TODO: Move into a separate function
            if(direction.X > 0) // we are moving right
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(obj.BBox.Max.X, obj.BBox.Min.Y),
                    new Vector2(obj.BBox.Max.X + direction.X, obj.BBox.Max.Y)
                    );
            }
            else
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(obj.BBox.Min.X + direction.X, obj.BBox.Min.Y),
                    new Vector2(obj.BBox.Min.X, obj.BBox.Max.Y)
                    );
            }


            if(direction.Y > 0) // we are moving downwards
            {
                yBox = new AxisAllignedBoundingBox(
                    new Vector2(obj.BBox.Min.X, obj.BBox.Max.Y),
                    new Vector2(obj.BBox.Max.X, obj.BBox.Max.Y + direction.Y)
                    );
            }
            else
            {
                yBox = new AxisAllignedBoundingBox(
                   new Vector2(obj.BBox.Min.X, obj.BBox.Min.Y + direction.Y),
                   new Vector2(obj.BBox.Max.X, obj.BBox.Min.Y)
                   );
            }

            // 3. check, if there are any collisions in the X-axis and correct position
            // TODO: Use GameState function instead
            List<GameObject> collisions = CollisionDetector.FindCollisions(
                xBox, GameState.Solids);

            // if obj is a miner holding an object, that object can also limit the miners movement
            List<GameObject> boxCollisions = new List<GameObject>();
            if(obj is Miner)
            {
                if((obj as Miner).Holding)
                {
                    AxisAllignedBoundingBox xCrate;
                    Miner actor = obj as Miner;
                    if(direction.X > 0) // we are moving right
                    {
                        xCrate = new AxisAllignedBoundingBox(
                            new Vector2(actor.HeldObj.BBox.Max.X, actor.HeldObj.BBox.Min.Y),
                            new Vector2(actor.HeldObj.BBox.Max.X + direction.X, actor.HeldObj.BBox.Max.Y)
                            );
                    }
                    else
                    {
                        xCrate = new AxisAllignedBoundingBox(
                            new Vector2(actor.HeldObj.BBox.Min.X + direction.X, actor.HeldObj.BBox.Min.Y),
                            new Vector2(actor.HeldObj.BBox.Min.X, actor.HeldObj.BBox.Max.Y)
                            );
                    }
                    boxCollisions = CollisionDetector.FindCollisions(xCrate, GameState.Solids);
                }
            }

            if(collisions.Count > 0 || boxCollisions.Count > 0)
            {
                MyDebugger.WriteLine("collided with x-axis");
                direction.X = 0;
            }

            bool climbingMiner = false;
            if(obj is Miner) climbingMiner = (obj as Miner).Climbing;

            // We only need to check things in y-axis (including intersection), if we are actually moving in it
            if(obj.Falling || climbingMiner)
            {
                // TODO: Use GameState functions instead
                collisions = CollisionDetector.FindCollisions(yBox, GameState.Solids);
                if(collisions.Count > 0)
                {
                    MyDebugger.WriteLine("collided with y-axis");

                    float lowestPoint = float.MaxValue;
                    foreach(GameObject collision in collisions)
                    {
                        lowestPoint = Math.Min(lowestPoint, collision.BBox.Min.Y);
                    }

                    obj.Speed = Vector2.Zero;

                    // TODO: Perhaps move this to the Miner class
                    if(obj is Miner && (obj as Miner).Holding) (obj as Miner).HeldObj.Speed = Vector2.Zero;
                    if(direction.Y > 0) // hitting floor
                    {
                        direction.Y = (lowestPoint - obj.BBox.Max.Y) - 0.1f;
                        obj.Falling = false;
                        if(obj is Miner && (obj as Miner).Holding) (obj as Miner).HeldObj.Falling = false;
                    }

                    // if the object is being held by a miner and the objects has a collision, that miner drops the object
                    List<GameObject> allObjects = GameState.GetAll();
                    foreach(GameObject c in allObjects)
                    {
                        if(c is Miner)
                        {
                            Miner miner = c as Miner;
                            if(miner.Holding && miner.HeldObj == obj)
                            {
                                GameState.SetObject(miner.HeldObj, GameState.Action.Remove);

                                miner.HeldObj.Handling = GameState.Handling.Solid;
                                miner.HeldObj.Falling = true;
                                GameState.SetObject(miner.HeldObj);

                                miner.HeldObj = null;
                                miner.Holding = false;
                            }
                        }
                    }
                }
            }


            if(!obj.Falling)
            {
                // Next, we need to check if we are falling, i.e. walking over an edge to store it to the character, to calculate the difference in height for the next iteration
                AxisAllignedBoundingBox tempBox = new AxisAllignedBoundingBox(
                    new Vector2(obj.BBox.Min.X, obj.BBox.Max.Y),
                    new Vector2(obj.BBox.Max.X, obj.BBox.Max.Y + 0.5f)
                    );

                collisions = CollisionDetector.FindCollisions(tempBox, GameState.Solids);
                if(collisions.Count == 0)
                {
                    obj.Falling = true;

                    // do not drop if object is being held by a miner
                    List<GameObject> allObjects = GameState.GetAll();
                    foreach(GameObject c in allObjects)
                    {
                        if(c is Miner)
                        {
                            Miner miner = c as Miner;
                            if(miner.Holding && miner.HeldObj == obj)
                            {
                                obj.Falling = false;
                                if(miner.Position.Y != obj.Position.Y) obj.Position = new Vector2(obj.Position.X, miner.Position.Y);
                            }
                        }
                    }

                    // correct if object is miner and is climbing a ladder
                    if(obj is Miner && (obj as Miner).Climbing)
                    {
                        obj.Falling = false;
                    }

                }
            }

            if(obj is Miner)
            {
                List<GameObject> ladders = new List<GameObject>();
                foreach(GameObject c in GameState.NonSolids)
                {
                    if(c is Ladder) ladders.Add(c);
                }
                AxisAllignedBoundingBox Box = new AxisAllignedBoundingBox(
                                   new Vector2(obj.BBox.Min.X, obj.BBox.Max.Y),
                                   new Vector2(obj.BBox.Max.X, obj.BBox.Max.Y + 10)
                                   );
                List<GameObject> onLadders = CollisionDetector.FindCollisions(Box, ladders);
                if(onLadders.Count == 0) (obj as Miner).Climbing = false;
                else
                {
                    (obj as Miner).Climbing = true;
                    (obj as Miner).Falling = false;
                }

                (obj as Miner).xVel = direction.X;
                (obj as Miner).ChangeCurrentMotion();
            }

            obj.Position += direction;
            obj.LastUpdated = gameTime;
        }
    }
}
