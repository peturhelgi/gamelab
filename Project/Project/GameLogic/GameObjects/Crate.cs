using Microsoft.Xna.Framework;
using System;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic.GameObjects {
    class Crate : GameObject
    {

        public Crate(Vector2 position, Vector2 spriteSize, string textureString)
            :base(position, spriteSize)
        {

            TextureString = textureString;
            Speed = Vector2.Zero;

            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            Falling = true;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Movable = true;
        }

        public override Obj GetObj()
        {
            Obj obj = new Obj();
            obj.SpriteSize = SpriteSize;
            obj.Position = Position;
            obj.Velocity = Speed;
            obj.Mass = (float)Mass;
            obj.Type = "crate";
            obj.TextureString = TextureString;
            obj.Displacement = 0;
            obj.Direction = "-1";
            obj.ActivationKey = -1;
            obj.SecondTexture = "-1";
            obj.Tool = "-1";
            obj.Id = -1;
            obj.Requirement = false;
            obj.RopeLength = -1f;
            return obj;
        }
    }
}
