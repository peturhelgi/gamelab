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
        private Miner miner1;
        private List<Rock> rocks;
        private List<Ground> ground_parts;

        public GameState() {
            this.rocks = new List<Rock>();
            this.ground_parts = new List<Ground>();
        }

        public void addMiner1(Miner miner1) {
            this.miner1 = miner1;
        }

        public void addRock(Rock rock) {
            rocks.Add(rock);
        }

        public void addGround(Ground ground) {
            ground_parts.Add(ground);
        }

        public Miner getMiner1() {
            return miner1;
        }

        public List<Rock> getRocks() {
            return rocks;
        }

        public List<Ground> getGround() {
            return ground_parts;
        }
    }
}
