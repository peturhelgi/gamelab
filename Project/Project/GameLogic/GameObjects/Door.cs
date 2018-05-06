using System;
using System.Linq;
using Microsoft.Xna.Framework;

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
            : base(position, spriteSize, texture)
        {
            Movable = false;
            Visible = true;
            OutEdge = outEdge;
            KeyId = keyId == null ? 0 : (int)keyId;
            IsExit = isExit == null ? true : (bool)isExit;
            RequiresKey = requiresKey == null ? false : (bool)requiresKey;
            Unlocked = !RequiresKey;      
            
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

        public override string ToString()
        {
            return "door";
        }
    }
}
