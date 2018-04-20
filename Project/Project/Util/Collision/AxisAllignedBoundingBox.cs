using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util.Collision
{
    public class AxisAlignedBoundingBox : PolygonalCollisionObject
    {
        public Vector2 Max;
        public Vector2 Min;

        public AxisAlignedBoundingBox(Vector2 min, Vector2 max) :
            base(new List<Vector2>
            {
                min, new Vector2(min.X, max.Y), max ,
                new Vector2(max.X, min.Y)
            })
        {
            Max = max;
            Min = min;
        }

    }
}
