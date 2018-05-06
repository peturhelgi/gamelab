﻿using System;
using TheGreatEscape.GameLogic.Util;
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
            Obj entity = obj as Obj;
            Tool tool;
            switch(entity.Type)
            {
                case "pickaxe":
                    tool = new Pickaxe();
                    break;
                case "rope":
                    tool = new Rope();
                    break;
                default:
                    throw new NotImplementedException(
                        string.Format("Tool '{0}' cannot be created", entity.Type));
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
                        entity.SpriteSize)
                    {
                        Speed = entity.Velocity,
                        Mass = entity.Mass,
                        TextureString = entity?.Texture,
                        Handling = GameState.Handling.Actor,
                        Tool = (new ToolFactory()).Create(new Obj { Type = entity.Tool  })
                    };
                  break;
                case "ground":
                    instance = new Ground(
                        entity.Position,
                        entity.SpriteSize)
                    {
                        TextureString = entity?.Texture,
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "rock":
                    instance = new Rock(
                        entity.Position,
                        entity.SpriteSize)
                    {
                        Mass = entity.Mass,
                        TextureString = entity?.Texture,
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "end":
                    instance = new Door(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture)
                    {
                        Handling = GameState.Handling.Interact
                    };
                    break;
                case "crate":
                    instance = new Crate(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture)
                    {
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "ladder":
                    instance = new Ladder(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture)
                    {
                        Handling = GameState.Handling.None
                    };
                    break;
                case "platform":
                    instance = new Platform(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture,
                        entity.Displacement,
                        entity.Direction,
                        entity.ActivationKey,
                        entity.SecondTexture)
                    {
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "lever":
                    instance = new Lever(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture,
                        entity.ActivationKey)
                    {
                        Handling = GameState.Handling.None
                    };
                    (instance as Lever).RightleverTexture = entity?.SecondTexture;
                    break;
                case "button":
                    instance = new Button(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture,
                        entity.ActivationKey)
                    {
                        Handling = GameState.Handling.None
                    };
                    break;
                case "rockandhook":
                    instance = new RockHook(
                        entity.Position,
                        entity.SpriteSize,
                        entity.Texture,
                        entity.SecondTexture)
                    {
                        Handling = GameState.Handling.Solid
                    };
                    break;
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
                instance.Active = true;
            }
            return instance;
        }
    }
}
