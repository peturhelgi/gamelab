using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.GameObjects;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public abstract class Factory<T>
    {
        public enum ObjectType
        {
            Miner,
            Ground,
            Rock,
            Crate,
            Ladder,
            Lever,
            Platform,
            Pickaxe,
            Rope
        }
        public abstract T CreateGameObject(ObjectType type,
            params object[] args);
    }

    public class GameObjectFactory : Factory<GameObject>
    {

        public override GameObject CreateGameObject(ObjectType type,
            params object[] args)
        {

            switch(type)
            {
                case ObjectType.Miner:
                    return Activator.CreateInstance(typeof(Miner), args)
                        as GameObject;
                case ObjectType.Ground:
                    return Activator.CreateInstance(typeof(Ground), args)
                        as GameObject;
                case ObjectType.Rock:
                    return Activator.CreateInstance(typeof(Rock), args)
                        as GameObject;
                case ObjectType.Pickaxe:
                    return Activator.CreateInstance(typeof(Pickaxe), args)
                        as GameObject;
                case ObjectType.Rope:
                case ObjectType.Crate:
                case ObjectType.Ladder:
                case ObjectType.Lever:
                case ObjectType.Platform:

                default:
                    throw new Exception(string.Format("GameObject '{0}' cannot be created", type));
            }
        }
    }
}
