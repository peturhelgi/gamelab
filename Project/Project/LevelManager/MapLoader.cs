using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.GameObjects;
using static TheGreatEscape.GameLogic.GameState;
using System;
using TheGreatEscape.GameLogic.Util;
using Project.LeveManager;

namespace TheGreatEscape.LevelManager
{
    class MapLoader
    {
        public ContentManager ContentManager;
        private JsonUtil<Level> _loader;

        public MapLoader(ContentManager contentManager)
        {
            ContentManager = contentManager;
            _loader = new JsonUtil<Level>();
        }

        public GameState InitMap(string levelName)
        {
            GameObjectFactory factory = new GameObjectFactory();
            GameState gameState = new GameState();

            //string text = ContentManager.Load<string>(levelName);
            //Level level = JsonConvert.DeserializeObject<Level>(text);
            Level level = _loader.Load(levelName);
            gameState.background = level.background;
            gameState.resources = level.resources;
            gameState.levelname = level.levelname;

            foreach (Obj obj in level.objects)
            {
                GameObject gameObject = factory.Create(obj);
                gameState.Add(gameObject);
                if (gameObject is Platform) gameState.Add((gameObject as Platform).Background, GameState.Handling.None);
                if(gameObject is Door)
                {
                    var lockedLight = (gameObject as Door).LockedLight;
                    var unlockedLight = (gameObject as Door).UnlockedLight;
                    gameState.Add(lockedLight, GameState.Handling.None);
                    gameState.Add(unlockedLight, GameState.Handling.None);
                }
            }

            //gameState.InstantiateTools();

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
                if (obj is Lever)
                    (obj as Lever).SecondTexture = ContentManager.Load<Texture2D>((obj as Lever).RightleverTexture);
                if (obj is RockHook)
                    (obj as RockHook).Rope.Texture = ContentManager.Load<Texture2D>((obj as RockHook).Rope.TextureString);
            }

            LoadMotionSheets(gameState);
            LoadTools(gameState);
            gameState.GameFont = ContentManager.Load<SpriteFont>("Fonts/gameFont");
        }

        private void LoadTools(GameState gameState) {

            String toolSpritePath = "Sprites/Tools/";

            foreach (ExistingTools et in Enum.GetValues(typeof(ExistingTools)))
            {
                Texture2D toolSprite = ContentManager.Load<Texture2D>(toolSpritePath + et.ToString());
                switch (et)
                {
                    case ExistingTools.pickaxe:
                        Pickaxe.ToolSprite = toolSprite;
                        break;
                    case ExistingTools.rope:
                        Rope.ToolSprite = toolSprite;
                        break;
                    default:
                        MyDebugger.WriteLine(
                            string.Format("GameObject '{0}' cannot be created", true));
                        break;
                }
            }
        }
        private void LoadMotionSheets(GameState gameState) {

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
                miner.SetMotionSprite(motionSprite, MotionType.walk);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/run");
                miner.SetMotionSprite(motionSprite, MotionType.run);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/jump");
                miner.SetMotionSprite(motionSprite, MotionType.jump);
                motionSprite = ContentManager.Load<Texture2D>(minerPath + i + "/pickaxe");
                miner.SetMotionSprite(motionSprite, MotionType.pickaxe);

            }
        }
    }                                      
}
