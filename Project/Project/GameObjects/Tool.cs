using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameObjects.Miner;

namespace Project.GameObjects
{
    abstract class Tool
    {
        abstract public void Use(Miner.Miner user, List<GameObject> gameObjects);
    }
}
