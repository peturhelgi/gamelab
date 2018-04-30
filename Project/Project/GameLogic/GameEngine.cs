using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Project.GameLogic.GameObjects.Miner;
using Project.GameLogic.GameObjects;
using Project.GameLogic.Collision;

using TheGreatEscape.GameLogic.Util;

namespace Project.GameLogic
{

    class GameEngine
    {
        public GameState GameState;
        public enum GameAction { walk_right, walk_left, jump, interact, collect };
        public CollisionDetector CollisionDetector;
        List<AxisAllignedBoundingBox> _attentions;

        int[] CurrentMiner = {0,1};
        public static Vector2 GRAVITY = new Vector2(0, 2000);
        public TimeSpan gameTime;

        public GameEngine(GameState gameState) {
            GameState = gameState;
            CollisionDetector = new CollisionDetector();
            _attentions = new List<AxisAllignedBoundingBox>();

            for (var i = 0; i < GameState.Actors.Count; i++) {
                _attentions.Add(new AxisAllignedBoundingBox(Vector2.Zero, Vector2.Zero));
            }
        }

        public List<AxisAllignedBoundingBox> GetAttentions() {
            return _attentions;
        }

        public void HandleInput(int player, GameAction action, float value) {

            if(player < 0 || player > 1)
            {
                return;
            }
            Miner miner = GameState.Actors.ElementAt(CurrentMiner[player]);
            Vector2 difference = new Vector2(0.0f);

            switch (action) {
                case (GameAction.walk_right):
                    difference = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(8, 0));
                    difference -= miner.Position;
                    if (miner.isHolding())
                    {
                        miner.holdingThisObject.Position -= difference;
                    }
                    break;

                case (GameAction.walk_left):
                    difference = miner.Position;
                    CalculateAndSetNewPosition(miner, new Vector2(-8, 0));
                    difference -= miner.Position;
                    if (miner.isHolding())
                    {
                        miner.holdingThisObject.Position -= difference;
                    }
                    break;
                case (GameAction.jump):
                    difference = miner.Position;
                    TryToJump(miner, new Vector2(0, -800));
                    difference -= miner.Position;
                    if (miner.isHolding())
                    {
                        miner.holdingThisObject.Position -= difference;
                    }
                    break;

                case (GameAction.interact):
                    TryToInteract(miner);
                    break;

                default:
                    break;
            }
        }

        public void Update() {


            Miner miner0 = GameState.Actors.ElementAt(CurrentMiner[0]);
            // we only need to update this, if some time has passed since the last update
            if (miner0.lastUpdated != gameTime)
            {
                Vector2 difference = miner0.Position;
                CalculateAndSetNewPosition(miner0, Vector2.Zero);
                difference -= miner0.Position;
                // Debug.WriteLine("x: " + difference.X.ToString() + ", y: " + difference.Y.ToString());
                if (miner0.holdingThisObject != null)
                {
                    miner0.holdingThisObject.Position -= difference;
                }
            }

            if (GameState.Actors.Count > 1) {
                Miner miner1 = GameState.Actors.ElementAt(CurrentMiner[1]);
                if (miner1.lastUpdated != gameTime)
                {
                    CalculateAndSetNewPosition(miner1, Vector2.Zero);
                }
            }

            foreach (GameObject c in GameState.GetAll())
            {
                if (c is Crate)
                {
                    if (c.Falling)
                    {
                        if ((c as Crate).lastUpdated != gameTime) fallingBox((Crate)c);
                    }
                    (c as Crate).lastUpdated = gameTime;
                }
            }

            for (var i = 0; i < _attentions.Count; i++) {
                _attentions[i] = GameState.Actors.ElementAt(CurrentMiner[i]).BBox;
            }

        }

        void TryToInteract(Miner obj)
        {
            obj.UseTool(GameState);
            obj.InteractWithCrate(GameState);
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
            AxisAllignedBoundingBox xBox, yBox, xCrate, yCrate;

            if (direction.X > 0) // we are moving right
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(actor.BBox.Max.X, actor.BBox.Min.Y), 
                    new Vector2(actor.BBox.Max.X + direction.X, actor.BBox.Max.Y)
                    );
                if (actor.isHolding())
                {
                    xCrate = new AxisAllignedBoundingBox(
                         new Vector2(actor.holdingThisObject.BBox.Max.X, actor.holdingThisObject.BBox.Min.Y),
                         new Vector2(actor.holdingThisObject.BBox.Max.X + direction.X, actor.holdingThisObject.BBox.Max.Y)
                         );
                }
                else xCrate = new AxisAllignedBoundingBox(new Vector2(0), new Vector2(0));
 
            }
            else
            {
                xBox = new AxisAllignedBoundingBox(
                    new Vector2(actor.BBox.Min.X + direction.X, actor.BBox.Min.Y),
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y)
                    );
                if (actor.isHolding())
                {
                    xCrate = new AxisAllignedBoundingBox(
                        new Vector2(actor.holdingThisObject.BBox.Min.X + direction.X, actor.holdingThisObject.BBox.Min.Y),
                        new Vector2(actor.holdingThisObject.BBox.Min.X, actor.holdingThisObject.BBox.Max.Y)
                        );
                }
                else xCrate = new AxisAllignedBoundingBox(new Vector2(0), new Vector2(0));

            }

            
            if (direction.Y > 0) // we are moving downwards
            {
                yBox = new AxisAllignedBoundingBox(
                    new Vector2(actor.BBox.Min.X, actor.BBox.Max.Y),
                    new Vector2(actor.BBox.Max.X, actor.BBox.Max.Y + direction.Y)
                    );
                if (actor.isHolding())
                {
                    yCrate = new AxisAllignedBoundingBox(
                    new Vector2(actor.holdingThisObject.BBox.Min.X, actor.holdingThisObject.BBox.Max.Y),
                    new Vector2(actor.holdingThisObject.BBox.Max.X, actor.holdingThisObject.BBox.Max.Y + direction.Y)
                    );
                }
                else yCrate = new AxisAllignedBoundingBox(new Vector2(0), new Vector2(0));
            }
            else
            {
                yBox = new AxisAllignedBoundingBox(
                   new Vector2(actor.BBox.Min.X, actor.BBox.Min.Y + direction.Y),
                   new Vector2(actor.BBox.Max.X, actor.BBox.Min.Y)
                   );
                if (actor.isHolding())
                {
                    yCrate = new AxisAllignedBoundingBox(
                    new Vector2(actor.holdingThisObject.BBox.Min.X, actor.holdingThisObject.BBox.Min.Y + direction.Y),
                    new Vector2(actor.holdingThisObject.BBox.Max.X, actor.holdingThisObject.BBox.Min.Y)
                    );
                }
                else yCrate = new AxisAllignedBoundingBox(new Vector2(0), new Vector2(0));
            }

            
            // 3. check, if there are any collisions in the X-axis and correct position
            List<GameObject> collisions = CollisionDetector.FindCollisions(xBox, GameState.Solids);
            List<GameObject> boxCollisions = CollisionDetector.FindCollisions(xCrate, GameState.Solids);
            if (collisions.Count > 0 || boxCollisions.Count > 0) {
                MyDebugger.WriteLine("collided with x-axis");
                direction.X = 0;
            }


            // We only need to check things in y-axis (including intersection), if we are actually moving in it
            if (actor.Falling)
            {
                collisions = CollisionDetector.FindCollisions(yBox, GameState.Solids);
                if (collisions.Count > 0)
                {
                    MyDebugger.WriteLine("collided with y-axis");

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
            // dropping crate
            boxCollisions = CollisionDetector.FindCollisions(yCrate, GameState.Solids);
            if (boxCollisions.Count > 0)
            {
                actor.holdingThisObject.Falling = true;
                // actor.holdingThisObject.Position = new Vector2(actor.holdingThisObject.Position.X,
                   //  actor.holdingThisObject.Position.Y + actor.SpriteSize.Y - actor.holdingThisObject.SpriteSize.Y);
                GameState.AddSolid(actor.holdingThisObject);
                GameState.RemoveCollectible(actor.holdingThisObject);

                actor.holdingThisObject = null;

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

        public void fallingBox(Crate crate)
        {
            if (!crate.Falling) return;
            Vector2 direction = Vector2.Zero;
            // 1. calulate position without any obstacles
            crate.Speed += GRAVITY * (float)(gameTime - crate.lastUpdated).TotalSeconds;
            direction += crate.Speed * (float)(gameTime - crate.lastUpdated).TotalSeconds;

            // 2. check for collisions in the Y-axis (falling) and the intersection of the movement
            AxisAllignedBoundingBox yCrate;


            if (direction.Y > 0) // we are moving downwards
            {
                yCrate = new AxisAllignedBoundingBox(
                    new Vector2(crate.BBox.Min.X, crate.BBox.Max.Y),
                    new Vector2(crate.BBox.Max.X, crate.BBox.Max.Y + direction.Y));
            }
            else
            {
                yCrate = new AxisAllignedBoundingBox(
                    new Vector2(crate.BBox.Min.X, crate.BBox.Min.Y + direction.Y),
                    new Vector2(crate.BBox.Max.X, crate.BBox.Min.Y));
            }

            List<GameObject> collisions = CollisionDetector.FindCollisions(yCrate, GameState.Solids);
            if (collisions.Count > 0)
            {
                float lowestPoint = float.MaxValue;
                foreach (GameObject collision in collisions)
                {
                    lowestPoint = Math.Min(lowestPoint, collision.BBox.Min.Y);
                }

                crate.Speed = Vector2.Zero;
                if (direction.Y > 0) // hitting floor
                {
                    direction.Y = (lowestPoint - crate.BBox.Max.Y) - 0.1f;
                    crate.Falling = false;
                }
            }

            crate.Position += direction;
            crate.lastUpdated = gameTime;
        }
    }
}
