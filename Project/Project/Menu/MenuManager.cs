

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.GameLogic;
using System;
using System.Collections.Generic;

namespace Project.Menu
{
    class MenuManager
    {
        Screen _currentScreen;
        SelectionMenu _mainMenu;
        SelectionMenu _levelSelector;
        GameScreen _game;
        LoadingScreen _loading;
        Screen _gameMenu;

        public enum Action { StartGame, ShowMainMenu, ShowOptionsMenu, ShowLevelSelector };
        GameManager _gameManager;
        GraphicsDeviceManager _graphics;
        GraphicsDevice _graphicsDevice;
        ContentManager _content;

        public KeyboardState OldKeyboardState;
        // TODO: move to renderer
        // Assets for the Menu
        public SpriteFont MenuFont;


        public MenuManager(ContentManager content, GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, GameManager gameManager)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _gameManager = gameManager;

            // create the screens
            _currentScreen = _mainMenu = new SelectionMenu("Main Menu", _graphicsDevice, this);
            _mainMenu.AddSelection("play", Action.StartGame, "more_platforms");
            _mainMenu.AddSelection("choose level", Action.ShowLevelSelector, null);

            _levelSelector = new SelectionMenu("Select a Level", _graphicsDevice, this);
            _levelSelector.AddSelection("more_platforms", Action.StartGame, "more_platforms");
            _levelSelector.AddSelection("samplelvl", Action.StartGame, "samplelvl");

            _loading = new LoadingScreen(_graphicsDevice, this);

            _game = new GameScreen(_gameManager, _graphicsDevice, this);

            OldKeyboardState = Keyboard.GetState();
        }

        public void CallAction(Action action, object value) {
            switch (action) {
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
        }

        public void Draw(GameTime gameTime, int width, int height)
        {
            _currentScreen.Draw(gameTime, width, height);
        }
    }

    abstract class Screen {
        protected MenuManager _manager;
        protected GraphicsDevice _graphicsDevice;

        public Screen(GraphicsDevice graphicsDevice, MenuManager manager) {
            _manager = manager;
            _graphicsDevice = graphicsDevice;
        }

        abstract public void Update(GameTime gameTime);
        abstract public void Draw(GameTime gameTime, int widht, int height);

    }

    class Selection {
        public string Text;
        public MenuManager.Action Action;
        public object Value;
        public Selection(string text, MenuManager.Action action, object value) {
            Text = text;
            Action = action;
            Value = value;
        }
    }

    class GameScreen : Screen {
        GameManager _gameManager;
        public GameScreen(GameManager gameManager, GraphicsDevice graphicsDevice, MenuManager manager) : base(graphicsDevice, manager)
        {
            _gameManager = gameManager;
        }

        public void LoadGame(String level) {
            _gameManager.UnloadContent();
            _gameManager.LoadLevel(level);
        }

        public override void Draw(GameTime gameTime, int width, int height)
        {
            _gameManager.Draw(gameTime, width, height);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && _manager.OldKeyboardState.IsKeyUp(Keys.Escape))
            {
                _manager.CallAction(MenuManager.Action.ShowMainMenu, null);
            }
            else
            {
                _gameManager.Update(gameTime);
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

    class SelectionMenu : Screen {

        List<Selection> _selections;
        int _currentPosition = 0;
        SpriteBatch _spriteBatch;
        String _title;


        public SelectionMenu(string title, GraphicsDevice graphicsDevice, MenuManager manager):base(graphicsDevice, manager) {
            _selections = new List<Selection>();
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _title = title;

        }

        public void AddSelection(string text, MenuManager.Action action, object value) {
            _selections.Add(new Selection(text, action, value));
        }

        public override void Update(GameTime gameTime) {
            // TODO add support for (multiple) Controllers
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && _manager.OldKeyboardState.IsKeyUp(Keys.Down))
            {
                _currentPosition += 1;
                _currentPosition %= _selections.Count;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && _manager.OldKeyboardState.IsKeyUp(Keys.Enter)||
                Keyboard.GetState().IsKeyDown(Keys.Space) && _manager.OldKeyboardState.IsKeyUp(Keys.Space))
            {
                _manager.CallAction(_selections[_currentPosition].Action, _selections[_currentPosition].Value);
                _currentPosition = 0;
            }
        }

        public override void Draw(GameTime gameTime, int width, int height) {
            _graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            _spriteBatch.DrawString(_manager.MenuFont, _title, new Vector2(50f, 50f), Color.White, 0f, new Vector2(), 0.5f, new SpriteEffects(), 0f);
            for (var i = 0; i < _selections.Count; i++)
            {
                _spriteBatch.DrawString(_manager.MenuFont, _selections[i].Text, new Vector2(i==_currentPosition?100:50, (i*100.0f)+150), Color.White, 0f, new Vector2(), i == _currentPosition ?1f:0.75f, new SpriteEffects(), 0f);
            }
            _spriteBatch.End();
        }

    }
}
