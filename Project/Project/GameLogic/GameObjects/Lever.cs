using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.Util;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Lever : GameObject
    {
        public bool ON;
        public readonly int ActivationId;
        private string _leftLeverTexture;
        public string RightleverTexture;
        public Texture2D SecondTexture;

        public Lever(Vector2 position, Vector2 spriteSize, string textureString, int actId)
            : base(position, spriteSize)
        {
            TextureString = textureString;
            Speed = Vector2.Zero;
            ActivationId = actId;
            _leftLeverTexture = textureString;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            Falling = false;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Moveable = false;
            if (ON)
            {
                Texture2D tmp = Texture;
                Texture = SecondTexture;
                SecondTexture = tmp;
            }
            ON = false;
        }

        public void Interact(List<Platform> platforms)
        {
            if (!ON)
            {
                foreach(Platform p in platforms)
                    if (p.ActivationId == ActivationId) p.Activate = true;
            }
            else
            {
                foreach (Platform p in platforms)
                    if (p.ActivationId == ActivationId) p.Activate = false;
            }
            Texture2D tmp = Texture;
            Texture = SecondTexture;
            SecondTexture = tmp;
            ON = !ON;
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "lever";
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = ActivationId;
            obj.SecondTexture = _leftLeverTexture;
            obj.Tool = "-1";
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = -1f;
            return obj;
        }
    }
}

