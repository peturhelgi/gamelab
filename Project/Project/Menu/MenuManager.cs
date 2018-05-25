using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using EditorLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TheGreatEscape.GameLogic;

namespace TheGreatEscape.Menu
{
    public class MenuManager
    {
        Screen _currentScreen;
        Screen _prevScreen;
        SelectionMenu _mainMenu;
        SelectionMenu _levelSelector;
        PopOverMenu _popOver;
        PopOverMenu _gameOver;
        PopOverMenu _levelCompleted;
        PopOverMenu _pauseGame;
        PopOverMenu _editorHelp;
        PopOverMenu _editorMenu;
        PopOverMenu _gameHelps;

        StoryScreen _story;
        GameScreen _game;
        LoadingScreen _loading;
        EditorScreen _editor;

        IAsyncResult result;
        bool _requestSave = false;

        public enum Action
        {
            StartStory,
            StartGame,
            ShowMainMenu,
            ShowOptionsMenu,
            ShowLevelSelector,
            ShowLevelCompletedScreen,
            ShowGameOverScreen,
            ShowPauseMenu,
            ResumeGame,
            ShowLevelEditor,
            ExitGame,
            Advance,
            Back,
            SaveLevel,
            ShowHelp,
            ToggleEdit
        };

        GameManager _gameManager;
        EditorManager _editorManager;
        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;
        String _currentLevel;
        int _currentLevelIdx;
        List<String> _allLevels;
        GreatEscape _theGame;
        List<Screen> screenStack;
        String TemplateName = "Template";
        public static Song MenuSong, IngameSong, StorySong;
        //public static SoundEffectInstance Song1, Song2;

        public KeyboardState OldKeyboardState, CurrKeyboardState;
        public GamePadState OldPlayerOneState, CurrPlayerOneState;
        public GamePadState OldPlayerTwoState, CurrPlayerTwoState;
        // TODO: move to renderer
        // Assets for the Menu
        public SpriteFont MenuFont;

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (OldKeyboardState.IsKeyUp(key)
                    && CurrKeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonDown(int player, params Buttons[] buttons)
        {
            var currpadState = player == 0 ? CurrPlayerOneState : CurrPlayerTwoState;
            foreach (var button in buttons)
            {
                if (currpadState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonUp(int player, params Buttons[] buttons)
        {
            var currpadState = player == 0 ? CurrPlayerOneState : CurrPlayerTwoState;
            foreach (var button in buttons)
            {
                if (currpadState.IsButtonDown(button))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ButtonPressed(int player, params Buttons[] buttons)
        {
            var oldpadState = player == 0 ? OldPlayerOneState : OldPlayerTwoState;
            var currpadState = player == 0 ? CurrPlayerOneState : CurrPlayerTwoState;
            foreach (var button in buttons)
            {
                if (currpadState.IsButtonDown(button)
                    && oldpadState.IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonReleased(int player, params Buttons[] buttons)
        {
            var oldpadState = player == 0 ? OldPlayerOneState : OldPlayerTwoState;
            var currpadState = player == 0 ? CurrPlayerOneState : CurrPlayerTwoState;
            foreach (var button in buttons)
            {
                if (currpadState.IsButtonUp(button)
                    && oldpadState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        public MenuManager(ContentManager content, GraphicsDevice graphicsDevice,
            GraphicsDeviceManager graphics, GameManager gameManager, EditorManager editorManager, GreatEscape game)

        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _gameManager = gameManager;
            _editorManager = editorManager;
            _popOver = null;
            _theGame = game;

            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            string[] files = isf.GetFileNames("Levels/*");
            _allLevels = new List<String>();
            foreach (String file in files)
            {
                if (file.ToLower().Contains(TemplateName.ToLower()+".json"))
                {
                    continue;
                }
                _allLevels.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }

            Texture2D selector = _content.Load<Texture2D>("Sprites/Tools/Pickaxe");
            screenStack = new List<Screen>();
            // create the screens
            _currentScreen = _mainMenu = new SelectionMenu(
                "Main Menu", _content.Load<Texture2D>("Sprites/Menus/MenuImage"), selector, false, _graphicsDevice, this);
            screenStack.Add(_currentScreen);
            string firstLvl = System.IO.Path.GetFileNameWithoutExtension(files[0]);
            _currentLevel = firstLvl;
            _currentLevelIdx = 0;
            _mainMenu.AddSelection("play", Action.StartStory, "Levels/" + _currentLevel, new Rectangle(71, 313, 458, 101));
            _mainMenu.AddSelection("choose level", Action.ShowLevelSelector, null, new Rectangle(82, 434, 471, 112));
            _mainMenu.AddSelection("level editor", Action.ShowLevelEditor, "Levels/" + TemplateName, new Rectangle(220, 530, 349, 102));
            _mainMenu.AddSelection("exit game", Action.ExitGame, null, new Rectangle(152, 636, 185, 94));

            _gameHelps = new PopOverMenu("Controls", _content.Load<Texture2D>("Sprites/ScreenOverlays/PlayControls"), selector, false, _graphicsDevice, this);
            _editorHelp = new PopOverMenu("Controls", _content.Load<Texture2D>("Sprites/ScreenOverlays/EditorController"), selector, false, _graphicsDevice, this);
            _levelSelector = new SelectionMenu(
                "Select a Level", _content.Load<Texture2D>("Sprites/Menus/LevelSelector"), selector, true, _graphicsDevice, this);

            foreach (string file in _allLevels)
            {
                _levelSelector.AddSelection(file, Action.StartGame, 
                    "Levels/" + file, new Rectangle(0,0,0,0));
            }

            _gameOver = new PopOverMenu(
                "Game Over", _content.Load<Texture2D>("Sprites/Menus/GameOver"),
                selector, false, _graphicsDevice, this);
            _gameOver.AddSelection("Retry", Action.StartGame, "", 
                new Rectangle(166, 157, 549, 193));
            _gameOver.AddSelection("Select Level", Action.ShowLevelSelector, 
                null, new Rectangle(182, 328, 561, 133));
            _gameOver.AddSelection("Main Menu", Action.ShowMainMenu, "", 
                new Rectangle(205, 518, 551, 165));

            _levelCompleted = new PopOverMenu(
                "Level Completed!", _content.Load<Texture2D>("Sprites/Menus/LevelComplete"), selector, false, _graphicsDevice, this);
            _levelCompleted.AddSelection("Next Level", Action.Advance, "", new Rectangle(184, 145, 522, 166));
            _levelCompleted.AddSelection("Replay Level", Action.StartGame, "", new Rectangle(159, 313, 617, 196));
            _levelCompleted.AddSelection("Main Menu", Action.ShowMainMenu, "", new Rectangle(197, 512, 539, 170));

            _pauseGame = new PopOverMenu(
                "Game Paused", _content.Load<Texture2D>("Sprites/Menus/GamePaused"), selector, false, _graphicsDevice, this);
            _pauseGame.AddSelection("Resume Game", Action.ResumeGame, "", new Rectangle(171, 194, 684, 167));
            _pauseGame.AddSelection("Restart Level", Action.StartGame, "", new Rectangle(171, 371, 687, 171));
            _pauseGame.AddSelection("Main Menu", Action.ShowMainMenu, "", new Rectangle(204, 537, 544, 149));

            _editorMenu = new PopOverMenu("Menu",
                _content.Load<Texture2D>("Sprites/Backgrounds/Level2Background"),
                selector, true, _graphicsDevice, this);

            _editorMenu.AddSelection("Continue", Action.Back, "", new Rectangle(0,0,0,0));
            _editorMenu.AddSelection("Play", Action.ToggleEdit, "", new Rectangle(0, 0, 0, 0));
            _editorMenu.AddSelection("Save Level", Action.SaveLevel, TemplateName, new Rectangle());
            _editorMenu.AddSelection("Main Menu", Action.ShowMainMenu, "", new Rectangle(0,0,0,0));

            _loading = new LoadingScreen(_graphicsDevice, this);

            _game = new GameScreen(_gameManager, _graphicsDevice, this);

            _editor = new EditorScreen(_editorManager, _graphicsDevice, this);

            _story = new StoryScreen(_graphicsDevice, this);

            OldKeyboardState = CurrKeyboardState;
            CurrKeyboardState = Keyboard.GetState();
            CurrPlayerOneState = GamePad.GetState(PlayerIndex.One);
            CurrPlayerTwoState = GamePad.GetState(PlayerIndex.Two);
        }

        public void CallAction(Action action, object value)
        {
            _popOver = null;
            Selection retry, nextLvl;
            switch (action)
            {
                case Action.StartStory:
                    _story.LoadStory();
                    _currentScreen = _story;
                    break;
                case Action.StartGame:
                    string rawLvl = (value as string).Replace("Levels/", "");
                    _currentLevelIdx = _allLevels.IndexOf(rawLvl);
                    _currentScreen = _loading;
                    _game.LoadGame((string)value);
                    _currentScreen = _game;
                    _prevScreen = null;
                    screenStack.Add(_currentScreen);
                    break;
                case Action.ShowLevelSelector:
                    _prevScreen = _currentScreen;
                    if ((_prevScreen is GameScreen))
                    {
                        _prevScreen = null;
                    }
                    if (_gameManager != null && _gameManager.GameEngine != null)
                    {
                        _gameManager.GameEngine.GameState = null;
                    }
                    _currentScreen = _levelSelector;
                    screenStack.Add(_currentScreen);
                    break;
                case Action.ShowMainMenu:
                    _prevScreen = _currentScreen;
                    if (_gameManager != null && _gameManager.GameEngine != null)
                    {
                        _gameManager.GameEngine.GameState = null;
                    }
                    _currentScreen = _mainMenu;
                    screenStack.Clear();
                    screenStack.Add(_currentScreen);
                    break;

                case Action.ShowLevelEditor:
                    _editor.LoadGame((string)value);
                    _currentScreen = _editor;
                    screenStack.Add(_currentScreen);
                    break;

                case Action.ShowGameOverScreen:
                    _prevScreen = null;
                    _popOver = _gameOver;
                    retry = _popOver.GetSelection(0);
                    retry.Value = "Levels/" + _allLevels[_currentLevelIdx];
                    _popOver.SetSelection(0, retry);
                    _currentScreen = _popOver;
                    break;

                case Action.ShowLevelCompletedScreen:
                    _prevScreen = null;
                    _popOver = _levelCompleted;
                    nextLvl = _popOver.GetSelection(0);

                    // Get next level from previous level
                    int nextIdx = (_currentLevelIdx + 1) % _allLevels.Count;
                    nextLvl.Value = "Levels/" + _allLevels[nextIdx];
                    _popOver.SetSelection(0, nextLvl);

                    retry = _popOver.GetSelection(1);
                    retry.Value = "Levels/" + _allLevels[_currentLevelIdx];
                    _popOver.SetSelection(1, retry);

                    _currentScreen = _popOver;
                    break;
                case Action.ShowPauseMenu:
                    _prevScreen = _currentScreen;
                    if (_currentScreen is EditorScreen)
                    {
                        _popOver = _editorMenu;
                        var toggleOption = (_popOver as SelectionMenu)?.GetSelection(1);
                        if (_editorManager.Editing)
                        {
                            _editorManager._camera.SetCameraToRectangle(new Rectangle(0, 0, 2000, 2000));
                            toggleOption.Text = "Play Level";
                        }
                        else
                        {
                            toggleOption.Text = "Edit Level";
                        }
                    (_popOver as SelectionMenu).SetSelection(1, toggleOption);
                    }
                    else
                    {
                        _popOver = _pauseGame;
                        retry = _popOver.GetSelection(1);
                        retry.Value = "Levels/" + _allLevels[_currentLevelIdx];
                        _popOver.SetSelection(1, retry);

                    }

                    _currentScreen = _popOver;
                    screenStack.Add(_currentScreen);
                    break;

                case Action.ResumeGame:
                    CallAction(Action.Back, value);

                    break;
                case Action.Back:
                    if (screenStack.Count > 1)
                    {
                        screenStack.RemoveAt(screenStack.Count - 1);
                        _currentScreen = screenStack[screenStack.Count - 1];
                    }
                    break;
                case Action.Advance:
                    _currentLevelIdx = (++_currentLevelIdx) % _allLevels.Count;
                    //value = "Levels/" + _allLevels[_currentLevelIdx];
                    CallAction(Action.StartGame, value);
                    break;
                case Action.ExitGame:
                    _theGame.Exit();
                    break;
                case Action.ShowHelp:
                    if (_currentScreen == _gameHelps
                        || _currentScreen == _editorHelp)
                    {
                        CallAction(Action.Back, value);
                        break;
                    }
                    _prevScreen = _currentScreen;
                    if (_prevScreen == _editorMenu)
                    {
                        _currentScreen = _editorHelp;
                    }
                    else if (_prevScreen == _pauseGame)
                    {
                        _currentScreen = _gameHelps;
                    }

                    screenStack.Add(_currentScreen);
                    break;

                case Action.ToggleEdit:
                    _editorManager.Editing = !_editorManager.Editing;
                    if (_editorManager.Editing)
                    {
                        _editorManager._camera.SetCameraToRectangle(new Rectangle(0, 0, 2000, 2000));   
                    }
                    CallAction(Action.Back, value);
                    break;

                case Action.SaveLevel:
                    if(_prevScreen == _editor)
                    {
                        int numTemplates = 1;
                        foreach(var lvlname in _allLevels) {
                            if (lvlname.ToLower().Contains(TemplateName.ToLower()))
                            {
                                ++numTemplates;
                            }
                        }
                        string currFileName = value.ToString() + numTemplates.ToString();
                        _editorManager._editorController.HandleSave(currFileName);
                        _allLevels.Add(currFileName);
                        _levelSelector.AddSelection(currFileName, Action.StartGame,
                            "Levels/" + currFileName, new Rectangle(0, 0, 0, 0));
                        _requestSave = true;
                    }
                    break;
                default:
                    break;
            }
        }

        // Load an unload the content specific to the Menu
        public void LoadContent()
        {
            MenuFont = _content.Load<SpriteFont>("Fonts/Orbitron");
            MenuSong = _content.Load<Song>("Sounds/menu");
            IngameSong = _content.Load<Song>("Sounds/ingame");
            StorySong = _content.Load<Song>("Sounds/story_song");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(MenuManager.MenuSong);
        }

        public ContentManager ContentLoader()
        {
            return _content;
        }

        public void UnloadContent()
        {
        }


        public void Update(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);

            OldKeyboardState = CurrKeyboardState;
            OldPlayerOneState = CurrPlayerOneState;
            OldPlayerTwoState = CurrPlayerTwoState;

            CurrKeyboardState = Keyboard.GetState();
            CurrPlayerOneState = GamePad.GetState(PlayerIndex.One);
            CurrPlayerTwoState = GamePad.GetState(PlayerIndex.Two);
            if (_gameManager?.GameEngine?.GameState != null
                && _gameManager.GameEngine.GameState.Completed)
            {

                CallAction(MenuManager.Action.ShowLevelCompletedScreen, null);
            }
            if (_gameManager?.GameEngine?.GameState != null
                && _gameManager.GameEngine.GameState.Mode == GameState.State.GameOver)
            {
                CallAction(Action.ShowGameOverScreen, null);
            }
        }

        public void Draw(GameTime gameTime, int width, int height)
        {
            _currentScreen.Draw(gameTime, width, height);
        }
    }

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
            MediaPlayer.Stop();
            MediaPlayer.Play(MenuManager.IngameSong);

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

            MediaPlayer.Stop();
            MediaPlayer.Play(MenuManager.StorySong);
        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            Rectangle dest = new Rectangle(0, 0, _graphicsDevice.PresentationParameters.BackBufferWidth
                , _graphicsDevice.PresentationParameters.BackBufferHeight);
            //Rectangle dest = new Rectangle(0, 0, _currentSlide.Width, _currentSlide.Height);
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

    class LoadingScreen : Screen
    {
        SpriteBatch _spriteBatch;
        public LoadingScreen(GraphicsDevice graphicsDevice, MenuManager manager) : base(graphicsDevice, manager)
        {
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _graphicsDevice.Clear(Color.Red);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_manager.MenuFont, "loading...", new Vector2(50f, 50f), Color.White, 0f, new Vector2(), 0.5f, new SpriteEffects(), 0f);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
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
                else if(_currentPosition >= _selections.Count - half)
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
                if(_currentPosition < half)
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
                    position + origin*scale, //position
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
        /*
        public override void Draw(GameTime gameTime, int width, int height)
        {
            _graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            _spriteBatch.DrawString(_manager.MenuFont, _title,
                new Vector2(50f, 50f), Color.White, 0f, new Vector2(), 0.5f,
                new SpriteEffects(), 0f);
            for(var i = 0; i < _selections.Count; i++)
            {
                _spriteBatch.DrawString(_manager.MenuFont, _selections[i].Text,
                    new Vector2(i == _currentPosition ? 100 : 50,
                    (i * 100.0f) + 150),
                    Color.White, 0f, new Vector2(),
                    i == _currentPosition ? 1f : 0.75f, new SpriteEffects(), 0f);
            }
            _spriteBatch.End();
            base.Draw(gameTime, (int)(width * _ratioX), (int)(height * _ratioY));
        }*/
    }
}
