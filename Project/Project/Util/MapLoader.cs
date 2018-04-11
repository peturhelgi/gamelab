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
        public Texture2D background;
        public ContentManager ContentManager;


        public MapLoader(ContentManager contentManager) {
            ContentManager = contentManager;
        }

        public GameState InitMap(string levelName) {
            GameState gameState = new GameState();

            string text = ContentManager.Load<string>(levelName);
            Level level = JsonConvert.DeserializeObject<Level>(text);

            foreach (GameObject obj in level.Objects) {

                switch (obj.Type) {
                    case "miner":
                        Miner miner = new Miner(obj.Position, obj.SpriteSize, obj.Velocity, obj.Mass, obj.Texture.Name);
                        gameState.AddActor(miner);
                        break;
                    case "rock":
                        Rock rock = new Rock(obj.Position, obj.SpriteSize, obj.Texture.Name);
                        gameState.AddCollectible(rock);
                        break;
                    case "ground":
                        Ground ground = new Ground(obj.Position, obj.SpriteSize, obj.Texture.Name);
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
           
            background = ContentManager.Load<Texture2D>("Sprites/Backgrounds/Background1");
            // TODO possibly add a hashed Map to only load every Texture once
            foreach (GameObject obj in gameState.GetAll()) {
                obj.Texture = ContentManager.Load<Texture2D>(obj.TextureString);
            }

        }

        public Texture2D getBackground() {
            return background;
        }
    }
}
