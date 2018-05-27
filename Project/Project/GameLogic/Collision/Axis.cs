using Microsoft.Xna.Framework;

namespace TheGreatEscape.GameLogic.Collision {
    public class Axis
    {
        public Vector2 direction;
        public Axis(Vector2 direction) {
            direction.Normalize();
            this.direction = direction;
        }

        public bool Equals(Axis other)
        {
            return (this.direction == other.direction) ;
        }
    }
}
