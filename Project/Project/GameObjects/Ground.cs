﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
namespace Project.GameObjects
{
    public class Ground : GameObject
    {
        public Ground(Vector2 position, Vector2 spriteSize, string TextureString) {
            this.Position = position;
            this.Velocity = new Vector2(0);
            this.Mass = 1000;
            this.SpriteSize = spriteSize;
            this.Visible = true;
            this.TextureString = TextureString;
        }

        public override void LoadContent()
        {
            //TODO: Implement
        }
        public override void UnloadContent()
        {
            //TODO: Implement
        }
        public virtual void Update(GameTime gameTime)
        {
            //TODO: Implement
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //TODO: Implement
        }
    }
}
