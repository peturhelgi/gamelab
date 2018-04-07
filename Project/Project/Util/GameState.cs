using Project.GameObjects;
using Project.GameObjects.Miner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    class GameState : Level
    {
        public List<GameObject> Actors;
        public List<GameObject> Solids;
        public List<GameObject> Collectibles;
        CollisionDetector CollisionDetector;


        public GameState() {
            Actors = new List<GameObject>();
            Solids = new List<GameObject>();
            Collectibles = new List<GameObject>();
            CollisionDetector = new CollisionDetector();
        }

        public List<GameObject> GetAll() {
            return Actors.Concat(Solids).Concat(Collectibles).ToList();
        }


        public void AddActor(GameObject actor)
        {
            Actors.Add(actor);
        }

        public List<GameObject> GetActors() {
            return Actors;
        }


        public void AddSolid(GameObject solid)
        {
            Solids.Add(solid);
        }
        public List<GameObject> GetSolids()
        {
            return Solids;
        }


        public void AddCollectible(GameObject collectible)
        {
            Collectibles.Add(collectible);
        }
        public List<GameObject> GetCollectibles()
        {
            return Collectibles;
        }



    }
}
