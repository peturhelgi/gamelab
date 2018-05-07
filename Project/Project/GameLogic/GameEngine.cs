using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Util;
using Microsoft.Xna.Framework.Graphics;


namespace TheGreatEscape.GameLogic
{

    public class GameEngine
    {
        public const float WalkSpeed = 5.6f;
        public const float RunSpeed = 9.8f;
        public const float JumpForce = -800;
        const float FatalSpeed = 2000.0f;
        public GameState GameState;

        public enum GameAction
        {
            walk,
            run,
            jump,
            interact,
            collect,
            climb_up,
            climb_down,
            change_tool,
            look
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

            for (var i = 0; i < GameState.Actors.Count; i++)
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

            if (player < 0 || player >= GameState.Actors.Count)
            {
                return;
            }

            Miner miner = GameState.Actors.ElementAt(CurrentMiner[player]);
            Vector2 posDiff = Vector2.Zero;

            if (!miner.Active)
            {
                return;
            }
            switch (action)
            {
                case (GameAction.change_tool):
                    ChangeTool(miner);
                    break;
                case (GameAction.walk):
                    miner.SetOrientation((int)value);
                    posDiff = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(value * WalkSpeed, 0));
                    posDiff -= miner.Position;
                    if (miner.Holding && (Math.Abs(posDiff.X) > 1e-6))
                    {
                        if (value == 1) miner.pickUpCrateRightSide(miner.HeldObj, GameState);
                        else if (value == -1) miner.pickUpCrateLeftSide(miner.HeldObj, GameState);
                        CalculateAndSetNewPosition(miner.HeldObj, new Vector2(value * WalkSpeed, 0));
                    }
                    break;
                case (GameAction.jump):
                    TryToJump(miner, new Vector2(0, JumpForce));
                    break;
                case (GameAction.run):
                    miner.SetOrientation((int)value);
                    posDiff = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(value * RunSpeed, 0));
                    posDiff -= miner.Position;
                    if (miner.Holding && (Math.Abs(posDiff.X) > 1e-6))
                    {
                        if (value == 1) miner.pickUpCrateRightSide(miner.HeldObj, GameState);
                        else if (value == -1) miner.pickUpCrateLeftSide(miner.HeldObj, GameState);
                        CalculateAndSetNewPosition(miner.HeldObj, new Vector2(value * RunSpeed, 0));
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
                case (GameAction.look):
                    // TODO: Add looking
                    float theta = value;
                    if (miner.Orientation != SpriteEffects.FlipHorizontally)
                    {
                        // Miner looks to the left,
                        // clamp theta to be in 3rd and 4th quarters
                        if (0 <= theta && theta < MathHelper.PiOver2)
                        {
                            theta = MathHelper.PiOver2;
                        }
                        else if (-MathHelper.PiOver2 < theta && theta < 0.0f)
                        {
                            theta = -MathHelper.PiOver2;
                        }
                        theta = MathHelper.Pi - theta;
                    }
                    else
                    {
                        if (theta < -MathHelper.PiOver2)
                        {
                            theta = -MathHelper.PiOver2;
                        }
                        else if (MathHelper.PiOver2 < theta)
                        {
                            theta = MathHelper.PiOver2;
                        }
                    }


                    miner.LookAt = theta;
                    break;
                default:
                    break;
            }
        }

        private void ChangeTool(Miner miner)
        {
            GameState.CanChangeTool(miner, false);
        }

        public bool IsGameOver()
        {
            bool gameRunning = false;

            foreach (var miner in GameState.GetActors())
            {
                gameRunning |= miner.Active;
            }

            return !gameRunning;
        }

        public void CollectItems(Miner miner)
        {
            List<GameObject> collectedItems = CollisionDetector
                .FindCollisions(miner.InteractionBox(),
                GameState.GetObjects(GameState.Handling.Collect));
            var interactables = GameState.GetObjects(
                GameState.Handling.Interact);
            foreach (var item in collectedItems)
            {
                if (item is Key)
                {
                    (item as Key).Collect(interactables);
                }
                GameState.Remove(item);
            }
        }

        public void Update()
        {
            List<GameObject> actorsFirst = new List<GameObject>();
            List<Miner> actors = GameState.GetActors();
            List<GameObject> allObjects = GameState.GetAll();
            List<Platform> platforms = new List<Platform>();

            foreach (Miner m in GameState.GetActors()) actorsFirst.Add(m as GameObject);
            foreach (GameObject g in allObjects)
            {
                if (!(g is Miner)) actorsFirst.Add(g);
                if (g is Platform)
                {
                    platforms.Add(g as Platform);
                }
            }

            foreach (var miner in actors)
            {
                CollectItems(miner);
            }

            foreach (Platform p in platforms) MovePlatform(p);

            foreach (GameObject c in actorsFirst)
            {
                if (c.Active && c.Moveable)
                {
                    if (c.Position.Y > GameState.OutOfBoundsBottom)
                    {
                        GameState.Remove(c);
                    }
                    if (c.LastUpdated != gameTime)
                    {
                        CalculateAndSetNewPosition(c, Vector2.Zero);
                    }
                }

                if (c is Button)
                {
                    List<GameObject> possibleObjs = new List<GameObject>();
                    foreach (GameObject q in actorsFirst)
                        if (!(q is Ground)) possibleObjs.Add(q);

                    List<GameObject> touchingButtons = CollisionDetector.FindCollisions(c.BBox, possibleObjs);
                    if (touchingButtons.Count > 0)
                    {
                        if (!(c as Button).ON) (c as Button).Interact(platforms);
                    }
                    else
                    {
                        if ((c as Button).ON) (c as Button).Interact(platforms);
                    }
                }

            }


            if (IsGameOver())
            {
                GameState.Mode = GameState.State.GameOver;
            }

            for (var i = _attentions.Count - 1; i >= 0; --i)
            {
                if (!GameState.Actors[CurrentMiner[i]].Active)
                {
                    _attentions[i] = null;
                }
                else
                {
                    _attentions[i] = GameState.Actors[CurrentMiner[i]].BBox;
                }
            }
        }


        void TryToInteract(Miner obj)
        {
            if (obj.Holding)
            {
                if (SwitchLever(obj)) return;
                obj.InteractWithCrate(GameState);
            }
            else
            {
                if (obj.InteractWithCrate(GameState)) return;
                if (SwitchLever(obj)) return;
                var interactingItems =
                    CollisionDetector.FindCollisions(
                        obj.InteractionBox(),
                        GameState.GetObjects(GameState.Handling.Interact));
                foreach(var item in interactingItems)
                {
                    item.Interact(GameState);
                }
                obj.UseTool(GameState);
            }

        }


        void TryToJump(Miner miner, Vector2 speed)
        {
            if (!miner.Falling && !miner.Climbing)
            {
                miner.Speed = speed;
                miner.Falling = true;
                if (miner.Holding)
                {
                    miner.HeldObj.Speed = speed;
                    miner.HeldObj.Falling = true;
                }
            }
        }

        void TryToClimb(Miner miner, Vector2 direction)
        {
            List<GameObject> ladders = new List<GameObject>();
            foreach (GameObject c in GameState.NonSolids)
            {
                if (c is Ladder) ladders.Add(c);
                else if (c is HangingRope) ladders.Add(c);
            }
            if (ladders.Count == 0) return;

            AxisAllignedBoundingBox Box = new AxisAllignedBoundingBox(
                   new Vector2(miner.BBox.Min.X, miner.BBox.Max.Y),
                   new Vector2(miner.BBox.Max.X, miner.BBox.Max.Y + direction.Y)
                   );

            List<GameObject> onLadders = CollisionDetector.FindCollisions(Box, ladders);
            if (onLadders.Count > 0)
            {
                miner.Speed = Vector2.Zero;
                miner.Climbing = true;
                miner.Falling = false;
                CalculateAndSetNewPosition(miner, direction);
            }
            else miner.Climbing = false;
        }

        private bool SwitchLever(Miner miner)
        {
            List<Platform> platforms = new List<Platform>();
            List<GameObject> levers = new List<GameObject>();

            foreach (GameObject g in GameState.GetObjects(GameState.Handling.Solid))
                if (g is Platform) platforms.Add((g as Platform));
            foreach (GameObject g in GameState.GetObjects(GameState.Handling.None))
                if (g is Lever) levers.Add(g);

            List<GameObject> collisions = CollisionDetector.FindCollisions(miner.InteractionBox(), levers);
            foreach (GameObject c in collisions)
            {
                (c as Lever).Interact(platforms);
            }
            return (collisions.Count != 0);
        }

        public void MovePlatform(Platform platform)
        {
            float offset = 0.4f;
            AxisAllignedBoundingBox interactionBBox = new AxisAllignedBoundingBox(
                new Vector2(platform.BBox.Min.X, platform.BBox.Min.Y - offset),
                new Vector2(platform.BBox.Max.X, platform.BBox.Min.Y));

            List<GameObject> actorsAndSolids = new List<GameObject>();
            foreach (GameObject g in GameState.GetObjects(GameState.Handling.Solid)) actorsAndSolids.Add(g);
            foreach (Miner g in GameState.GetActors()) actorsAndSolids.Add(g as GameObject);

            List<GameObject> collisions = CollisionDetector.FindCollisions(interactionBBox, actorsAndSolids);

            if (!platform.Activate)
            {
                // moving down or left
                if ((platform.IsMovingInY() && platform.Position.Y < platform.MinHeight)
                    || (!platform.IsMovingInY() && platform.Position.X > platform.MinHeight))
                {
                    platform.Position = platform.Position + platform.DisplacementStep;
                    foreach (GameObject c in collisions)
                    {
                        if (c is Platform) continue;
                        if (platform.IsMovingInY() && (c.Position.Y + c.SpriteSize.Y > platform.Position.Y + platform.SpriteSize.Y)) continue;
                        if (!platform.IsMovingInY() && (c.Position.X > platform.Position.X + platform.SpriteSize.X)) continue;

                        CalculateAndSetNewPosition(c, platform.DisplacementStep);
                        c.Falling = false;
                        c.Speed = Vector2.Zero;
                        if ((c is Miner) && (c as Miner).Holding)
                        {
                            CalculateAndSetNewPosition((c as Miner).HeldObj, platform.DisplacementStep);
                        }
                    }
                }
            }
            else
            {
                // moving up or right
                if ((platform.IsMovingInY() && platform.Position.Y > platform.MaxHeight)
                    || (!platform.IsMovingInY() && platform.Position.X < platform.MaxHeight))
                {
                    platform.Position = platform.Position - platform.DisplacementStep;

                    foreach (GameObject c in collisions)
                    {
                        if (c is Platform) continue;
                        if (platform.IsMovingInY() && (c.Position.Y + c.SpriteSize.Y > platform.Position.Y + platform.SpriteSize.Y)) continue;
                        if (!platform.IsMovingInY() && (c.Position.X + c.SpriteSize.X < platform.Position.X)) continue;

                        CalculateAndSetNewPosition(c, -platform.DisplacementStep);
                        c.Falling = false;
                        c.Speed = Vector2.Zero;
                        if ((c is Miner) && (c as Miner).Holding)
                        {
                            CalculateAndSetNewPosition((c as Miner).HeldObj, -platform.DisplacementStep);
                        }
                    }
                }
            }
        }

        private float DistBetweenMiners()
        {
            List<Miner> actors = GameState.GetActors();
            if(actors.Count == 2)
            {
                if (!actors[0].Active || !actors[1].Active) return 0.0f;
                return Math.Abs(actors[0].Position.X - actors[1].Position.X);
            }
            else return 0.0f;
        }

        void CalculateAndSetNewPosition(GameObject obj, Vector2 direction)
        {
            int leftrightminer = 0; // rightminer then 1, leftminer then -1, not a miner(or they are close) then 0
            if (obj is Miner && (DistBetweenMiners() > 2500))
            {
                foreach (Miner m in GameState.GetActors())
                {
                    if(obj != m)
                    {
                        if (obj.Position.X < m.Position.X) leftrightminer = -1;
                        else leftrightminer = 1;
                    }
                }
            }

            // 1. calulate position without any obstacles
            if (obj.Falling)
            {
                obj.Speed += GRAVITY * (float)(gameTime - obj.LastUpdated).TotalSeconds;
            }
            direction += obj.Speed * (float)(gameTime - obj.LastUpdated).TotalSeconds;

            // 2. check for collisions in the X-axis, the Y-axis (falling and jumping against something) and the intersection of the movement
            AxisAllignedBoundingBox xBox, yBox;

            // TODO: Move into a separate function
            if (direction.X > 0) // we are moving right
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(obj.BBox.Max.X, obj.BBox.Min.Y),
                    new Vector2(obj.BBox.Max.X + direction.X, obj.BBox.Max.Y)
                    );
                if (leftrightminer == 1) direction.X = 0;
            }
            else
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(obj.BBox.Min.X + direction.X, obj.BBox.Min.Y),
                    new Vector2(obj.BBox.Min.X, obj.BBox.Max.Y)
                    );
                if (leftrightminer == -1) direction.X = 0;
            }


            if (direction.Y > 0) // we are moving downwards
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
            if (obj is Miner)
            {
                if ((obj as Miner).Holding)
                {
                    AxisAllignedBoundingBox xCrate;
                    Miner actor = obj as Miner;
                    if (direction.X > 0) // we are moving right
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

            if (collisions.Count > 0 || boxCollisions.Count > 0)
            {
                MyDebugger.WriteLine("collided with x-axis");
                direction.X = 0;
            }

            bool climbingMiner = false;
            if (obj is Miner) climbingMiner = (obj as Miner).Climbing;

            bool heldObjFalling = false;
            foreach (Miner m in GameState.GetActors())
            {
                if (m.Holding && m.HeldObj == obj)
                {
                    heldObjFalling = true;
                    break;
                }
            }

            // We only need to check things in y-axis (including intersection), if we are actually moving in it
            if (obj.Falling || climbingMiner || heldObjFalling)
            {
                // TODO: Use GameState functions instead
                collisions = CollisionDetector.FindCollisions(yBox, GameState.Solids);
                if (collisions.Count > 0)
                {
                    MyDebugger.WriteLine("collided with y-axis");

                    float lowestPoint = float.MaxValue;
                    foreach (GameObject collision in collisions)
                    {
                        lowestPoint = Math.Min(lowestPoint, collision.BBox.Min.Y);
                    }

                    if (obj is Miner && obj.Speed.Y > FatalSpeed)
                    {
                        GameState.Remove(obj);
                    }

                    obj.Speed = Vector2.Zero;
                    // TODO: Perhaps move this to the Miner class
                    if (obj is Miner && (obj as Miner).Holding) (obj as Miner).HeldObj.Speed = Vector2.Zero;

                    if (direction.Y > 0) // hitting floor
                    {
                        direction.Y = (lowestPoint - obj.BBox.Max.Y) - 0.1f;
                        obj.Falling = false;
                        if (obj is Miner && (obj as Miner).Holding) (obj as Miner).HeldObj.Falling = false;
                    }

                    // if the object is being held by a miner and the objects has a collision, that miner drops the object
                    foreach (Miner m in GameState.GetActors())
                    {
                        if (m.Holding && m.HeldObj == obj)
                        {
                            MyDebugger.WriteLine("held object hits y axis");
                            GameState.Remove(obj, GameState.Handling.None);

                            obj.Falling = true;
                            GameState.Add(obj, GameState.Handling.Solid);

                            m.HeldObj = null;
                            m.Holding = false;
                        }
                    }
                }
            }


            if (!obj.Falling)
            {
                // Next, we need to check if we are falling, i.e. walking over an edge to store it to the character, to calculate the difference in height for the next iteration
                AxisAllignedBoundingBox tempBox = new AxisAllignedBoundingBox(
                    new Vector2(obj.BBox.Min.X, obj.BBox.Max.Y),
                    new Vector2(obj.BBox.Max.X, obj.BBox.Max.Y + 0.5f)
                    );

                collisions = CollisionDetector.FindCollisions(tempBox, GameState.Solids);
                if (collisions.Count == 0)
                {
                    obj.Falling = true;

                    // do not drop if object is being held by a miner
                    List<GameObject> allObjects = GameState.GetAll();
                    foreach (Miner c in GameState.GetActors())
                    {
                        if (c.Holding && c.HeldObj == obj)
                        {
                            obj.Falling = false;
                            if (c.Position.Y != obj.Position.Y) obj.Position = new Vector2(obj.Position.X, c.Position.Y);
                        }
                    }

                    // correct if object is miner and is climbing a ladder
                    if (obj is Miner && (obj as Miner).Climbing)
                    {
                        obj.Falling = false;
                    }

                }
            }

            if (obj is Miner)
            {
                List<GameObject> ladders = new List<GameObject>();
                foreach (GameObject c in GameState.NonSolids)
                {
                    if (c is Ladder) ladders.Add(c);
                    else if (c is HangingRope) ladders.Add(c);
                }
                AxisAllignedBoundingBox Box = new AxisAllignedBoundingBox(
                                   new Vector2(obj.BBox.Min.X, obj.BBox.Max.Y),
                                   new Vector2(obj.BBox.Max.X, obj.BBox.Max.Y + 10)
                                   );
                List<GameObject> onLadders = CollisionDetector.FindCollisions(Box, ladders);
                if (onLadders.Count == 0) (obj as Miner).Climbing = false;
                else
                {
                    (obj as Miner).Climbing = true;
                    (obj as Miner).Falling = false;
                    obj.Speed = Vector2.Zero;
                }

                (obj as Miner).xVel = direction.X;
                (obj as Miner).ChangeCurrentMotion();
            }

            obj.Position += direction;
            obj.LastUpdated = gameTime;
        }
    }
}
