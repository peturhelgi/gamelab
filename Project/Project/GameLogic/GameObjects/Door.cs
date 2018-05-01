using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameLogic.GameObjects;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public class Door : GameObject
    {
        public Tuple<int, int> OutEdge;
        public bool IsExit { get; private set; }
        public bool RequiresKey { get; private set; }
        public int KeyId { get; private set; }
        public bool Unlocked { get; private set; }
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
                Unlocked = true;
            }
            return Unlocked;
        }
    }
}
