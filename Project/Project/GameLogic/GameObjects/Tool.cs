﻿using System.Collections.Generic;

namespace TheGreatEscape.GameLogic.GameObjects
{
    abstract class Tool
    {
        abstract public void Use(Miner user, List<GameObject> gameObjects);
    }
}
