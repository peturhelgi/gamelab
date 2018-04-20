using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    class GameState
    {
        public List<Miner> Actors;
        public List<GameObject> Solids;
        public List<GameObject> Collectibles;

        public GameState()
        {
            Actors = new List<Miner>();
            Solids = new List<GameObject>();
            Collectibles = new List<GameObject>();
        }

        public void LoadContent(ref Level level)
        {
            foreach(GameObject obj in level.Objects)
            {
                AddObject(obj);
            }
        }

        public void UnloadContent()
        {
            Actors.Clear();
            Solids.Clear();
            Collectibles.Clear();
        }

        public List<GameObject> GetAll()
        {
            return Actors.Concat(Solids).Concat(Collectibles).ToList();
        }

        public void AddObject(GameObject obj)
        {
            if(obj is Miner)
            {
                Actors.Add((Miner)obj);
            }
            else if(obj is Ground)
            {
                Solids.Add(obj);
            }
            else
            {
                Collectibles.Add(obj);
            }
        }

        public List<Miner> GetActors()
        {
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
