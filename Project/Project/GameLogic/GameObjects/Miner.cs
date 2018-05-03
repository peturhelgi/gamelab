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

    public enum MotionType { idle, walk_left, walk_right, run_left, run_right, jump, pickaxe };

    public class Miner : GameObject
    {
        public Dictionary<MotionType, MotionSpriteSheet> Motion;
        public MotionSpriteSheet CurrMotion;
        public SpriteEffects Orientation;
        public float xVel;

        ToolFactory factory = new ToolFactory();
        Tool Tool;
        public GameObject HeldObj;
        public bool Holding;
        public bool Climbing;
        public bool Interacting;

        private CollisionDetector CollisionDetector = new CollisionDetector();

        public Miner(Vector2 position, Vector2 spriteSize)
            :base(position, spriteSize)
        {

            Tool = factory.Create(new Obj { Type = "pickaxe" });

            // Miner Lights
            Lights = new List<Light>
            {
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Circular, this),
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Directional, this)
            };
            Seed = SingleRandom.Instance.Next();

            LastUpdated = new TimeSpan();
            HeldObj = null;
            Holding = false;
            Climbing = false;
            Moveable = true;
            Interacting = false;

            // Motion sheets
            xVel = 0;
            InstantiateMotionSheets();
            Orientation = SpriteEffects.FlipHorizontally;
            //TODO: add a case when it fails to get that type of motion
            Motion.TryGetValue(MotionType.idle, out CurrMotion);

        }

        private void InstantiateMotionSheets() {
            MotionSpriteSheet mss;
            Motion = new Dictionary<MotionType, MotionSpriteSheet>();

            foreach (MotionType m in Enum.GetValues(typeof(MotionType)))
            {
                switch (m)
                {
                    case MotionType.idle:
                        mss = new MotionSpriteSheet(24, 42, MotionType.idle, new Vector2(1, 1));
                        break;
                    case MotionType.walk_left:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.2f, 1));
                        break;
                    case MotionType.walk_right:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.2f, 1));
                        break;
                    case MotionType.jump:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.5f, 1.12f));
                        break;
                    case MotionType.run_left:
                        mss = new MotionSpriteSheet(12, 64, m, new Vector2(1.4f, 1));
                        break;
                    case MotionType.run_right:
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

        private MotionType GetCurrentState() {

            if (this.xVel > 0)
            {
                if (this.Speed.Y != 0f)
                    return MotionType.jump;
                else if (this.xVel >= GameEngine.RunSpeed)
                    return MotionType.run_right;
                else
                    return MotionType.walk_right;
            }
            if (this.xVel < 0)
            {
                if (this.Speed.Y != 0)
                    return MotionType.jump;
                else if (this.xVel <= -GameEngine.RunSpeed)
                    return MotionType.run_left;
                else
                    return MotionType.walk_left;
            }
            if (this.Speed.Y != 0)
            {
                return MotionType.jump;
            }
            if (this.Interacting)
                return MotionType.pickaxe;


            return MotionType.idle;

        }

        public void ChangeCurrentMotion()
        {
            MotionType m = GetCurrentState();

            switch (m)
            {
                case MotionType.walk_right:
                    this.Orientation = SpriteEffects.FlipHorizontally;
                    break;
                case MotionType.walk_left:
                    this.Orientation = SpriteEffects.None;
                    break;
                case MotionType.run_right:
                    this.Orientation = SpriteEffects.FlipHorizontally;
                    break;
                case MotionType.run_left:
                    this.Orientation = SpriteEffects.None;
                    break;

            }
           
            //TODO: add check when this TryGetValue fails
            Motion.TryGetValue(m, out CurrMotion);
            if (CurrMotion.DifferentMotionType(m))
            {
                CurrMotion.ResetCurrentFrame();
            }

        }

        /// <summary>
        /// Uses the tool that the miner currenty has
        /// </summary>
        /// <returns>True iff 1==1</returns>
        public bool UseTool(GameState gs) {
            this.Interacting = true;
            Tool.Use(this, gs);
            return true;
        }

        public AxisAllignedBoundingBox InteractionBox()
        {
            Vector2 offset = new Vector2(50, 50);
            return new AxisAllignedBoundingBox(Position - offset, Position + SpriteSize + offset);
        }

        public bool InteractWithCrate(GameState gs)
        {
            // picking up crate
            if(!Holding)
            {
                List<GameObject> collisions = CollisionDetector.FindCollisions(InteractionBox(), gs.GetSolids());
                foreach (GameObject c in collisions)
                {
                    if (c is Crate)
                    {
                        c.Position = new Vector2(c.Position.X, Position.Y);
                        gs.AddNonSolid(c);
                        gs.RemoveSolid(c);

                        HeldObj = c;
                        HeldObj.Falling = false;
                        Holding = true;
                        return true;
                    }
                }
                return false;
            }
            else
            {
                HeldObj.Falling = true;
                gs.AddSolid(HeldObj);
                gs.RemoveNonSolid(HeldObj);

                HeldObj = null;
                Holding = false;
                return true;
            }
        }

    }
}
