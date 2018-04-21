using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameObjects;
using Project.Screens;
using Project.Util;
using Project.GameStates;
using Microsoft.Xna.Framework;
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

            if(_gameState != null)
            {
                foreach(var obj in _gameState.GetAll())
                {
                    spriteBatch.Draw(obj.Image.Texture, obj.Image.Position,
                        obj.Image.SourceRect,
                        Color.White * obj.Image.Alpha, 0.0f, Vector2.Zero,
                        obj.Image.Scale, SpriteEffects.None, 0.0f);
                }
            }

            spriteBatch.End();
        }
    }
}
