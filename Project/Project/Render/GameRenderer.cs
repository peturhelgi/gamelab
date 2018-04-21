using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.Util;
using Project.Screens;
using Project.GameObjects;
using Project.GameStates;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Render
{
    public class GameRenderer : Renderer
    {
        public GameRenderer() : base()
        {
        }

        public override void Initialize(
            ref GameState gameState, ref Camera camera)
        {
            _gameState = gameState as GamePlayState;
            _camera = camera;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null,
                null, _camera.view);
            (_gameState as GamePlayState).Background?.Draw(spriteBatch);
            
            //TODO: Remove Image.Draw()
            foreach(var obj in _gameState.GetAll())
            {
                obj.Image.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
