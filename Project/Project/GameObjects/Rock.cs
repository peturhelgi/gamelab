using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Project.GameObjects
{
    class Rock : GameObject
    {
        public Rock(Vector2 position,  Vector2 spriteSize, string textureString, bool destroyable = true, bool movable = false) {

            this.Destroyable = destroyable;
            this.Movable = movable;
            this.Mass = 10;
            this.Position = position;
            this.Velocity = new Vector2(0);
            this.SpriteSize = spriteSize;
            this.TextureString = textureString;
            this.Visible = true;
        }

        void Respawn() {
            // TODO: set variables to initial values
        
        }

        void Destroy() {
            if (!this.Destroyable) return;

            // TODO: add destoy logic
        }

        void Move() {
            if (!this.Movable) return;

            //TODO: add move logic
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
