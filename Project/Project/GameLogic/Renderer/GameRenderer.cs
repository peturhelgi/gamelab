using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic.Renderer
{
    class GameRenderer
    {
        GraphicsDevice _graphicsDevice;
        GameState _gameState;
        SpriteBatch _spriteBatch;
        Texture2D _debugBox;


        public enum Mode { Normal, DebugView}

        public GameRenderer(GraphicsDevice graphicsDevice, GameState gameState, ContentManager _content) {
            _graphicsDevice = graphicsDevice;
            _gameState = gameState;
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            _debugBox = _content.Load<Texture2D>("Sprites/Misc/box");

        }

        public void Draw(GameTime gameTime, int width, int height, Mode mode, Matrix camera)
        {
            _graphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(SpriteSortMode.Deferred, mode==Mode.DebugView?BlendState.Opaque:null, null, null, null, null, camera);
            
            RasterizerState state = new RasterizerState
            {
                FillMode = FillMode.WireFrame
            };

            _spriteBatch.GraphicsDevice.RasterizerState = state;

            foreach (GameObject obj in _gameState.GetAll())
            {
                if (obj.Visible)
                {
                        _spriteBatch.Draw(mode == Mode.DebugView ? _debugBox: obj.Texture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.SpriteSize.X, (int)obj.SpriteSize.Y), Color.White);  
                }
            }

            _spriteBatch.End();

        }
    }
}
