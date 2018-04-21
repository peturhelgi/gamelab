using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameObjects;
using Project.Screens;
using Project.Util;
using Project.GameStates;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Render
{
    public class Renderer
    {
        protected GameState _gameState;
        protected Camera _camera;
        public Renderer()
        {

        }

        public virtual void Initialize(ref GameState gameState,
            ref Camera camera)
        {
            _gameState = gameState;
            _camera = camera;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null,
                null, _camera?.view);

            //TODO: Remove Image.Draw()
            if(_gameState != null)
            {
                foreach(var obj in _gameState.GetAll())
                {
                    obj.Image.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }
}
