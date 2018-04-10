using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Util
{
    public class Level
    {
        public List<Layer> Layer;
        public Vector2 TileDimensions;

        public Level()
        {
            Layer = new List<Layer>();
            TileDimensions = Vector2.Zero;
        }

        public void LoadContent()
        {
            foreach(Layer layer in Layer)
            {
                layer.LoadContent(TileDimensions);
            }
        }

        public void UnloadContent()
        {
            foreach (Layer layer in Layer)
            {
                layer.UnloadContent();
            }
        }

        public void Update(GameTime gameTime, ref Miner miner)
        {
            foreach (Layer layer in Layer)
            {
                layer.Update(gameTime, ref miner);
            }
        }

        public void Draw(SpriteBatch spriteBatch, string drawType)
        {
            foreach (Layer layer in Layer)
            {
                layer.Draw(spriteBatch, drawType);
            }
        }
    }
}
