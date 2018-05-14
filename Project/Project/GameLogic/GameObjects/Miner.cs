using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.Libs;
using TheGreatEscape.LevelManager;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Util;
using TheGreatEscape.GameLogic.Renderer;

namespace TheGreatEscape.GameLogic.GameObjects
{

    public enum MotionType
    {
        idle,
        walk,
        run,
        jump,
        pickaxe
    };


    
    public class Miner : GameObject
    {
        public Dictionary<MotionType, MotionSpriteSheet> Motion;
        public Dictionary<int, SpriteEffects> Directions;
        public MotionSpriteSheet CurrMotion;
        public SpriteEffects Orientation;
        public float xVel;
        public float LookAt;

        ToolFactory factory = new ToolFactory();
        public Tool Tool;
        public GameObject HeldObj;
        public bool Holding;
        public bool Climbing;
        public bool Interacting;

        public readonly Vector2 InitialPosition;

        private CollisionDetector CollisionDetector = new CollisionDetector();

        public Miner(Vector2 position, Vector2 spriteSize)
            : base(position, spriteSize)
        {

            // Miner Lights
            float x = 0.5f, y = 0.15f;
            Vector2 center = new Vector2(x, y),
                size = 3.0f * new Vector2(SpriteSize.Y);
            Lights = new List<Light>
            {
                new Light(
                    (SpriteSize * center), // Offset
                    LightRenderer.Lighttype.Circular, // type
                    this, // owner
                    size, // size
                    new Vector2(0.5f)), // origin
                new Light(
                    (SpriteSize * center), // Offset 
                    LightRenderer.Lighttype.Directional, // type
                    this, // Owner
                    new Vector2(1.3f, 1.1f) * size, // size
                    new Vector2(0.1f, 0.5f)) // origin in proportion to light sprite
            };
            Seed = SingleRandom.Instance.Next();

            InitialPosition = position;
            LastUpdated = new TimeSpan();
            HeldObj = null;
            Holding = false;
            Climbing = false;
            Moveable = true;
            Interacting = false;
            LookAt = 0.0f;

            // Motion sheets
            xVel = 0;
            InstantiateMotionSheets();
            Directions = new Dictionary<int, SpriteEffects>
            {
                { -1, SpriteEffects.None },
                { 1, SpriteEffects.FlipHorizontally }
            };

            Orientation = SpriteEffects.FlipHorizontally;
            //TODO: add a case when it fails to get that type of motion
            Motion.TryGetValue(MotionType.idle, out CurrMotion);



        }

        public void SetOrientation(int value)
        {
            Directions.TryGetValue(value, out Orientation);
        }

        private void InstantiateMotionSheets()
        {
            MotionSpriteSheet mss;
            Motion = new Dictionary<MotionType, MotionSpriteSheet>();

            foreach (MotionType m in Enum.GetValues(typeof(MotionType)))
            {
                switch (m)
                {
                    case MotionType.idle:
                        mss = new MotionSpriteSheet(24, 42, MotionType.idle, new Vector2(1, 1));
                        break;
                    case MotionType.walk:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.2f, 1));
                        break;
                    case MotionType.jump:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.5f, 1.12f));
                        break;
                    case MotionType.run:
                        mss = new MotionSpriteSheet(12, 64, m, new Vector2(1.4f, 1));
                        break;
                    case MotionType.pickaxe:
                        mss = new MotionSpriteSheet(12, 64, m, new Vector2(2.3f, 1));
                        break;
                    default:
                        mss = null;
                        break;
                }

                Motion.Add(m, mss);
            }
        }

        public void SetMotionSprite(Texture2D sprite, MotionType m)
        {
            if (Motion.TryGetValue(m, out MotionSpriteSheet mss))
            {
                mss.Image = sprite;
            }
        }

        private MotionType GetCurrentState()
        {

            if (this.xVel > 0)
            {
                if (this.Speed.Y != 0f)
                    return MotionType.jump;
                else if (this.xVel >= GameEngine.RunSpeed)
                    return MotionType.run;
                else
                    return MotionType.walk;
            }

            if (this.xVel < 0)
            {
                if (this.Speed.Y != 0)
                    return MotionType.jump;
                else if (this.xVel <= -GameEngine.RunSpeed)
                    return MotionType.run;
                else
                    return MotionType.walk;
            }

            if (this.Speed.Y != 0)
            {
                return MotionType.jump;
            }

            if (this.Interacting && Tool is Pickaxe)
                return MotionType.pickaxe;


            return MotionType.idle;

        }

        public void ChangeCurrentMotion()
        {
            MotionType motion = GetCurrentState();

            //TODO: add check when this TryGetValue fails
            Motion.TryGetValue(motion, out CurrMotion);
            if (CurrMotion.DifferentMotionType(motion))
            {
                CurrMotion.ResetCurrentFrame();
            }

            switch (motion)
            {
                case MotionType.pickaxe:
                    if (CurrMotion.LoopsPlayed >= 1)
                    {
                        this.Interacting = false;
                        CurrMotion.ResetCurrentFrame();
                    }
                    break;

            }

        }

        /// <summary>
        /// Uses the tool that the miner currenty has
        /// </summary>
        /// <returns>True iff 1==1</returns>
        public bool UseTool(GameState gs)
        {
            this.Interacting = true;
            Tool.Use(this, gs);
            return true;
        }

        public void ResetPosition()
        {
            this.Position = InitialPosition;
        }

        public AxisAllignedBoundingBox InteractionBox()
        {
            Vector2 offset = new Vector2(50, 50);
            return new AxisAllignedBoundingBox(Position - offset, Position + SpriteSize + offset);
        }

        public bool InteractWithCrate(GameState gs)
        {
            // picking up crate
            if (!Holding)
            {
                List<GameObject> collisions = CollisionDetector.FindCollisions(
                    InteractionBox(), gs.GetObjects(GameState.Handling.Solid));
                foreach (GameObject c in collisions)
                {
                    if (c is Crate)
                    {
                        bool worked = false;
                        if (c.Position.X < Position.X)
                        {
                            worked = pickUpCrateLeftSide(c, gs);
                            if (!worked) worked = pickUpCrateRightSide(c, gs);
                        }
                        else
                        {
                            worked = pickUpCrateRightSide(c, gs);
                            if (!worked) worked = pickUpCrateLeftSide(c, gs);
                        }
                        if (worked) return true;
                    }
                }
                return false;
            }
            else
            {
                HeldObj.Falling = true;

                // Move HeldObj from NonSolids to Solids
                gs.Remove(HeldObj, GameState.Handling.None);
                gs.Add(HeldObj, GameState.Handling.Solid);

                HeldObj = null;
                Holding = false;
                return true;
            }
        }


        public bool pickUpCrateRightSide(GameObject c, GameState gs)
        {
            AxisAllignedBoundingBox BBox = new AxisAllignedBoundingBox(
                new Vector2(Position.X + SpriteSize.X, Position.Y), 
                new Vector2(Position.X + SpriteSize.X, Position.Y) + c.SpriteSize);
            List<GameObject> tmpSolids = gs.GetObjects(GameState.Handling.Solid);
            // tmpSolids.Remove(c);
            List<GameObject> crateCollisions = CollisionDetector.FindCollisions(BBox, tmpSolids);
            if (crateCollisions.Count == 0)
            {
                c.Position = new Vector2(Position.X + SpriteSize.X, Position.Y);
                gs.Add(c, GameState.Handling.None);
                gs.Remove(c, GameState.Handling.Solid);

                HeldObj = c;
                HeldObj.Falling = false;
                Holding = true;
                return true;
            }
            else
            {
                // gs.Add(c, GameState.Handling.Solid);
                MyDebugger.WriteLine("crate hits something as it is picked up");
                return false;
            }
        }

        public bool pickUpCrateLeftSide(GameObject c, GameState gs)
        {
            AxisAllignedBoundingBox BBox = new AxisAllignedBoundingBox(
                new Vector2(Position.X - c.SpriteSize.X, Position.Y), new Vector2(Position.X, Position.Y + c.SpriteSize.Y));
            List<GameObject> tmpSolids = gs.GetObjects(GameState.Handling.Solid);
            // tmpSolids.Remove(c);
            List<GameObject> crateCollisions = CollisionDetector.FindCollisions(BBox, tmpSolids);
            if (crateCollisions.Count == 0)
            {
                c.Position = new Vector2(Position.X - c.SpriteSize.X, Position.Y);
                gs.Add(c, GameState.Handling.None);
                gs.Remove(c, GameState.Handling.Solid);

                HeldObj = c;
                HeldObj.Falling = false;
                Holding = true;
                return true;
            }
            else
            {
                // gs.Add(c, GameState.Handling.Solid);
                MyDebugger.WriteLine("crate hits something as it is picked up");
                return false;
            }
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "miner";
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = -1;
            obj.SecondTexture = "-1";
            obj.Tool = Tool.ToString();
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = -1f;
            return obj;
        }
    }
}
