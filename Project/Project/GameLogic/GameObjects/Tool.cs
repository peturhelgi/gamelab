using System.Collections.Generic;

namespace Project.GameLogic.GameObjects
{
    abstract class Tool
    {
        abstract public void Use(Miner user, List<GameObject> gameObjects);
    }
}
