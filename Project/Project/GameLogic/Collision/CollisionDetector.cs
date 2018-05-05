using System.Collections.Generic;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.GameObjects;

namespace TheGreatEscape.GameLogic
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
