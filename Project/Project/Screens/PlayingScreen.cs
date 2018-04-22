using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Project.GameObjects;
using Project.Controls;
using Project.Render;
using Project.GameStates;

namespace Project.Screens {
    public class PlayingScreen : GameScreen {
        public GameEngine GameEngine;
        public static string baseFolder = "Content/GamePlay/Levels/";

        Level level;

        public PlayingScreen(string path) {
            this.Path = path;
            controller = new PlayingControls(new Camera(0.8f, Vector2.Zero));
            Renderer = new GameRenderer();
        }

        public override void LoadContent() {
            base.LoadContent();
            GameState = new GamePlayState();
            DataManager<Level> levelLoader = new DataManager<Level>();

            level = levelLoader.Load(baseFolder + this.Path);
            level.Initialize(ScreenManager);
            level.LoadContent();

            GameState.LoadContent(level);

            GameEngine = new GameEngine();
            GameEngine.Initialize(GameState);

            controller.Initialize(GameEngine);

            Renderer.Initialize(ref GameState, ref controller.Camera);
        }

        public override void UnloadContent() {
            base.UnloadContent();
            level.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            GameEngine.Update(gameTime);
        }
    }
}
