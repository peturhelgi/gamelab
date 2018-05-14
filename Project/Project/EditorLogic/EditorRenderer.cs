using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.GameObjects;

namespace EditorLogic {
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
                Vector2 cursorMin = Vector2.Min(
                    _manager.CursorPosition, 
                    _manager.CursorPosition + _manager.CursorSize);
                Vector2 cursorMax = Vector2.Max(
                    _manager.CursorPosition, 
                    _manager.CursorPosition + _manager.CursorSize);

                // Draws cursor on screen
                _spriteBatch.Draw(
                    _debugBox,
                    new Rectangle( 
                        cursorMin.ToPoint(), 
                        (cursorMax - cursorMin).ToPoint()),           
                    Color.White
                    );


                if (_manager.CurrentObjects != null)
                {

                    // Current GameObjects
                    foreach (GameObject obj in _manager.CurrentObjects)
                    {
                        var size = obj.SpriteSize.ToPoint();
                        var pos = (obj.Position
                            + _manager.CursorPosition
                            - _manager.MovingStartPosition).ToPoint();

                        _spriteBatch.Draw(
                           obj.Texture,
                           new Rectangle(pos, size),
                           Color.White);
                    }
                }

                if (_manager.AuxiliaryObject != null)
                {
                    GameObject auxObj = _manager.AuxiliaryObject;
                    var size = auxObj.SpriteSize.ToPoint();
                    var pos = (auxObj.Position
                        + _manager.CursorPosition
                        - _manager.MovingStartPosition).ToPoint();

                    _spriteBatch.Draw(
                       auxObj.Texture,
                       new Rectangle(pos, size),
                       Color.White);
                }


                _spriteBatch.End();

            }


        }
    }
}


