﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Project.Libs;
using Project.GameLogic.Collision;


namespace Project.GameLogic.GameObjects.Miner
{

    enum Gait { stop, crawl, walk, run, jump};
    enum Stance { stand, jump, crouch, lie };
    class Miner : GameObject
    {
        Tool tool;
        Gait Gait;
        Stance Stance;
        private CollisionDetector CollisionDetector = new CollisionDetector();
        public GameObject HeldObj;
        public bool Holding;

        public Miner(Vector2 position, Vector2 spriteSize, Vector2 speed, double mass, string textureString)
        {
            Position = position;
            Speed    = speed;
            Mass     = mass;
            SpriteSize = spriteSize;
            Visible  = true;
            Gait     = Gait.walk;
            Stance   = Stance.jump;
            tool = new Pickaxe();
            TextureString = textureString;
            Lights = new List<Light>
            {
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Circular, this),
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Directional, this)
            };
            Seed = SingleRandom.Instance.Next();
            LastUpdated = new TimeSpan();
            HeldObj = null;
            Holding = false;
            Moveable = true;
        }

        /// <summary>
        /// Makes the miner jump if possible
        /// </summary>
        /// <returns>True if 1==1</returns>
        public bool Jump(Vector2 speed) {
            this.Stance = Stance.jump;
            this.Gait = Gait.jump;
            // TODO: add jump logic
            this.Speed = speed;

            return true;
        }
        public bool IsAirborne()
        {
            return this.Stance == Stance.jump;
        }


        /// <summary>
        /// Makes the miner crouch if possible
        /// </summary>
        /// <returns></returns>
        public bool Crouch() {
            this.Stance = Stance.crouch;
            this.Gait = Gait.crawl;
            // TODO: add crouch logic

            return true;
        }

        public bool IsCrouching()
        {
            return this.Stance == Stance.crouch;
        }

        public bool IsCrawling() {
            return this.Gait == Gait.crawl;
        }
        /// <summary>
        /// Makes the miner walk if possible
        /// </summary>
        /// <returns>true iff 1==1</returns>
        public bool Walk() {
            this.Stance = Stance.stand;
            this.Gait = Gait.walk;
            // TODO: Add some walk logic

            return true;
        }

        /// <summary>
        /// Makes the miner stand up if possible
        /// </summary>
        /// <returns>true iff 1==1</returns>
        public bool StandUp()
        {
            this.Stance = Stance.stand;
            return true;
        }

        public bool IsStanding()
        {
            return this.Stance == Stance.stand;
        }
        public bool IsWalking() {
            return this.Gait == Gait.walk;
        }

        public bool LieDown() {
            this.Stance = Stance.lie;
            this.Gait = Gait.stop;

            return true;
        }              

        public bool IsLying()
        {
            return this.Stance == Stance.lie;
        }

        /// <summary>
        /// Makes the miner run if possible
        /// </summary>
        /// <returns>true iff 1==1</returns>
        public bool Run() {
            this.Stance = Stance.stand;
            this.Gait = Gait.run;
            // TODO: Add some running logic

            return true;
        }

        public bool IsRunning() {
            return this.Gait == Gait.run;
        }
        public bool Halt() {
            this.Speed = new Vector2(0, 0);
            this.Gait = Gait.stop;
            this.Stance = Stance.stand;
            return true;
        }

        public bool IsStill() {
            return this.Gait == Gait.stop;
        }

        /// <summary>
        /// Updates the speed if the miner if possible.
        /// </summary>
        /// <param name="direction">Direction in which to move the miner</param>
        /// <returns>True iff 1==1</returns>
        public bool Move(Vector2 dv) {
            //TODO: add move logic, the one here is just an example
            switch (this.Gait) {
                case Gait.crawl:
                    this.Speed = dv/2; // for example, there could be some more logic here using our physics
                    break;

                case Gait.walk:
                    this.Speed = dv;   // for example, there could be some more logic here using our physics
                    break;

                case Gait.run:
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
        /// Uses the tool that the miner currenty has
        /// </summary>
        /// <returns>True iff 1==1</returns>
        public bool UseTool(GameState gs) {
            this.Stance = Stance.stand;
            tool.Use(this, gs);

            return true;
        }

        public AxisAllignedBoundingBox InteractionBox()
        {
            Vector2 offset = new Vector2(50, 50);
            return new AxisAllignedBoundingBox(Position - offset, Position + SpriteSize + offset);
        }

        public void InteractWithCrate(GameState gs)
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
                        gs.AddCollectible(c);
                        gs.RemoveSolid(c);

                        HeldObj = c;
                        HeldObj.Falling = false;
                        Holding = true;
                    }
                }
            }
            else
            {
                HeldObj.Falling = true;
                gs.AddSolid(HeldObj);
                gs.RemoveCollectible(HeldObj);

                HeldObj = null;
                Holding = false;
            }
        }

    }
}
