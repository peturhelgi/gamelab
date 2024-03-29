﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using TheGreatEscape.LevelManager;
using TheGreatEscape.Menu;

namespace TheGreatEscape.GameLogic.GameObjects {
    public class Door : GameObject
    {
        /// <summary>
        /// Specifies the edge (level, door)  which the current door leads to.
        /// </summary>
        public Tuple<int, int> OutEdge;
        public bool IsExit { get; private set; }
        public bool RequiresKey { get; set; }
        public int KeyId { get; set; }
        public int Id { get; set; }
        public bool Unlocked { get; private set; }
        public PlatformBackground LockedLight, UnlockedLight;

        public Door(Vector2 position, Vector2 spriteSize, string texture,
            object requiresKey = null, object keyId = null, object isExit = null,
            Tuple<int, int> outEdge = null)
            : base(position, spriteSize)
        {
            OutEdge = outEdge;
            KeyId = keyId == null ? 0 : (int)keyId;
            IsExit = isExit == null ? true : (bool)isExit;
            RequiresKey = requiresKey == null ? false : (bool)requiresKey;
            Unlocked = !RequiresKey;

            AddLights();
        }

        public override void Initialize()
        {
            base.Initialize();
            Movable = false;
            Visible = true;
            Unlocked = !RequiresKey;
            if (Unlocked)
            {
                Unlock();
            }
            else
            {
                Lock();
            }
        }

        public bool Open(params int[] keys)
        {
            if(!Unlocked && keys.Contains(KeyId))
            {
                Unlock();
            }
            return Unlocked;
        }

        public void Unlock()
        {
            Unlocked = true;
            LockedLight.Active = false;
            UnlockedLight.Active = true;
        }

        public void Lock()
        {
            Unlocked = false;
            LockedLight.Active = true;
            UnlockedLight.Active = false;
        }

        public void AddKey(int key)
        {
            KeyId = key;
            Lock();
        }

        public void RemoveKey(int key)
        {
            KeyId = 0;
            Unlock();
        }

        public override void Interact(GameState gameState)
        {
            base.Interact(gameState);
            {
                if (Unlocked)
                {
                    MenuManager.SoundsPlayer.PlayIngameSound(MenuManager.SoundToPlay.Door);
                    if (IsExit)
                    {
                        gameState.Completed = true;
                    }
                    else
                    {
                        // TODO: Set some other level
                    }
                }
            }
        }
        public void AddLights()
        {
            GameObjectFactory factory = new GameObjectFactory();
            Vector2 size = new Vector2(SpriteSize.Y) * 0.1f,
                        pos = Position
                        + new Vector2(0.5f, -0.1f) * SpriteSize
                        - 0.5f * size;

            LockedLight = factory.Create(
                new Obj
                {
                    Type = "secondary",
                    Position = pos,
                    SpriteSize = size,
                    TextureString = "Sprites/Misc/red_light"
                }) as PlatformBackground;

            UnlockedLight = factory.Create(
                new Obj
                {
                    Type = "secondary",
                    Position = pos,
                    SpriteSize = size,
                    TextureString = "Sprites/Misc/green_light"
                }) as PlatformBackground;

            LockedLight.Active = !Unlocked;
            UnlockedLight.Active = Unlocked;

        }
        public void SetLights()
        {
            Vector2 size = new Vector2(SpriteSize.Y) * 0.1f,
                        pos = Position
                        + new Vector2(0.5f, -0.1f) * SpriteSize
                        - 0.5f * size;
            UnlockedLight.Position = LockedLight.Position = pos;
            UnlockedLight.SpriteSize = LockedLight.SpriteSize = size;
        }

        public override string ToString()
        {
            return "door";
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = ToString();
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = -1;
            obj.SecondTexture = "-1";
            obj.Tool = "-1";
            obj.Id = KeyId;
            obj.Requirement = true;
            obj.RopeLength = -1f;
            return obj;
        }
    }
}
