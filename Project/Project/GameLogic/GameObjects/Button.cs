using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Button : GameObject
    {
        public bool ON;
        public readonly int ActivationId;

        public Button(Vector2 position, Vector2 spriteSize, string textureString, int actId)
            : base(position, spriteSize)
        {

            TextureString = textureString;
            Position = position;
            SpriteSize = spriteSize;
            Falling = false;

            Speed = Vector2.Zero;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Moveable = false;
            ON = false;
            ActivationId = actId;

        }

        public void Interact(List<Platform> platforms)
        {
            foreach (Platform p in platforms)
                if (p.ActivationId == ActivationId) p.Activate = !p.Activate;
            ON = !ON;
        }
    }
}

