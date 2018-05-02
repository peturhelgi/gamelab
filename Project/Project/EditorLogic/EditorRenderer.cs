using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.GameObjects;
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




            if (_manager.ObjectPickerOpen)
            {
                Vector2 dimensions = new Vector2(width, height);
                Vector2 pickerSize = new Vector2(width / 2);
                Vector2 pickerPosition = (dimensions - pickerSize) / 2;

                _spriteBatch.Begin();
                _manager.CircularSelector.Draw(_spriteBatch, pickerSize, pickerPosition);
                _spriteBatch.End();

            }
            else
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera);

                // Cursor
                Vector2 cursorMin = Vector2.Min(_manager.CursorPosition, _manager.CursorPosition + _manager.CursorSize);
                Vector2 cursorMax = Vector2.Max(_manager.CursorPosition, _manager.CursorPosition + _manager.CursorSize);
                _spriteBatch.Draw(
                    _debugBox,
                    new Rectangle(
                        (int)cursorMin.X,
                        (int)cursorMin.Y,
                        (int)(cursorMax.X - cursorMin.X),
                        (int)(cursorMax.Y - cursorMin.Y)
                        ),
                    Color.White
                    );


                if (_manager.CurrentObjects != null)
                {

                    // Current GameObjects
                    foreach (GameObject obj in _manager.CurrentObjects)
                    {
                        _spriteBatch.Draw(
                           obj.Texture,
                           new Rectangle(
                               (int)(obj.Position.X + (_manager.CursorPosition.X - _manager.MovingStartPosition.X)),
                               (int)(obj.Position.Y + (_manager.CursorPosition.Y - _manager.MovingStartPosition.Y)),
                               (int)obj.SpriteSize.X,
                               (int)obj.SpriteSize.Y),
                           Color.White);
                    }
                }
                _spriteBatch.End();

            }


        }
    }
}


