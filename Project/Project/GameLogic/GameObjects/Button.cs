using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.Util;
using TheGreatEscape.LevelManager;

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

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "button";
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = ActivationId;
            obj.SecondTexture = "-1";
            obj.Tool = "-1";
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = -1f;
            return obj;
        }
    }
}

