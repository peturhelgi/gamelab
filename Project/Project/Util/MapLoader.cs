using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameObjects;
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

        public MapLoader(List<GameObject> gameObjects, ContentManager contentManager) {
            this.gameObjects = gameObjects;
            this.contentManager = contentManager;
        }

        public GameState InitMap(string levelName) {
            GameState gameState = new GameState();
           /*
            string text = contentManager.Load<string>(levelName);

            Level level = JsonConvert.DeserializeObject<Level>(text);

            Miner miner1 = new Miner(
                new Vector2(level.player1Start.x, level.player1Start.y), 
                new Vector2(level.player1Start.vx, level.player1Start.vy), 
                level.player1Start.m, new BoundingBox());

            gameObjects.Add(miner1);
            gameState.AddMiner(miner1);

            //TODO: add miner2
            foreach (Rock obj in level.rocks) {
                //Rock rock = new Rock(new Vector2(obj.x, obj.y), obj.w, obj.h);
                //gameObjects.Add(rock);
                //gameState.AddRock(rock);
            }*/

            return gameState;
        }

        public void loadMapContent(GameState gameState) {
            gameState.GetMiner(0).Texture = contentManager.Load<Texture2D>("Miner");
            foreach (Rock rock in gameState.GetRocks()) {
                rock.Texture = contentManager.Load<Texture2D>("Rock");
            }
        }
    }
}
