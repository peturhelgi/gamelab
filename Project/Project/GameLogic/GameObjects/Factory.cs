﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGreatEscape.GameLogic.Util;
using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.LevelManager;
namespace TheGreatEscape.GameLogic.GameObjects
{
    public abstract class Factory<T>
    {
        public abstract T Create(
            Object obj);
    }

    public class ToolFactory : Factory<Tool>
    {
        public override Tool Create(Object obj)
        {
            string type = "pickaxe";
            Tool tool;
            switch(type)
            {
                case "pickaxe":
                    tool = new Pickaxe();
                    break;
                case "rope":
                default:
                    throw new NotImplementedException(
                        string.Format("Tool '{0}' cannot be created", type));
            }
            return tool;
        }
    }
    public class GameObjectFactory : Factory<GameObject>
    {

        public override GameObject Create(
            Object obj)
        {
            GameObject instance;
            Obj entity = obj as Obj;
            switch(entity.Type.ToLower())
            {
                case "miner":
                    instance = new Miner(
                        entity.Position,
                        entity.SpriteSize);
                    instance.Speed = entity.Velocity;
                    instance.Mass = entity.Mass;
                    instance.TextureString = entity.Texture;                    
                    break;
                case "ground":
                    instance = new Ground(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture);
                    instance.TextureString = entity.Texture;                    
                    break;
                case "rock":
                    instance = new Rock(
                        entity.Position,
                        entity.SpriteSize);
                    instance.TextureString = entity.Texture;                    
                    break;
                case "end":
                case "crate":
                case "ladder":
                case "lever":
                case "platform":
                default:
                    instance = null;
                    MyDebugger.WriteLine(
                        string.Format("GameObject '{0}' cannot be created",
                        entity?.Type), true);
                    break;
            }
            if(instance != null)
            {
                instance.Visible = true;
            }
            return instance;
        }
    }
}
