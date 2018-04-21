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

            var background = (_gameState as GamePlayState)?.Background;
            if(!(background is null))
            {
                spriteBatch.Draw(background.Texture, background.Position,
                    background.SourceRect,
                    Color.White * background.Alpha, 0.0f, Vector2.Zero,
                    background.Scale, SpriteEffects.None, 0.0f);
            }
            
            foreach(var obj in _gameState.GetAll())
            {
                spriteBatch.Draw(obj.Image.Texture, obj.Image.Position, 
                    obj.Image.SourceRect,
                    Color.White * obj.Image.Alpha, 0.0f, Vector2.Zero, 
                    obj.Image.Scale,  SpriteEffects.None, 0.0f);
            }

            spriteBatch.End();
        }
    }
}
