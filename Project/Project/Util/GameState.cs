using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    class GameState : Level
    {
        public List<Miner> Actors;
        public List<GameObject> Solids;
        public List<GameObject> Collectibles;
        CollisionDetector CollisionDetector;


        public GameState() {
            Actors = new List<Miner>();
            Solids = new List<GameObject>();
            Collectibles = new List<GameObject>();
            CollisionDetector = new CollisionDetector();
        }

        public List<GameObject> GetAll() {
            return Actors.Concat(Solids).Concat(Collectibles).ToList();
        }

        public void AddObject(GameObject obj)
        {
            if(obj is Miner)
            {
                Actors.Add((Miner)obj);
            } else if(obj is Ground)
            {
                Solids.Add(obj);
            } else
            {
                // Console.WriteLine("Trying to add " + obj.Type + ".");
                Collectibles.Add(obj);
            }
        }

        public List<Miner> GetActors() {
            return Actors;
        }

        public List<GameObject> GetSolids()
        {
            return Solids;
        }

        public List<GameObject> GetCollectibles()
        {
            return Collectibles;
        }



    }
}
