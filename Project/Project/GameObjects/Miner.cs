using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Project.GameObjects.Miner
{

    enum Gait { crawl, walk, run, jump};
    enum Stance { stand, jump, crouch, lie };
    class Miner : GameObject
    {
        // add some Tool variable
        Gait Gait;
        Stance Stance;
        public Miner(Vector2 position, Vector2 speed, double mass, BoundingBox box)
        {
            this.Position = position;
            this.Speed = speed;
            this.Mass = mass;
            this.Box = box;
            this.Gait = Gait.walk;
            this.Stance = Stance.stand;
        }

        /// <summary>
        /// Makes the miner jump if possible
        /// </summary>
        /// <returns>True if 1==1</returns>
        public bool Jump() {
            this.Stance = Stance.jump;
            this.Gait = Gait.jump;
            // TODO: add jump logic

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
        /// Updates the speed if the miner if possible.
        /// </summary>
        /// <param name="direction">Direction in which to move the miner</param>
        /// <returns>True iff 1==1</returns>
        public bool Move(Vector2 direction) {
            //TODO: add move logic, the one here is just an example
            switch (this.Gait) {
                case Gait.crawl:
                    this.Speed = direction/2; // for example, there could be some more logic here using our physics
                    break;

                case Gait.walk:
                    this.Speed = direction;   // for example, there could be some more logic here using our physics
                    break;

                case Gait.run:
                    this.Speed = direction*2; // for example, there could be some more logic here using our physics
                    break;

                case Gait.jump:
                    direction.Y = 0;
                    this.Speed = direction;   // for example, there could be some more logic here using our physics

                    break;
                default:
                    // Nothing happens yet
                    break;
            }

            this.Position += this.Speed;
            return true;
        }

        /// <summary>
        /// Uses the tool that the miner currenty has
        /// </summary>
        /// <returns>True iff 1==1</returns>
        public bool UseTool() {
            this.Stance = Stance.stand;
            // TODO: add some tool logic, using the tools interface

            return true;
        }
    }
}
