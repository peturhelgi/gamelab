using Microsoft.Xna.Framework;
using Project.GameObjects;
using Project.Util.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    public class CollisionDetector
    {
        public List<GameObject> FindCollisions(
            PolygonalCollisionObject collidable, List<GameObject> objects)
        {
            List<GameObject> results = new List<GameObject>();
            foreach(GameObject obj in objects)
            {
                if(collidable.Intersects(obj.BBox))
                {
                    results.Add(obj);
                }
            }
            return results;
        }
    }
}
