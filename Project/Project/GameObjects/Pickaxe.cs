using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameObjects
{
    class Pickaxe : Tool
    {
        public override void use(Miner.Miner user, List<GameObject> gameObjects)
        {
            foreach (GameObject obj in gameObjects) {
                if (obj is Rock && obj.visible) { 
                    //TODO: add collision detection
                    if (true)
                    {
                        obj.visible = false;
                    }
                    
                }
            }
        }
    }
}
