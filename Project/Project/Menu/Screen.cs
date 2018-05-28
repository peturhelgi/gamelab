using EditorLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGreatEscape.GameLogic;

using static TheGreatEscape.Menu.MenuManager;

namespace TheGreatEscape.Menu
{
    public abstract class Screen
    {
        protected MenuManager _manager;
        protected GraphicsDevice _graphicsDevice;

        public Screen(GraphicsDevice graphicsDevice, MenuManager manager)
        {
            _manager = manager;
            _graphicsDevice = graphicsDevice;
        }

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(GameTime gameTime, int widht, int height);

    }

    class Selection
    {
        public string Text;
        public MenuManager.Action Action;
        public object Value;
        public Rectangle Position;
        public Selection(string text, MenuManager.Action action, object value,
            Rectangle position)
        {
            Text = text;
            Action = action;
            Value = value;
            Position = position;
        }
    }

    class GameScreen : Screen
    {
        GameManager _gameManager;
        public GameScreen(GameManager gameManager, GraphicsDevice graphicsDevice, MenuManager manager) : base(graphicsDevice, manager)
        {
            _gameManager = gameManager;
        }

        public void LoadGame(String level)
        {
            _gameManager.UnloadContent();
            _gameManager.LoadLevel(level);
            _manager.PlaySound(SoundToPlay.Ingame);

        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _gameManager.Draw(gameTime, width, height);
        }

        public override void Update(GameTime gameTime)
        {

            if (_manager.KeyPressed(Keys.Escape) ||
                _manager.ButtonPressed(0, Buttons.Start) ||
                _manager.ButtonPressed(1, Buttons.Start))
            {
                _manager.CallAction(MenuManager.Action.ShowPauseMenu, null);
            }

            else if (_manager.KeyPressed(Keys.Space))
            {
                _manager.CallAction(MenuManager.Action.ShowPauseMenu, null);
            }
            else
            {
                _gameManager.Update(gameTime);
            }
        }
    }

    class EditorScreen : Screen
    {
        EditorManager _editorManager;
        public EditorScreen(EditorManager editorManager, GraphicsDevice graphicsDevice, MenuManager manager) : base(graphicsDevice, manager)
        {
            _editorManager = editorManager;
        }

        public void LoadGame(String level)
        {
            _editorManager.LoadLevel(level);
        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _editorManager.Draw(gameTime, width, height);
        }

        public override void Update(GameTime gameTime)
        {
            if (_manager.ButtonPressed(0, Buttons.Start)
                || _manager.CurrKeyboardState.IsKeyDown(Keys.S))
            {
                _manager.CallAction(MenuManager.Action.ShowPauseMenu, 0);
            }
            else if ((_manager.KeyPressed(Keys.Escape) ||
                _manager.ButtonPressed(0, Buttons.Start) ||
                _manager.ButtonPressed(1, Buttons.Start)))
            {
                _manager.CallAction(MenuManager.Action.ShowMainMenu, null);
            }
            else
            {
                _editorManager.Update(gameTime);
            }
        }
    }

    class StoryScreen : Screen
    {
        SpriteBatch _spriteBatch;
        ContentManager _content;
        List<Texture2D> _story;
        Texture2D _currentSlide;
        GraphicsDevice _graphics;

        int slideCnt;

        int mAlphaValue = 1;
        int mFadeIncrement = 20;
        double mFadeDelay = .035;

        public StoryScreen(GraphicsDevice graphicsDevice, MenuManager manager) : base(graphicsDevice, manager)
        {
            _graphics = graphicsDevice;
            _story = new List<Texture2D>();
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _content = manager.ContentLoader();
            slideCnt = 0;
        }

        public void LoadStory()
        {
            slideCnt = 0;
            _story.Add(_content.Load<Texture2D>("Backstory/Storyboard_background"));
            for (int i = 1; i <= 11; ++i)
            {
                _story.Add(_content.Load<Texture2D>("Backstory/Storyboard_" + i));
            }
            _currentSlide = _story[slideCnt++];

            _manager.PlaySound(SoundToPlay.Story);
        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            Rectangle dest = new Rectangle(0, 0, _graphicsDevice.PresentationParameters.BackBufferWidth
                , _graphicsDevice.PresentationParameters.BackBufferHeight);
            _spriteBatch.Draw(_currentSlide, dest, new Color((byte)255, (byte)255, (byte)255, (byte)MathHelper.Clamp(mAlphaValue, 0, 225)));
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mFadeDelay <= 0)
            {
                mFadeDelay = .035;
                mAlphaValue += mFadeIncrement;
                if (mAlphaValue >= 255 || mAlphaValue <= 0)
                {
                    mFadeIncrement *= -1;
                }
            }

            if (_manager.ButtonPressed(0, Buttons.A)
                || _manager.CurrKeyboardState.IsKeyDown(Keys.A))
            {
                if (slideCnt == 12)
                {
                    _manager.CallAction(MenuManager.Action.StartGame, "Levels/level_1");
                    return;
                }
                _currentSlide = _story[slideCnt++];
            }
        }
    }

    class CreditsScreen : Screen
    {
        SpriteBatch _spriteBatch;
        ContentManager _content;
        List<Texture2D> _credits;
        Texture2D _currentSlide;
        GraphicsDevice _graphics;

        int slideCnt;

        int mAlphaValue = 1;
        int mFadeIncrement = 20;
        double mFadeDelay = .035;

        public CreditsScreen(GraphicsDevice graphicsDevice, MenuManager manager) : base(graphicsDevice, manager)
        {
            _graphics = graphicsDevice;
            _credits = new List<Texture2D>();
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _content = manager.ContentLoader();
            slideCnt = 0;
        }

        public void LoadCredits()
        {
            slideCnt = 0;
            for (int i = 1; i <= 3; ++i)
            {
                _credits.Add(_content.Load<Texture2D>("Credits/Credits_" + i));
            }
            _currentSlide = _credits[slideCnt++];

            _manager.PlaySound(SoundToPlay.Story);
        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            Rectangle dest = new Rectangle(0, 0, _graphicsDevice.PresentationParameters.BackBufferWidth
                , _graphicsDevice.PresentationParameters.BackBufferHeight);
            _spriteBatch.Draw(_currentSlide, dest, new Color((byte)255, (byte)255, (byte)255, (byte)MathHelper.Clamp(mAlphaValue, 0, 225)));
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mFadeDelay <= 0)
            {
                mFadeDelay = .035;
                mAlphaValue += mFadeIncrement;
                if (mAlphaValue >= 255 || mAlphaValue <= 0)
                {
                    mFadeIncrement *= -1;
                }
            }

            if (_manager.ButtonPressed(0, Buttons.A)
                || _manager.CurrKeyboardState.IsKeyDown(Keys.A))
            {
                if (slideCnt == 3)
                {
                    _manager.CallAction(MenuManager.Action.ShowMainMenu, null);
                    return;
                }
                _currentSlide = _credits[slideCnt++];
            }
        }
    }
    class HelpScreen : Screen
    {
        SpriteBatch _spriteBatch;
        Texture2D _helpImage;

        public HelpScreen(GraphicsDevice graphicsDevice, MenuManager manager, Texture2D helpImage) : base(graphicsDevice, manager)
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _helpImage = helpImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void Draw(GameTime gameTime, int width, int height)
        {
            _spriteBatch.Begin();
            int imgH = Math.Min(_helpImage.Height, _graphicsDevice.PresentationParameters.BackBufferHeight);
            int imgW;
            if (imgH == _helpImage.Height)
                imgW = _helpImage.Width;
            else
                imgW = _helpImage.Width * imgH / _helpImage.Height;

            var screenCenter = new Vector2(
            _graphicsDevice.Viewport.Bounds.Width / 2,
            _graphicsDevice.Viewport.Bounds.Height / 2);
            var textureCenter = new Vector2(
            imgW / 2,
            imgH / 2);

            int offset = (_graphicsDevice.PresentationParameters.BackBufferWidth - imgW) / 2;
            Rectangle dest = new Rectangle(offset, 0, imgW, imgH);
            _spriteBatch.Draw(_helpImage, dest, Color.White);

            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (_manager.ButtonPressed(0, Buttons.Y)
               || _manager.CurrKeyboardState.IsKeyDown(Keys.Y))
            {
                _manager.CallAction(MenuManager.Action.ShowHelp, null);
            }

        }
    }

    class SelectionMenu : Screen
    {

        protected List<Selection> _selections;
        protected int _currentPosition = 0;
        int _lastSelection = -1;
        protected SpriteBatch _spriteBatch;
        protected String _title;
        protected Texture2D _background;
        protected Texture2D _selector;
        protected Boolean _isDynamic;


        public SelectionMenu(string title, Texture2D background, Texture2D selector, Boolean isDynamic, GraphicsDevice graphicsDevice,
            MenuManager manager) : base(graphicsDevice, manager)
        {
            _selections = new List<Selection>();
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _title = title;
            _background = background;
            _selector = selector;
            _isDynamic = isDynamic;
        }

        public void AddSelection(string text, MenuManager.Action action, object value, Rectangle position)
        {
            _selections.Add(new Selection(text, action, value, position));
        }

        public void AddSelection(Selection selection)
        {
            if (selection != null)
            {
                _selections.Add(selection);
            }
        }

        public void SetSelection(int index, Selection selection)
        {
            if (selection == null)
            {
                return;
            }

            if (_selections.IndexOf(selection) == -1)
            {
                if (index < _selections.Count)
                {
                    _selections[index] = selection;
                }
                else
                {
                    AddSelection(selection);
                }
            }
        }

        public Selection GetSelection(int index)
        {
            if (_selections.Count > 0)
            {
                return _selections[index % _selections.Count];
            }
            return null;
        }
        public Selection GetLastSelection()
        {
            if (_lastSelection != -1 && _selections.Count > 0)
            {
                return _selections[_lastSelection];
            }
            return null;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO add support for (multiple) Controllers
            // Keyboard controls
            if (_manager.KeyPressed(Keys.Enter, Keys.Space))
            {
                _manager.CallAction(_selections[_currentPosition].Action, _selections[_currentPosition].Value);
                _lastSelection = _currentPosition;
                _currentPosition = 0;
            }

            if (_manager.KeyPressed(Keys.Down))
            {
                _currentPosition = (++_currentPosition) % _selections.Count;
            }

            if (_manager.KeyPressed(Keys.Up))
            {
                _currentPosition = --_currentPosition < 0 ? _selections.Count - 1 : _currentPosition;
            }


            if (_selections.Count > 0)
            {
                // Xbox controls for player one
                if (_manager.ButtonPressed(0, Buttons.LeftThumbstickDown, Buttons.DPadDown))
                {
                    _currentPosition = (++_currentPosition) % _selections.Count;
                }
                if (_manager.ButtonPressed(0, Buttons.LeftThumbstickUp, Buttons.DPadUp))
                {
                    _currentPosition = --_currentPosition < 0 ? _selections.Count - 1 : _currentPosition;
                }
            }
            if (_manager.ButtonPressed(0, Buttons.A))
            {
                _manager.CallAction(_selections[_currentPosition].Action, _selections[_currentPosition].Value);
                _currentPosition = 0;
            }

            if (_manager.ButtonPressed(0, Buttons.Back))
            {
                _manager.CallAction(MenuManager.Action.Back, null);
                _currentPosition = 0;
            }
            if (_manager.ButtonPressed(0, Buttons.Y))
            {
                _manager.CallAction(MenuManager.Action.ShowHelp, 0);
                _currentPosition = 0;
            }

        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, width, height), Color.White);

            float stretchX = (float)width / 1920f;
            float stretchY = (float)height / 1080f;
            Selection sel = _selections.Count > _currentPosition
                ? _selections[_currentPosition]
                : null;

            if (!_isDynamic && sel != null)
            {
                _spriteBatch.Draw(
                    _selector,
                    new Rectangle(
                        (int)(sel.Position.Right * stretchX),
                        (int)(sel.Position.Top * stretchY),
                        (int)(140f * stretchX),
                        (int)(100f * stretchY)),
                    Color.White);
            }

            else
            {
                int maxNumOnScreen = (int)(height / _manager.MenuFont.MeasureString("M").Y);
                int min_id, items_on_screen = Math.Min(maxNumOnScreen, _selections.Count),
                    half = items_on_screen / 2;
                if (_currentPosition < half)
                {
                    min_id = 0;
                }
                else if (_currentPosition >= _selections.Count - half)
                {
                    min_id = _selections.Count - items_on_screen;
                }
                else
                {
                    min_id = _currentPosition - half;
                }
                Vector2 yOffsetter = new Vector2(
                    0,
                    (int)_manager.MenuFont.MeasureString("M").Y);
                Vector2 xOffsetter = new Vector2(50, 0),
                    ySpacing = new Vector2(0, 10),
                    origin, offset;

                for (var i = 0; i < items_on_screen; i++)
                {
                    int idx = min_id + i;
                    // If level exists.
                    if (idx < _selections.Count)
                    {
                        offset = xOffsetter
                            + i * yOffsetter + ySpacing;
                        origin = _manager.MenuFont.MeasureString(_selections[idx].Text) / 2;
                        _spriteBatch.DrawString(
                            _manager.MenuFont,
                            _selections[idx].Text,
                             offset + origin,
                            Color.White, 0f, origin,
                            1.0f, new SpriteEffects(), 0f);
                    }
                }

                if (sel == null)
                {
                    _spriteBatch.End();
                    return;
                }


                Vector2 position = new Vector2(
                    (int)(xOffsetter.X
                    + _manager.MenuFont.MeasureString(
                        _selections[_currentPosition].Text + "-").X),
                    0
                    );

                int total = _selections.Count;

                position.Y = ySpacing.Y;
                if (_currentPosition < half)
                {
                    position.Y += _currentPosition * yOffsetter.Y;
                }
                else if (_currentPosition >= total - half)
                {
                    position.Y += (_currentPosition - min_id) * yOffsetter.Y;
                }
                else
                {
                    position.Y += (half) * yOffsetter.Y;
                }

                origin = new Vector2(
                    (int)(0f * stretchX),
                    (int)_selector.Bounds.Size.Y) / 2;

                Vector2 scale = (_manager.MenuFont.MeasureString("level")) / _selector.Bounds.Size.ToVector2();
                _spriteBatch.Draw(_selector,
                    position + origin * scale, //position
                    new Rectangle(Point.Zero, _selector.Bounds.Size), //source rectangle
                    Color.White, //color
                    0.0f, //rotation
                    origin, //origin
                    scale, //scale
                    SpriteEffects.None, //effects
                    0.0f);
            }

            _spriteBatch.End();
        }

    }

    class PopOverMenu : SelectionMenu
    {
        float _ratioX;
        float _ratioY;
        float _alpha;
        public PopOverMenu(string title, Texture2D background, Texture2D selector, Boolean isDynamic, GraphicsDevice graphicsDevice,
            MenuManager manager, float ratioX = 1.0f, float ratioY = 1.0f,
            float alpha = 1.0f) :
            base(title, background, selector, isDynamic, graphicsDevice, manager)
        {
            _ratioX = ratioX;
            _ratioY = ratioY;
            _alpha = alpha;
        }
    }
}
