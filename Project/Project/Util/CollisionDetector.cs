using Microsoft.Xna.Framework;
using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util {

    class CollisionDetector {

        public List<GameObject> FindCollisions(BoundingBox box, List<GameObject> objects) {
            List<GameObject> results = new List<GameObject>();
            foreach(GameObject obj in objects) {
                if(box.Intersects(obj.BBox)) {
                    results.Add(obj);
                }
            }
            return results;
        }
    }
}
