using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Project.GameObjects.Miner
{
    class Miner : GameObject
    {

        public Miner(Vector2 position, Vector2 speed, double mass, BoundingBox box)
        {
            Position = position;
            Speed = speed;
            Mass = mass;
            Box = box;
        }

       
    }
}
