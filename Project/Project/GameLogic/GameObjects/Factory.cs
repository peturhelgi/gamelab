using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TheGreatEscape.Util;
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
            switch (entity.Type)
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
        ToolFactory _toolFactory = new ToolFactory();
        public static int currentKey = 0;
        public static int currentDoor = 0;
        public static int activationKeys = 0;
        public override GameObject Create(
            Object obj)
        {
            GameObject instance;
            Obj entity = obj as Obj;
            switch (entity.Type.ToLower())
            {
                case "miner":
                    instance = new Miner(
                        entity.Position,
                        entity.SpriteSize)
                    {
                        Speed = entity.Velocity,
                        Mass = entity.Mass,
                        TextureString = entity?.TextureString,
                        Handling = GameState.Handling.Actor,
                        Tool = _toolFactory.Create(new Obj { Type = entity.Tool })
                    };
                    break;
                case "ground":
                    instance = new Ground(
                        entity.Position,
                        entity.SpriteSize)
                    {
                        TextureString = entity?.TextureString,
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "rock":
                    instance = new Rock(
                        entity.Position,
                        entity.SpriteSize)
                    {
                        // compute the mass of the rock depending on the sprite size 
                        // and consider the density as being 1/750
                        Mass = entity.SpriteSize.X * entity.SpriteSize.Y / 750f,
                        TextureString = entity?.TextureString,
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "door":
                    instance = new Door(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString)
                    {
                        Handling = GameState.Handling.Interact,
                        RequiresKey = entity.Requirement,
                        KeyId = entity.Id,
                        TextureString = entity.TextureString,
                        Id = currentDoor++
                    };
                    
                    bool unlocked = !entity.Requirement;
                    if (entity.Requirement)
                    {
                        (instance as Door).AddKey(entity.Id);
                    }
                    break;
                case "crate":
                    instance = new Crate(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString)
                    {
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "ladder":
                    instance = new Ladder(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString)
                    {
                        Handling = GameState.Handling.None
                    };
                    break;
                case "platform":
                    instance = new Platform(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString,
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
                        entity.TextureString,
                        entity.ActivationKey)
                    {
                        Handling = GameState.Handling.None,
                        RightleverTexture = entity?.SecondTexture
                    };
                    //(instance as Lever).RightleverTexture = entity?.SecondTexture;
                    break;
                case "button":
                    instance = new Button(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString,
                        entity.ActivationKey)
                    {
                        Handling = GameState.Handling.None
                    };
                    activationKeys++;
                    break;
                case "rockandhook":
                    entity.SecondTexture = "Sprites/Misc/Rope";
                    instance = new RockHook(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString,
                        entity.SecondTexture,
                        entity.RopeLength)
                    {
                        Handling = GameState.Handling.Solid
                    };
                    break;
                case "secondary":
                    instance = new PlatformBackground(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString);
                    break;
                case "key":
                    instance = new Key(
                        entity.Position,
                        entity.SpriteSize)
                    {
                        TextureString = entity.TextureString,
                        Handling = GameState.Handling.Collect,
                        Id = entity.Id
                    };
                    currentKey++;
                    break;
                case "sign":
                    instance = new Sign(
                        entity.Position,
                        entity.SpriteSize,
                        entity.TextureString)
                    {
                        Handling = GameState.Handling.None
                    };
                    break;
                default:
                    instance = null;
                    MyDebugger.WriteLine(
                        string.Format("GameObject '{0}' cannot be created",
                        entity?.Type), true);
                    break;
            }
            if (instance != null)
            {
                instance.Visible = true;
                instance.Active = true;
            }
            return instance;
        }
    }
}
