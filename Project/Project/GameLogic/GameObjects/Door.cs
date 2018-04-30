using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameLogic.GameObjects;

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
            int keyId = 0, bool requiresKey = false, bool isExit = true,
            Tuple<int, int> outEdge = null)
            : base(position, spriteSize, texture)
        {
            Movable = false;
            Visible = true;
            IsExit = isExit;
            OutEdge = outEdge;
            RequiresKey = requiresKey;
            KeyId = keyId;
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
