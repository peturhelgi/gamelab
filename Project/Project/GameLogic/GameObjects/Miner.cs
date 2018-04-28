using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Project.Libs;
using TheGreatEscape.GameLogic.Util;

namespace Project.GameLogic.GameObjects.Miner
{

    public enum MotionType { idle, walk, run, jump };
    class Miner : GameObject
    {
        Tool tool;
        MotionType motionType;

        public Dictionary<MotionType, MotionSpriteSheet> Motion;
        public MotionSpriteSheet CurrMotion;
        public SpriteEffects orientation;

        public TimeSpan lastUpdated;
        public Miner(Vector2 position, Vector2 spriteSize, Vector2 speed, double mass, string textureString)
        {
            Position = position;
            Speed    = speed;
            Mass     = mass;
            SpriteSize = spriteSize;
            Visible  = true;
            motionType     = MotionType.walk;
            tool = new Pickaxe();
            TextureString = textureString;
            Lights = new List<Light>
            {
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Circular, this),
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Directional, this)
            };
            Seed = SingleRandom.Instance.Next();
            lastUpdated = new TimeSpan();

            orientation = SpriteEffects.FlipHorizontally;
            InstantiateMotionSheets();
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
                        mss = new MotionSpriteSheet(24, 42, MotionType.idle);
                        break;
                    case MotionType.walk:
                        mss = new MotionSpriteSheet(11, 100, MotionType.walk);
                        break;
                    case MotionType.run:
                        mss = new MotionSpriteSheet(12, 88, MotionType.run);
                        break;
                    //TODO: fix the jump sprite, has a small artefact
                    case MotionType.jump:
                        mss = new MotionSpriteSheet(12, 60, MotionType.jump);
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

        public void ChangeCurrentMotion(MotionType m)
        {

            //TODO: Improve fix for corner case when miner is walking while in air
            if (m == MotionType.walk && CurrMotion.SheetType == MotionType.jump)
            {
                return;
            }
            //TODO: add check when this TryGetValue fails
            Motion.TryGetValue(m, out CurrMotion);
            if (CurrMotion.DifferentMotionType(m))
            {
                CurrMotion.ResetCurrentFrame();
            }

        }
        /// <summary>
        /// Makes the miner jump if possible
        /// </summary>
        /// <returns>True if 1==1</returns>
        public bool Jump(Vector2 speed) {
            this.motionType = MotionType.jump;
            // TODO: add jump logic
            this.Speed = speed;

            ChangeCurrentMotion(MotionType.jump);
            return true;
        }


        /// <summary>
        /// Makes the miner crouch if possible
        /// </summary>
        /// <returns></returns>
        public bool Crouch() {
            this.motionType = MotionType.crawl;
            // TODO: add crouch logic

            return true;
        }


        public bool IsCrawling() {
            return this.motionType == MotionType.crawl;
        }
        /// <summary>
        /// Makes the miner walk if possible
        /// </summary>
        /// <returns>true iff 1==1</returns>
        public bool Walk() {
            this.motionType = MotionType.walk;
            // TODO: Add some walk logic

            return true;
        }

        public bool IsWalking() {
            return this.motionType == MotionType.walk;
        }

        public bool LieDown() {
            this.motionType = MotionType.stop;

            return true;
        }              

        public bool Run() {
            this.motionType = MotionType.run;
            // TODO: Add some running logic

            return true;
        }

        public bool IsRunning() {
            return this.motionType == MotionType.run;
        }

        public bool Halt() {
            this.Speed = new Vector2(0, 0);
            this.motionType = MotionType.stop;
            return true;
        }

        public bool IsStill() {
            return this.motionType == MotionType.stop;
        }

        /// <summary>
        /// Updates the speed if the miner if possible.
        /// </summary>
        /// <param name="direction">Direction in which to move the miner</param>
        /// <returns>True iff 1==1</returns>
        public bool Move(Vector2 dv) {
            //TODO: add move logic, the one here is just an example
            switch (this.motionType) {
                case MotionType.crawl:
                    this.Speed = dv/2; // for example, there could be some more logic here using our physics
                    break;

                case MotionType.walk:
                    this.Speed = dv;   // for example, there could be some more logic here using our physics
                    break;

                case MotionType.run:
                    this.Speed = 2*dv; // for example, there could be some more logic here using our physics
                    break;

                default:
                    // Nothing happens yet
                    this.Speed = dv;
                    break;
            }

            return true;
        }

        /// <summary>
        /// Uses the tool that the miner currently has
        /// </summary>
        /// <returns>True iff 1==1</returns>
        public bool UseTool(List<GameObject> gameObjects) {
            tool.Use(this, gameObjects);

            return true;
        }
    }
}
