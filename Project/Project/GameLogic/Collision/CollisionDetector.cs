using Project.GameLogic.Collision;
using Project.GameLogic.GameObjects;
using System.Collections.Generic;
using System.Diagnostics;
using Project.GameLogic.GameObjects.Miner;

namespace Project.GameLogic
{

    class CollisionDetector
    {

        public List<GameObject> FindCollisions(PolygonalCollisionObject collidable, List<GameObject> objects)
        {
            List<GameObject> results = new List<GameObject>();
            foreach (GameObject obj in objects)
            {
                if (collidable.Intersects(obj.BBox))
                {
                    results.Add(obj);
                }
            }
            return results;
        }
    }
}
