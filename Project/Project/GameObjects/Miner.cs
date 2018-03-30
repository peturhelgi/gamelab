using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Project.GameObjects.Miner
{

    enum Gait { stop, crawl, walk, run, jump};
    enum Stance { stand, jump, crouch, lie };
    class Miner : GameObject
    {
        Tool tool;
        Gait Gait;
        Stance Stance;
        public Miner(Vector2 position, Vector2 speed, double mass, BoundingBox box, World world)
        {
            this.Position = position;
            this.Speed    = speed;
            this.Mass     = mass;
            this.Box      = box;
            this.Visible  = true;
            this.Gait     = Gait.walk;
            this.Stance   = Stance.stand;
            this.tool = new Pickaxe();
            this.Body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(128f / 2f), 1f, ConvertUnits.ToSimUnits(position), BodyType.Dynamic);
            //this.Body = BodyFactory.CreateBody(world, ConvertUnits.ToSimUnits(position), 0f, BodyType.Dynamic); //
            Body.Restitution = 0.3f;
            Body.Friction = 1f;
        }

        /// <summary>
        /// Makes the miner jump if possible
        /// </summary>
        /// <returns>True if 1==1</returns>
        public bool Jump() {
            this.Stance = Stance.jump;
            this.Gait = Gait.jump;
            // TODO: add jump logic
            this.Body.ApplyLinearImpulse(new Vector2(0, -0.4f));
            //this.Speed = new Vector2(this.Speed.X, -400);

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
                    this.Body.ApplyLinearImpulse(dv);
                    this.Speed = dv;
                    break;
            }

            return true;
        }

       

        /// <summary>
        /// Uses the tool that the miner currenty has
        /// </summary>
        /// <returns>True iff 1==1</returns>
        public bool UseTool(List<GameObject> gameObjects) {
            this.Stance = Stance.stand;
            tool.use(this, gameObjects);

            return true;
        }
    }
}
