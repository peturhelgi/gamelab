using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using EditorLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic;
using TheGreatEscape.Util;
using static TheGreatEscape.Menu.MenuManager;

namespace TheGreatEscape.Menu
{
    public class MenuManager
    {
        IAsyncResult result;
        bool _requestSave = false;

        public enum SoundToPlay
        {
            Ingame,
            Menu,
            GameOver,
            Story,
            LevelCompleted,
            Pickaxe,
            Key,
            Door,
            Dying
        }
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
            ToggleEdit,
            ShowCredits
        };

        // Menus
        PopOverMenu _popOver;
        PopOverMenu _gameOver;
        PopOverMenu _levelCompleted;
        PopOverMenu _pauseGame;
        PopOverMenu _editorMenu;

        Screen _currentScreen;
        Screen _prevScreen;

        SelectionMenu _mainMenu;
        SelectionMenu _levelSelector;

        // Main Screens
        StoryScreen _storyScreen;
        GameScreen _gameScreen;
        LoadingScreen _loadingScreen;
        EditorScreen _editorScreen;
        // Secondary Screens
        HelpScreen _editorHelp;
        HelpScreen _gameHelp;
        StoryScreen _story;
        CreditsScreen _credits;

        public static SoundPlayer SoundsPlayer;

        // Managers
        GameManager _gameManager;
        EditorManager _editorManager;
        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;
        public InputManager Input;

        GreatEscape _theGame;

        int _currentLevelIdx;
        List<String> _allLevels;
        List<Screen> screenStack;
        String _currentLevel;
        String _templateName = "Template";
        public SpriteFont MenuFont;


        public void PlaySound(SoundToPlay sound)
        {
            SoundsPlayer.Play(sound);
        }

        public MenuManager(ContentManager content, GraphicsDevice graphicsDevice,
            GraphicsDeviceManager graphics, GameManager gameManager, 
            EditorManager editorManager, GreatEscape game)

        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _gameManager = gameManager;
            _editorManager = editorManager;
            _popOver = null;
            _theGame = game;
            Input = new InputManager(1);
            SoundsPlayer = new SoundPlayer(_content);

            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            string[] files = isf.GetFileNames("Levels/*");
            _allLevels = new List<String>();
            foreach (String file in files)
            {
                if (file.ToLower().Contains(_templateName.ToLower()+".json"))
                {
                    continue;
                }
                _allLevels.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }

            Texture2D selector = _content.Load<Texture2D>("Sprites/Tools/Pickaxe");
            screenStack = new List<Screen>();

            // create the screens
            _currentScreen = _mainMenu = new SelectionMenu(
                "Main Menu", _content.Load<Texture2D>("Sprites/Menus/MenuImage"),
                selector, false, _graphicsDevice, this);
            _levelSelector = new SelectionMenu(
                "Select a Level", 
                _content.Load<Texture2D>("Sprites/Menus/LevelSelector"), 
                selector, true, _graphicsDevice, this);

            screenStack.Add(_currentScreen);
            string firstLvl = System.IO.Path.GetFileNameWithoutExtension(files[0]);
            _currentLevel = firstLvl;
            _currentLevelIdx = 0;
            _mainMenu.AddSelection("play", Action.StartStory, "Levels/" + _currentLevel, new Rectangle(71, 313, 458, 101));
            _mainMenu.AddSelection("choose level", Action.ShowLevelSelector, null, new Rectangle(82, 434, 471, 112));
            _mainMenu.AddSelection("level editor", Action.ShowLevelEditor, "Levels/" + _templateName, new Rectangle(220, 530, 349, 102));
            _mainMenu.AddSelection("exit game", Action.ExitGame, null, new Rectangle(152, 636, 185, 94));

            _gameHelp = new HelpScreen(graphicsDevice, this, _content.Load<Texture2D>("Sprites/ScreenOverlays/PlayControls"));
            _editorHelp = new HelpScreen(graphicsDevice, this, _content.Load<Texture2D>("Sprites/ScreenOverlays/EditorController"));
            _levelSelector = new SelectionMenu(
                "Select a Level", _content.Load<Texture2D>("Sprites/Menus/LevelSelector"), selector, true, _graphicsDevice, this);

            foreach (string file in _allLevels)
            {
                _levelSelector.AddSelection(file.Replace("_", " "), Action.StartGame, 
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
            _editorMenu.AddSelection("Save Level", Action.SaveLevel, _templateName, new Rectangle());
            _editorMenu.AddSelection("Main Menu", Action.ShowMainMenu, "", new Rectangle(0,0,0,0));

            _story = new StoryScreen(_graphicsDevice, this);
            _credits = new CreditsScreen(_graphicsDevice, this);

            _gameScreen = new GameScreen(_gameManager, _graphicsDevice, this);
            _editorScreen = new EditorScreen(_editorManager, _graphicsDevice, this);
            _storyScreen = new StoryScreen(_graphicsDevice, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="value"></param>
        public void CallAction(Action action, object value)
        {
            _popOver = null;
            Selection retry, nextLvl;
            Input.Reset();
            switch (action)
            {
                case Action.StartStory:
                    _storyScreen.LoadStory();
                    _currentScreen = _storyScreen;
                    break;
                case Action.ShowCredits:
                    _credits.LoadCredits();
                    _currentScreen = _credits;
                    _gameManager.GameEngine.GameState = null;
                    break;
                case Action.StartGame:
                    string rawLvl = (value as string).Replace("Levels/", "");
                    _currentLevelIdx = _allLevels.IndexOf(rawLvl);
                    _gameScreen.LoadGame((string)value);
                    _currentScreen = _gameScreen;
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
                    _editorScreen.LoadGame((string)value);
                    _currentScreen = _editorScreen;
                    screenStack.Add(_currentScreen);
                    break;

                case Action.ShowGameOverScreen:
                    PlaySound(SoundToPlay.GameOver);
                    _prevScreen = null;
                    _popOver = _gameOver;
                    retry = _popOver.GetSelection(0);
                    retry.Value = "Levels/" + _allLevels[_currentLevelIdx];
                    _popOver.SetSelection(0, retry);
                    _currentScreen = _popOver;
                    break;

                case Action.ShowLevelCompletedScreen:
                    if (_currentLevelIdx == 6)
                    {
                        CallAction(Action.ShowCredits, null);
                        break;
                    }
                    PlaySound(SoundToPlay.LevelCompleted);
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
                    CallAction(Action.StartGame, value);
                    break;
                case Action.ExitGame:
                    _theGame.Exit();
                    break;
                case Action.ShowHelp:
                    if (_currentScreen == _gameHelp
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
                        _currentScreen = _gameHelp;
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
                    if(_prevScreen == _editorScreen)
                    {
                        int numTemplates = 1;
                        foreach(var lvlname in _allLevels) {
                            if (lvlname.ToLower().Contains(_templateName.ToLower()))
                            {
                                ++numTemplates;
                            }
                        }
                        string currFileName = value.ToString() + numTemplates.ToString();
                        _editorManager._editorController.HandleSave(currFileName);
                        _allLevels.Add(currFileName);
                        _levelSelector.AddSelection(currFileName, Action.StartGame,
                            "Levels/" + currFileName, new Rectangle(0, 0, 0, 0));
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
            SoundsPlayer.LoadSongs();
            PlaySound(SoundToPlay.Menu);
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
            Input.Update();
            _currentScreen.Update(gameTime);

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
}
