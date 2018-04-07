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
        public ContentManager ContentManager;


        public MapLoader(ContentManager contentManager) {
            ContentManager = contentManager;
        }

        public GameState InitMap(string levelName) {
            GameState gameState = new GameState();

            string text = ContentManager.Load<string>(levelName);
            Level level = JsonConvert.DeserializeObject<Level>(text);

            foreach (Obj obj in level.objects) {

                switch (obj.Type) {
                    case "miner":
                        Miner miner = new Miner(obj.Position, obj.Dimension, obj.Velocity, obj.Mass, obj.Texture);
                        gameState.AddActor(miner);
                        break;
                    case "rock":
                        Rock rock = new Rock(obj.Position, obj.Dimension, obj.Texture);
                        gameState.AddCollectible(rock);
                        break;
                    case "ground":
                        Ground ground = new Ground(obj.Position, obj.Dimension, obj.Texture);
                        gameState.AddSolid(ground);
                        break;
                    case "end":
                        break;

                    default:
                        Console.WriteLine("Object of Type "+obj.Type+" not implemented.");
                        break;
                }
            }

            return gameState;
        }

        public void LoadMapContent(GameState gameState) {
           
            // TODO possibly add a hashed Map to only load every Texture once
            foreach (GameObject obj in gameState.GetAll()) {
                obj.Texture = ContentManager.Load<Texture2D>(obj.TextureString);
            }

        }
    }
}
