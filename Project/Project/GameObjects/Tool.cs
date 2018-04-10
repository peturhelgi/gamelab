using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameObjects;

namespace Project.GameObjects
{
    abstract class Tool
    {
        abstract public void use(Miner user, List<GameObject> gameObjects);
    }
}
