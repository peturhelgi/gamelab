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
        private List<Miner> Miners;
        private List<Rock> Rocks;

        public GameState() {
            this.Rocks = new List<Rock>();
            this.Miners = new List<Miner>();
        }

        public void AddMiner(Miner miner) {
            this.Miners.Add(miner);
        }

        public void AddRock(Rock rock) {
            Rocks.Add(rock);
        }

        public Miner GetMiner(int id) {
            id = Math.Max(0, id);
            id = Math.Min(Miners.Count - 1, id);
            return Miners[id];
        }

        public List<Rock> GetRocks() {
            return Rocks;
        }
    }
}
