using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Newtonsoft.Json;
using Project.GameLogic;
using Project.GameLogic.GameObjects;
using Project.GameLogic.GameObjects.Miner;
using System.Collections.Generic;
using System.Linq;

namespace Project.LevelManager
{
    class MapLoader
    {
        public Texture2D background;
        public ContentManager ContentManager;


        public MapLoader(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }

        public GameState InitMap(string levelName)
        {
            GameState gameState = new GameState();

            string text = ContentManager.Load<string>(levelName);
            Level level = JsonConvert.DeserializeObject<Level>(text);

            foreach (Obj obj in level.objects)
            {

                switch (obj.Type)
                {
                    case "miner":
                        Miner miner = new Miner(obj.Position, obj.SpriteSize, obj.Velocity, obj.Mass, obj.Texture);
                        gameState.AddActor(miner);
                        break;
                    case "rock":
                        Rock rock = new Rock(obj.Position, obj.SpriteSize, obj.Texture);
                        gameState.AddCollectible(rock);
                        break;
                    case "ground":
                        Ground ground = new Ground(obj.Position, obj.SpriteSize, obj.Texture);
                        gameState.AddSolid(ground);
                        break;
                    case "end":
                        break;

                    default:
                        Console.WriteLine("Object of Type " + obj.Type + " not implemented.");
                        break;
                }
            }

            return gameState;
        }

        public void LoadMapContent(GameState gameState)
        {

            //background = ContentManager.Load<Texture2D>("Sprites/Backgrounds/Background1");
            // TODO possibly add a hashed Map to only load every Texture once
            foreach (GameObject obj in gameState.GetAll())
            {
                obj.Texture = ContentManager.Load<Texture2D>(obj.TextureString);
            }

            LoadMotionSheets(gameState);
        }

        public void LoadMotionSheets(GameState gameState) {

            List<Miner> miners = gameState.GetActors();
            Miner miner;
            Texture2D motionSprite;
            string minerPath = "Sprites/Miner";
            for (int i = 1; i < miners.Count + 1; ++i)
            {
                miner = miners[i - 1];
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/idle");
                miner.SetMotionSprite(motionSprite, MotionType.idle);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/walk");
                miner.SetMotionSprite(motionSprite, MotionType.walk_left);
                miner.SetMotionSprite(motionSprite, MotionType.walk_right);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/run");
                miner.SetMotionSprite(motionSprite, MotionType.run);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/jump");
                miner.SetMotionSprite(motionSprite, MotionType.jump);

            }
        }
    }                                      
}
