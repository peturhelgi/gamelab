using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameObjects;

namespace Project.GameObjects {
    public abstract class Tool {
        // TODO: Remove MIner from parameters
        abstract public void Use(Miner user, List<GameObject> gameObjects);
    }
}
