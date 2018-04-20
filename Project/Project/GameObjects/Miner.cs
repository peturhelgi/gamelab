using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Project.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Project.GameObjects {

    enum Gait { stop, crawl, walk, run, jump };
    enum Stance { stand, jump, crouch, lie };
    public class Miner : GameObject
    {
        public TimeSpan lastUpdated;

        Tool tool;
        Gait Gait;
        Stance Stance;

        public Miner() : base() {
        /*
            // Old variables. Make compatible with game engine
            this.SpriteSize = spriteSize;
            this.Visible  = true;
            this.TextureString = textureString;           
        */
            this.Gait     = Gait.walk;
            this.Stance   = Stance.stand;
            lastUpdated = new TimeSpan();
            this.tool = new Pickaxe();
        }

        /// <summary>
        /// Makes the miner jump if possible
        /// </summary>
        public bool Jump(Vector2 speed) {
            //TODO: make compatible with current version
            this.Stance = Stance.jump;
            this.Gait = Gait.jump;
            // TODO: add jump logic

            this.Velocity = speed;

            return true;
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
        public bool StandUp() {
            this.Stance = Stance.stand;
            return true;
        }

        public bool LieDown() {
            this.Stance = Stance.lie;
            this.Gait = Gait.stop;

            return true;
        }

        public bool Halt() {
            this.Velocity = Vector2.Zero;
            this.Gait = Gait.stop;
            this.Stance = Stance.stand;
            return true;
        }

        public bool IsAirborne() => this.Stance == Stance.jump;
        public bool IsStanding() => this.Stance == Stance.stand;
        public bool IsCrouching() => this.Stance == Stance.crouch;
        public bool IsLying() => this.Stance == Stance.lie;
        public bool IsStill() => this.Gait == Gait.stop;
        public bool IsCrawling() => this.Gait == Gait.crawl;
        public bool IsWalking() => this.Gait == Gait.walk;
        public bool IsRunning() => this.Gait == Gait.run;
        /// <summary>
        /// Uses the tool that the miner currenty has
        /// </summary>
        /// <returns>True iff 1==1</returns>
        public bool UseTool(List<GameObject> gameObjects) {
            this.Stance = Stance.stand;
            tool.Use(this, gameObjects);

            return true;
        }
        /// <summary>
        /// Updates the speed of the miner.
        /// </summary>
        /// <param name="direction">Direction in which to move the miner</param>
        /// <returns>True iff 1==1</returns>
        public bool Move(Vector2 dv) {
            //TODO: Make compatible with game engine
            switch(this.Gait) {
                case Gait.crawl:
                    break;

                case Gait.walk:
                    break;

                case Gait.run:
                    break;

                default:

                    break;
            }

            return true;
        }
    }
}