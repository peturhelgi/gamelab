using System.Collections.Generic;

namespace TheGreatEscape.GameLogic.GameObjects
{
    abstract class Tool
    {
        abstract public void Use(Miner.Miner user, List<GameObject> gameObjects);
    }
}
