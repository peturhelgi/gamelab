using System;
using System.Collections.Generic;

using System.IO;
using System.IO.IsolatedStorage;
using EditorLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic;
using TheGreatEscape.Menu;


namespace TheGreatEscape
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GreatEscape : Game
    {
        GameManager _gameManager;
        EditorManager _editorManager;
        MenuManager _menu;
        GraphicsDeviceManager _graphics;

        // Simple camera controls
        private Vector2 _viewCenter;
        private float _viewZoom;
        private Matrix view;
        private Vector2 cameraPosition;


        public GreatEscape()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
                IsFullScreen = false
            };
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";

            _transferLevels();
            _gameManager = new GameManager(Content, GraphicsDevice, _graphics);
            _editorManager = new EditorManager(Content, GraphicsDevice, _graphics);
            _menu = new MenuManager(Content, GraphicsDevice, _graphics, _gameManager, _editorManager, this);

            IsMouseVisible = true;
            base.Initialize();
        }


        //Writes all level files into a write access directory
        private void _transferLevels()
        {
            string levelDir = "Levels";
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isf.DirectoryExists(levelDir))
            {
                isf.CreateDirectory(levelDir);
            }
            string[] files = Directory.GetFiles("Content\\Levels");
            foreach (String file in files)
            {
                string level = File.ReadAllText(file);
                StreamWriter streamWriter = new StreamWriter(isf.CreateFile(levelDir + "/" + System.IO.Path.GetFileName(file)));
                streamWriter.Write(level);
                streamWriter.Dispose();
            }

        }

        protected override void LoadContent()
        {
            _menu.LoadContent();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {

            _menu.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _menu.Draw(
                gameTime,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight);
            base.Draw(gameTime);
        }
    }
}
