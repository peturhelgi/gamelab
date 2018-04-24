using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic;
using Project.GameLogic.GameObjects;
using Project.GameLogic.GameObjects.Miner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorLogic
{
    class EditorRenderer
    {
        GraphicsDevice _graphicsDevice;
        GameState _gameState;
        SpriteBatch _spriteBatch;
        Texture2D _debugBox;
        EditorManager _manager;

        public enum Mode { Normal, DebugView }

        public EditorRenderer(GraphicsDevice graphicsDevice, GameState gameState, ContentManager content, EditorManager manager)
        {
            _graphicsDevice = graphicsDevice;
            _gameState = gameState;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _manager = manager;

            _debugBox = content.Load<Texture2D>("Sprites/Misc/box");
        }

        public void Draw(GameTime gameTime, int width, int height, Matrix camera)
        {

            SpriteBatch spriteBatch = new SpriteBatch(_graphicsDevice);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, null, camera);
            spriteBatch.Draw(_debugBox, new Rectangle((int)_manager.CursorPosition.X, (int)_manager.CursorPosition.Y, 10, 10), Color.White);
            spriteBatch.End();

        }
    }
}


