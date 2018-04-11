using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Project.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Project.GameObjects
{

    enum Gait { stop, crawl, walk, run, jump};
    enum Stance { stand, jump, crouch, lie };
    public class Miner : GameObject
    {
        public TimeSpan lastUpdated;
        public Image Image;
        public float MoveSpeed;

        Tool tool;
        Gait Gait;
        Stance Stance;
        /*
            // Old variables. Make compatible with game engine
            this.SpriteSize = spriteSize;
            this.Visible  = true;
            this.Gait     = Gait.walk;
            this.Stance   = Stance.stand;
            this.TextureString = textureString;           
        */
        
        public Miner()
        {
            lastUpdated = new TimeSpan();
            this.tool = new Pickaxe();
        }
        public Miner(Vector2 position, Vector2 spriteSize, Vector2 speed, double mass, string textureString)
        {       }

        public override void LoadContent()
        {
            if(Image != null)
                Image.LoadContent();
        }

        public override void UnloadContent()
        {
            if (Image != null)
                Image.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (Image != null)
            {
                Image.IsActive = true;
                if (Velocity.X == 0 && Velocity.Y == 0)
                {
                    Image.IsActive = false;
                }

                Image.Update(gameTime);
                Image.Position += Velocity;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Image != null)
                Image.Draw(spriteBatch);
        }


        /// <summary>
        /// Makes the miner jump if possible
        /// </summary>
        public void Jump(Vector2 speed)
        {
            //TODO: make compatible with current version
            this.Stance = Stance.jump;
            this.Gait = Gait.jump;
            // TODO: add jump logic
            this.Velocity += speed;
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


        public bool Run() {
            this.Stance = Stance.stand;
            this.Gait = Gait.run;
            // TODO: Add some running logic

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

        public bool IsRunning() {
            return this.Gait == Gait.run;
        }
        public bool Halt() {
            this.Velocity = new Vector2(0, 0);
            this.Gait = Gait.stop;
            this.Stance = Stance.stand;
            return true;
        }

        public bool IsStill() {
            return this.Gait == Gait.stop;
        }
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
        /// Updates the speed if the miner if possible.
        /// </summary>
        /// <param name="direction">Direction in which to move the miner</param>
        /// <returns>True iff 1==1</returns>
        public bool Move(Vector2 dv) {
            //TODO: Make compatible with game engine
            switch (this.Gait) {
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
        #region Old Code commented out
        #endregion
    }
}