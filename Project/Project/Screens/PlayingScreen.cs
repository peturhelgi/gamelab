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


namespace Project.Screens {
    public class PlayingScreen : GameScreen {
        Miner miner;
        Level level;
        GameEngine engine;
        GameState state;
        string baseFolder = "Content/GamePlay/Levels/";

        public PlayingScreen(string path) {
            this.Path = path;
        }

        public void Init() {
            // mapLoader.InitMap(lvlName
            state = new GameState();
            engine = new GameEngine(state);
        }

        public override void LoadContent() {
            base.LoadContent();

            DataManager<Miner> minerLoader = new DataManager<Miner>();
            DataManager<Level> levelLoader = new DataManager<Level>();
            miner = new Miner();//minerLoader.Load("Content/GamePlay/Miner");

            level = levelLoader.Load(baseFolder + this.Path);
            // miner.LoadContent();
            level.LoadContent();
            //state.LoadContent(ref level);
        }

        public override void UnloadContent() {
            base.UnloadContent();
            //miner.UnloadContent();
            level.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            //miner.Update(gameTime);
            level.Update(gameTime, ref miner);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            level.Draw(spriteBatch, "Underlay");
            //miner.Draw(spriteBatch);
            level.Draw(spriteBatch, "Overlay");
        }

    }
}
