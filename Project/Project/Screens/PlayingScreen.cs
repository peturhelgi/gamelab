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


namespace Project.Screens {
    public class PlayingScreen : GameScreen {
        Level level;
        GameState state;
        public static string baseFolder = "Content/GamePlay/Levels/";

        public PlayingScreen(string path) {
            this.Path = path;
            controller = new PlayingControls(new Camera(0.8f, Vector2.Zero));
        }

        public override void LoadContent() {
            base.LoadContent();
            
            state = new GameState();
            DataManager<Level> levelLoader = new DataManager<Level>();

            level = levelLoader.Load(baseFolder + this.Path);
            level.LoadContent();
            state.LoadContent(ref level);
            GameEngine.Instance.Initialize(state);       
        }

        public override void UnloadContent() {
            base.UnloadContent();
            level.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            GameEngine.Instance.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {

            // TODO: move to the renderer
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, 
                null, controller.Camera.view);
            base.Draw(spriteBatch);
            level.Draw(spriteBatch, "Underlay");
            spriteBatch.End();
        }

    }
}
