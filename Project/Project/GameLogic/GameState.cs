
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.LevelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGreatEscape.GameLogic
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


        public void AddActor(Miner actor)
        {
            Actors.Add(actor);
        }

        public List<Miner> GetActors() {
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
