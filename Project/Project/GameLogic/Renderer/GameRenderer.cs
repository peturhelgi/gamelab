using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Game.Renderer
{
    class GameRenderer
    {
        GraphicsDevice _graphicsDevice;
        GameState _gameState;
        SpriteBatch _spriteBatch;

        public GameRenderer(GraphicsDevice graphicsDevice, GameState gameState) {
            _graphicsDevice = graphicsDevice;
            _gameState = gameState;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public void Draw(GameTime gameTime, int width, int height)
        {


        }
    }
}
