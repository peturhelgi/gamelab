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

            background = ContentManager.Load<Texture2D>("Sprites/Backgrounds/Background1");
            // TODO possibly add a hashed Map to only load every Texture once
            foreach (GameObject obj in gameState.GetAll())
            {
                obj.Texture = ContentManager.Load<Texture2D>(obj.TextureString);
            }

            LoadMotionSheets(gameState);
        }

        public void LoadMotionSheets(GameState gameState) {

            List<Miner> miners = gameState.GetActors();
            Miner miner = miners.ElementAt(0);
            // TODO: add a generic way of loading the motion sheets, when more assets are made
            Texture2D motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/idle_sheet_24");
            miner.SetMotionSprite(motionSprite, MotionType.idle);
            motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/walk_sheet_11");
            miner.SetMotionSprite(motionSprite, MotionType.walk);
            motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/run_sheet_12");
            miner.SetMotionSprite(motionSprite, MotionType.run);
            motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/jump_sheet_12");
            miner.SetMotionSprite(motionSprite, MotionType.jump);

            if (miners.Last() != miner)
            {
                miner = miners.ElementAt(1);
                motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/idle_sheet_24_b");
                miner.SetMotionSprite(motionSprite, MotionType.idle);
                motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/walk_sheet_11_b");
                miner.SetMotionSprite(motionSprite, MotionType.walk);
                motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/run_sheet_12_b");
                miner.SetMotionSprite(motionSprite, MotionType.run);
                motionSprite = ContentManager.Load<Texture2D>("Sprites/Miners/jump_sheet_12_b");
                miner.SetMotionSprite(motionSprite, MotionType.jump);
            }

        }
        public Texture2D getBackground()
        {
            return background;
        }
    }                                      
}
