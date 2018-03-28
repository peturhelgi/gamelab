using Project.GameObjects;
using Project.GameObjects.Miner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    class GameState
    {
        private Miner miner1;
        private List<Rock> rocks;

        public GameState() {
            this.rocks = new List<Rock>();
        }

        public void addMiner1(Miner miner1) {
            this.miner1 = miner1;
        }

        public void addRock(Rock rock) {
            rocks.Add(rock);
        }

        public Miner getMiner1() {
            return miner1;
        }

        public List<Rock> getRocks() {
            return rocks;
        }
    }
}
