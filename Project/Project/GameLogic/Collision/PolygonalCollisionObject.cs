using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic.Collision
{
    public abstract class PolygonalCollisionObject
    {
        public List<Vector2> Points = new List<Vector2>();
        public List<Axis> Axis = new List<Axis>();


        // Important: The Points need to be given in order (CW or CCW doesn't matter)
        public PolygonalCollisionObject(List<Vector2> points) {
            Points = points;
            Axis = GetCollisionAxis();
        }

        bool OverlapOfProjections(Axis axis, List<Vector2> pointsA, List<Vector2> pointsB ) {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach(Vector2 p in pointsA) {
                float pos = Vector2.Dot(axis.direction, p);
                min = Math.Min(pos, min);
                max = Math.Max(pos, max);
            }

            float o_min = float.MaxValue;
            float o_max = float.MinValue;

            foreach (Vector2 p in pointsB)
            {
                
                float pos = Vector2.Dot(axis.direction, p);
                if (pos > min && pos < max)
                {
                    // We can return early, if a point is between the others
                    return true;
                }
                else {
                    o_min = Math.Min(pos, o_min);
                    o_max = Math.Max(pos, o_max);
                }
                
            }

            if (o_min > min && o_min < max || o_max > min && o_max < max || min > o_min && min < o_max)
            {
                return true;
            }

            return false;
        }

        public bool Intersects(PolygonalCollisionObject other) {

            // TODO: do not do this for duplicate axis
            // SAT: If there is always a collision on the Axis projections, then there is a collision in space
            foreach (Axis a in Axis) {
                if(!OverlapOfProjections(a, Points, other.Points))
                    return false;
            }

            foreach (Axis a in other.Axis)
            {
                if (!OverlapOfProjections(a, Points, other.Points))
                    return false;
            }

            return true;
        }


        List<Axis> GetCollisionAxis() {
            List<Axis> axis = new List<Axis>();
            for(int i = 0; i < Points.Count; i++) {
                Axis a = new Axis(Points[i] - Points[(i + 1) % Points.Count]);
                if (!Axis.Contains(a)) { // to enhance performance: We don't want to check for the same axis twice (e.g. for a rectangle)
                    axis.Add(a);
                }
            }
            return axis;
        }
    }
}
