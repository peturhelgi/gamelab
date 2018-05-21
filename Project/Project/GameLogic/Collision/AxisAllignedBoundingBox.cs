using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TheGreatEscape.GameLogic.Collision {
    public class AxisAllignedBoundingBox : PolygonalCollisionObject
    {
        public Vector2 Max;
        public Vector2 Min;
            
        public AxisAllignedBoundingBox(Vector2 min, Vector2 max) : base(new List<Vector2> { min, new Vector2(min.X, max.Y), max , new Vector2(max.X, min.Y) }) {
            Max = max;
            Min = min;
        }
        
    }
}
