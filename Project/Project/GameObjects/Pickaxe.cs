using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameObjects {
    public class Pickaxe : Tool {
        public override void Use(Miner user, List<GameObject> gameObjects) {
            foreach(GameObject obj in gameObjects) {
                if(obj is Rock && obj.Visible) {
                    //TODO: Make compatible with GameEngine
                    if(true) {
                        obj.Visible = false;
                    }

                }
            }
        }
    }

}
