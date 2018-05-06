﻿using System;
using System.Collections.Generic;
using System.IO;
using EditorLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic;

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
            ShowLevelEditor,
            ExitGame
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

        public KeyboardState OldKeyboardState;
        public GamePadState OldPlayerOneState;
        public GamePadState OldPlayerTwoState;
        // TODO: move to renderer
        // Assets for the Menu
        public SpriteFont MenuFont;



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

            string[] files = Directory.GetFiles("Content\\Levels");
            _allLevels = new List<String>();
            foreach (String file in files)
            {
                _allLevels.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }

            Texture2D selector = _content.Load<Texture2D>("Sprites/Tools/Pickaxe");

            // create the screens
            _currentScreen = _mainMenu = new SelectionMenu(
                "Main Menu", _content.Load<Texture2D>("Sprites/Menus/MenuImage"), selector, false, _graphicsDevice, this);
            string firstLvl = System.IO.Path.GetFileNameWithoutExtension(files[0]);
            _currentLevel = firstLvl;
            _currentLevelIdx = 0;
            _mainMenu.AddSelection("play", Action.StartGame, "Levels/" + _currentLevel, new Rectangle(71, 313, 458, 101));
            _mainMenu.AddSelection("choose level", Action.ShowLevelSelector, null, new Rectangle(82, 434, 471, 112));
            _mainMenu.AddSelection("level editor", Action.ShowLevelEditor, "Levels/" + _currentLevel, new Rectangle(120, 530, 349, 102));
            _mainMenu.AddSelection("exit game", Action.ExitGame, null, new Rectangle(152, 636, 185, 94));


            _levelSelector = new SelectionMenu(
                "Select a Level", _content.Load<Texture2D>("Sprites/Menus/LevelSelector"), selector, true, _graphicsDevice, this);

            
            for (int i = 0; i < files.Length; i++)
            {
                string file = System.IO.Path.GetFileNameWithoutExtension(files[i]);
                int size = file.Length;
                _levelSelector.AddSelection(file, Action.StartGame, "Levels/" + file, new Rectangle(120, 100+120*i, 35*size, 100));
            }

            _gameOver = new PopOverMenu(
                "Game Over", _content.Load<Texture2D>("Sprites/Menus/GameOver"), selector, false, _graphicsDevice, this);
            _gameOver.AddSelection("Retry", Action.StartGame, "", new Rectangle(166, 157, 549, 193));
            _gameOver.AddSelection("Select Level", Action.ShowLevelSelector, null, new Rectangle(182, 328, 561, 133));
            _gameOver.AddSelection("Main Menu", Action.ShowMainMenu, "", new Rectangle(205, 518, 551, 165));

            _levelCompleted = new PopOverMenu(
                "Level Completed!", _content.Load<Texture2D>("Sprites/Menus/LevelComplete"), selector, false, _graphicsDevice, this);
            _levelCompleted.AddSelection("Next Level", Action.StartGame, "", new Rectangle(184, 145, 522, 166));
            _levelCompleted.AddSelection("Replay Level", Action.StartGame, "", new Rectangle(159, 313, 617, 196));
            _levelCompleted.AddSelection("Main Menu", Action.ShowMainMenu, "", new Rectangle(197, 512, 539, 170));

            _pauseGame = new PopOverMenu(
                "Game Paused", _content.Load<Texture2D>("Sprites/Menus/GamePaused"), selector, false, _graphicsDevice, this);
            _pauseGame.AddSelection("Resume Game", Action.ResumeGame, "", new Rectangle(171, 194, 684, 167));
            _pauseGame.AddSelection("Restart Level", Action.StartGame, "", new Rectangle(171, 371, 687, 171));
            _pauseGame.AddSelection("Main Menu", Action.ShowMainMenu, "", new Rectangle(204, 537, 544, 149));
            
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
                    if (_gameManager != null && _gameManager.GameEngine != null)
                    {
                        _gameManager.GameEngine.GameState = null;
                    }
                    _currentScreen = _levelSelector;
                    break;
                case Action.ShowMainMenu:
                    if (_gameManager != null && _gameManager.GameEngine != null)
                    {
                        _gameManager.GameEngine.GameState = null;
                    }
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

                    // Get next level from previous level
                    int nextIdx = (_currentLevelIdx + 1) % _allLevels.Count;
                    nextLvl.Value = _allLevels[nextIdx];
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
                case Action.ExitGame:
                    _theGame.Exit();
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
            if(_gameManager?.GameEngine?.GameState != null
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
        public Rectangle Position;
        public Selection(string text, MenuManager.Action action, object value, Rectangle position)
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
                _manager.CallAction(MenuManager.Action.ShowPauseMenu, null);
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
            _spriteBatch.Draw(_background, new Rectangle(0, 0, width, height), Color.White);

            float stretchX = (float)width/1920f;
            float stretchY = (float)height/1080f;
            Selection sel = _selections[_currentPosition];

            if(!_isDynamic)
            {
                _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)((float)sel.Position.Top * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
            }
            //Draw some level names
            else
            {
                int[] levelsToDraw;
                if (_currentPosition == 0 || _currentPosition == 1 || _currentPosition == 2)
                {
                    levelsToDraw = new int[] { 0, 1, 2, 3, 4, 5, 6 };
                }
                else if (_currentPosition == _selections.Count-1 || _currentPosition == _selections.Count-2 || _currentPosition == _selections.Count - 3)
                {
                    int i = _selections.Count;
                    levelsToDraw = new int[] {i - 7, i - 6, i - 5, i - 4, i - 3, i - 2, i - 1 };
                }
                else
                {
                    int i = _currentPosition;
                    levelsToDraw = new int[] {i - 3, i - 2, i - 1, i, i + 1, i + 2, i + 3};
                }
                
                for (var i = 0; i < 7; i++)
                {
                    int idx = levelsToDraw[i];
                    if (idx < _selections.Count) //If level exists
                    {
                        _spriteBatch.DrawString(_manager.MenuFont, _selections[idx].Text,
                        new Vector2((int)((float)_selections[idx].Position.Left * stretchX),
                        (int)((float)(_selections[i].Position.Top - 20)*stretchY)),
                        Color.White, 0f, new Vector2(),
                        stretchY, new SpriteEffects(), 0f);
                    }
                }

                sel = _selections[_currentPosition];
                int total = _selections.Count;
                if (_currentPosition == 0)
                {
                    _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)(100f * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
                }
                else if (_currentPosition == 1)
                {
                    _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)(220f * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
                }
                else if (_currentPosition == 2)
                {
                    _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)(340f * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
                }
                else if (_currentPosition == total - 1)
                {
                    _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)(820f * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
                }
                else if (_currentPosition == total - 2)
                {
                    _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)(700f * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
                }
                else if (_currentPosition == total - 3)
                {
                    _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)(580f * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_selector, new Rectangle((int)((float)sel.Position.Right * stretchX), (int)(460f * stretchY), (int)(140f * stretchX), (int)(100f * stretchY)), Color.White);
                }
                
                
            }

            _spriteBatch.End();
        }

    }

    class PopOverMenu : SelectionMenu {
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
