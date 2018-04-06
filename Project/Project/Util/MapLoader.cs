using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameObjects;
using Project.GameObjects.Miner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project.Util
{
    class MapLoader
    {
        public List<GameObject> gameObjects;
        public ContentManager contentManager;
        public Texture2D background;

        public MapLoader(List<GameObject> gameObjects, ContentManager contentManager) {
            this.gameObjects = gameObjects;
            this.contentManager = contentManager;
        }

        public GameState initMap(string levelName) {
            GameState gameState = new GameState();

            string text = contentManager.Load<string>(levelName);
            Level level = JsonConvert.DeserializeObject<Level>(text);

            Miner miner1 = new Miner(new Vector2(level.player1Start.x, level.player1Start.y), new Vector2(level.player1Start.vx, level.player1Start.vy), level.player1Start.m, new BoundingBox());
            gameObjects.Add(miner1);
            gameState.addMiner1(miner1);
            //TODO: add miner2
            foreach (Obj obj in level.rocks) {
                Rock rock = new Rock(new Vector2(obj.x, obj.y), obj.w, obj.h);
                gameObjects.Add(rock);
                gameState.addRock(rock);
            }

            return gameState;
        }

        public void loadMapContent(GameState gameState) {
            background = contentManager.Load<Texture2D>("backgrounds/background_1");

            gameState.getMiner1().Texture = contentManager.Load<Texture2D>("miners/miner_hands_in_pants");
            foreach (Rock rock in gameState.getRocks()) {
                rock.Texture = contentManager.Load<Texture2D>("rocks/rock");
            }
        }

        public Texture2D getBackground() {
            return background;
        }
    }
}
