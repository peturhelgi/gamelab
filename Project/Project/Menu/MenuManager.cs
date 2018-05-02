

using EditorLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic;
using System;
using System.Collections.Generic;

namespace TheGreatEscape.Menu
{
    class MenuManager
    {
        Screen _currentScreen;
        Screen _prevScreen;
        SelectionMenu _mainMenu;
        SelectionMenu _levelSelector;
        PopOverMenu _popOver;
        PopOverMenu _gameOver;
        PopOverMenu _levelCompleted;
        PopOverMenu _pauseGame;
        GameScreen _game;
        LoadingScreen _loading;
        Screen _gameMenu;
        EditorScreen _editor;

        public enum Action
        {
            StartGame,
            ShowMainMenu,
            ShowOptionsMenu,
            ShowLevelSelector,
            ShowLevelCompletedScreen,
            ShowGameOverScreen,
            ShowPauseMenu,
            ResumeGame,
            ShowLevelEditor
        };

        GameManager _gameManager;
        EditorManager _editorManager;
        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;

        public KeyboardState OldKeyboardState;
        public GamePadState OldPlayerOneState;
        public GamePadState OldPlayerTwoState;
        // TODO: move to renderer
        // Assets for the Menu
        public SpriteFont MenuFont;



        public MenuManager(ContentManager content, GraphicsDevice graphicsDevice,
            GraphicsDeviceManager graphics, GameManager gameManager, EditorManager editorManager)

        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _gameManager = gameManager;
            _editorManager = editorManager;
            _popOver = null;

            // create the screens
            _currentScreen = _mainMenu = new SelectionMenu(
                "Main Menu", _graphicsDevice, this);
            _mainMenu.AddSelection("play", Action.StartGame, "Level_1");
            _mainMenu.AddSelection("choose level", Action.ShowLevelSelector, null);
            _mainMenu.AddSelection("level editor", Action.ShowLevelEditor, "Level_1");


            _levelSelector = new SelectionMenu(
                "Select a Level", _graphicsDevice, this);

            _levelSelector.AddSelection("Level 1", Action.StartGame, "Level_1");
            _levelSelector.AddSelection(
                "wide_level", Action.StartGame, "wide_level");
            _levelSelector.AddSelection(
                "more_platforms", Action.StartGame, "more_platforms");
            _levelSelector.AddSelection("samplelvl", Action.StartGame, "samplelvl");
            _gameOver = new PopOverMenu(
                "Game Over", _graphicsDevice, this);

            _gameOver.AddSelection("Retry", Action.StartGame, "");
            _gameOver.AddSelection("Main Menu", Action.ShowMainMenu, "");

            _levelCompleted = new PopOverMenu(
                "Level Completed!", _graphicsDevice, this);
            _levelCompleted.AddSelection("Next Level", Action.StartGame, "");
            _levelCompleted.AddSelection("Replay Level", Action.StartGame, "");
            _levelCompleted.AddSelection("Main Menu", Action.ShowMainMenu, "");

            _pauseGame = new PopOverMenu(
                "Game Paused", _graphicsDevice, this);
            _pauseGame.AddSelection("Resume Game", Action.ResumeGame, "");
            _pauseGame.AddSelection("Restart Level", Action.StartGame, "");
            _pauseGame.AddSelection("Main Menu", Action.ShowMainMenu, "");
            
            _loading = new LoadingScreen(_graphicsDevice, this);

            _game = new GameScreen(_gameManager, _graphicsDevice, this);

            _editor = new EditorScreen(_editorManager, _graphicsDevice, this);

            OldKeyboardState = Keyboard.GetState();
            OldPlayerOneState = GamePad.GetState(PlayerIndex.One);
            OldPlayerTwoState = GamePad.GetState(PlayerIndex.Two);
        }

        public void CallAction(Action action, object value)
        {
            _popOver = null;
            Selection retry, nextLvl;
            switch(action)
            {
                case Action.StartGame:
                    _currentScreen = _loading;
                    _game.LoadGame((string)value);
                    _currentScreen = _game;
                    break;
                case Action.ShowLevelSelector:
                    _currentScreen = _levelSelector;
                    break;
                case Action.ShowMainMenu:
                    _currentScreen = _mainMenu;
                    break;

                case Action.ShowLevelEditor:
                    _editor.LoadGame((string)value);
                    _currentScreen = _editor;
                    break;

                case Action.ShowGameOverScreen:
                    _popOver = _gameOver;
                    retry = _popOver.GetSelection(0);
                    retry.Value = _levelSelector?.GetLastSelection()?.Value;
                    if(retry.Value == null)
                    {
                        retry.Value = _mainMenu.GetSelection(0).Value;
                    }
                    _popOver.SetSelection(0, retry);
                    _currentScreen = _popOver;
                    break;

                case Action.ShowLevelCompletedScreen:
                    _popOver = _levelCompleted;
                    nextLvl = _popOver.GetSelection(0);

                    // TODO: Get next level from previous level
                    nextLvl.Value = "more_platforms";
                    _popOver.SetSelection(0, nextLvl);

                    retry = _popOver.GetSelection(1);
                    retry.Value = _levelSelector?.GetLastSelection()?.Value;
                    if(retry.Value == null)
                    {
                        retry.Value = _mainMenu.GetSelection(0).Value;
                    }
                    _popOver.SetSelection(1, retry);

                    _currentScreen = _popOver;
                    break;
                case Action.ShowPauseMenu:
                    _prevScreen = _currentScreen;
                    _popOver = _pauseGame;

                    retry = _popOver.GetSelection(1);
                    retry.Value = _levelSelector?.GetLastSelection()?.Value;
                    if(retry.Value == null)
                    {
                        retry.Value = _mainMenu.GetSelection(0).Value;
                    }
                    _popOver.SetSelection(1, retry);

                    _currentScreen = _popOver;
                    break;

                case Action.ResumeGame:
                    if(_prevScreen != null)
                    {
                        _currentScreen = _prevScreen;
                        _prevScreen = null;
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

        }

        public void UnloadContent()
        {
        }


        public void Update(GameTime gameTime)
        {
            _currentScreen.Update(gameTime);
            OldKeyboardState = Keyboard.GetState();
            OldPlayerOneState = GamePad.GetState(PlayerIndex.One);
            OldPlayerTwoState = GamePad.GetState(PlayerIndex.Two);
            if(_gameManager?.GameEngine?.GameState != null
                && _gameManager.GameEngine.GameState.Completed) {
                CallAction(MenuManager.Action.ShowLevelCompletedScreen, null);
            }
        }

        public void Draw(GameTime gameTime, int width, int height)
        {
            _currentScreen.Draw(gameTime, width, height);
        }
    }

    abstract class Screen
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
        public Selection(string text, MenuManager.Action action, object value)
        {
            Text = text;
            Action = action;
            Value = value;
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
        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _gameManager.Draw(gameTime, width, height);
        }

        public override void Update(GameTime gameTime)
        {

            if ((Keyboard.GetState().IsKeyDown(Keys.Escape) && _manager.OldKeyboardState.IsKeyUp(Keys.Escape)) ||
                (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) && _manager.OldPlayerOneState.IsButtonUp(Buttons.Start)) ||
                (GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.Start) && _manager.OldPlayerTwoState.IsButtonUp(Buttons.Start)))
            {
                _manager.CallAction(MenuManager.Action.ShowMainMenu, null);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                _manager.CallAction(MenuManager.Action.ShowGameOverScreen, null);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                _manager.CallAction(MenuManager.Action.ShowLevelCompletedScreen, null);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space)
                && _manager.OldKeyboardState.IsKeyUp(Keys.Space))
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
            if ((Keyboard.GetState().IsKeyDown(Keys.Escape) && _manager.OldKeyboardState.IsKeyUp(Keys.Escape)) || 
                (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start) && _manager.OldPlayerOneState.IsButtonUp(Buttons.Start)) || 
                (GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.Start) && _manager.OldPlayerTwoState.IsButtonUp(Buttons.Start)))
            {
                _manager.CallAction(MenuManager.Action.ShowMainMenu, null);
            }
            else
            {
                _editorManager.Update(gameTime);
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


        public SelectionMenu(string title, GraphicsDevice graphicsDevice,
            MenuManager manager) : base(graphicsDevice, manager)
        {
            _selections = new List<Selection>();
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _title = title;
        }

        public void AddSelection(string text, MenuManager.Action action, object value)
        {
            _selections.Add(new Selection(text, action, value));
        }

        public void AddSelection(Selection selection)
        {
            if(selection != null)
            {
                _selections.Add(selection);
            }
        }

        public void SetSelection(int index, Selection selection)
        {
            if(selection == null)
            {
                return;
            }

            if(_selections.IndexOf(selection) == -1)
            {
                if(index < _selections.Count)
                {
                    _selections[index] = selection;
                } else
                {
                    AddSelection(selection);
                }
            }
        }

        public Selection GetSelection(int index)
        {
            if(_selections.Count > 0)
            {
                return _selections[index % _selections.Count];
            }
            return null;
        }
        public Selection GetLastSelection()
        {
            if(_lastSelection != -1 && _selections.Count > 0)
            {
                return _selections[_lastSelection];
            }
            return null;
        }

        public override void Update(GameTime gameTime)
        {
            // TODO add support for (multiple) Controllers
            // Keyboard controls
            if(Keyboard.GetState().IsKeyDown(Keys.Down) && _manager.OldKeyboardState.IsKeyUp(Keys.Down))
            {
                _currentPosition = (++_currentPosition) %_selections.Count;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && _manager.OldKeyboardState.IsKeyUp(Keys.Up))
            {
                _currentPosition = --_currentPosition < 0 ? _selections.Count - 1 : _currentPosition;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _manager.OldKeyboardState.IsKeyUp(Keys.Enter) ||
                Keyboard.GetState().IsKeyDown(Keys.Space) && _manager.OldKeyboardState.IsKeyUp(Keys.Space))
            {
                _manager.CallAction(_selections[_currentPosition].Action, _selections[_currentPosition].Value);
                _lastSelection = _currentPosition;
                _currentPosition = 0;
            }

            // Xbox controls for player one
            if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown)
                && _manager.OldPlayerOneState.IsButtonUp(Buttons.LeftThumbstickDown))
            {
                _currentPosition = (++_currentPosition) % _selections.Count;
            }
            if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickUp)
                && _manager.OldPlayerOneState.IsButtonUp(Buttons.LeftThumbstickUp))
            {
                _currentPosition = --_currentPosition < 0 ? _selections.Count - 1 : _currentPosition;
            }
            if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown)
                && _manager.OldPlayerOneState.IsButtonUp(Buttons.DPadDown))
            {
                _currentPosition = (++_currentPosition) % _selections.Count;
            }
            if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp)
                && _manager.OldPlayerOneState.IsButtonUp(Buttons.DPadUp))
            {
                _currentPosition = --_currentPosition < 0 ? _selections.Count - 1 : _currentPosition;
            }

            if(GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)
                && _manager.OldPlayerOneState.IsButtonUp(Buttons.A))
            {
                _manager.CallAction(_selections[_currentPosition].Action, _selections[_currentPosition].Value);
                _currentPosition = 0;
            }
        }

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
        }

    }

    class PopOverMenu : SelectionMenu {
        float _ratioX;
        float _ratioY;
        float _alpha;
        public PopOverMenu(string title, GraphicsDevice graphicsDevice,
            MenuManager manager, float ratioX = 1.0f, float ratioY = 1.0f,
            float alpha = 1.0f) : 
            base(title, graphicsDevice, manager)
        {
            _ratioX = ratioX;
            _ratioY = ratioY;
            _alpha = alpha;
        }

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
        }
    }
}
