using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.GameObjects;

namespace TheGreatEscape.LevelManager
{
    class MapLoader
    {
        public ContentManager ContentManager;


        public MapLoader(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }

        public GameState InitMap(string levelName)
        {
            GameObjectFactory factory = new GameObjectFactory();
            GameState gameState = new GameState();

            string text = ContentManager.Load<string>(levelName);
            Level level = JsonConvert.DeserializeObject<Level>(text);
            gameState.background = level.background;
            
            foreach (Obj obj in level.objects)
            {
                GameObject gameObject = factory.Create(obj);
                gameState.Add(gameObject);
            }

            return gameState;
        }

        public void LoadMapContent(GameState gameState)
        {
            Texture2D background = ContentManager.Load<Texture2D>(gameState.background);
            gameState.SetBackground(background);

            // TODO possibly add a hashed Map to only load every Texture once
            foreach (GameObject obj in gameState.GetAll())
            {
                if(obj?.TextureString == null || obj.TextureString == "")
                {
                    continue;
                }
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
                miner.SetMotionSprite(motionSprite, MotionType.run_left);
                miner.SetMotionSprite(motionSprite, MotionType.run_right);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/jump");
                miner.SetMotionSprite(motionSprite, MotionType.jump);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/pickaxe");
                miner.SetMotionSprite(motionSprite, MotionType.pickaxe);

            }
        }
    }                                      
}
