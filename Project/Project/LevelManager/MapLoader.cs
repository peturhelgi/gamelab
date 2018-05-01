using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Newtonsoft.Json;
using TheGreatEscape.GameLogic;
using TheGreatEscape.GameLogic.GameObjects;

namespace TheGreatEscape.LevelManager
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
            GameObjectFactory factory = new GameObjectFactory();
            GameState gameState = new GameState();

            string text = ContentManager.Load<string>(levelName);
            Level level = JsonConvert.DeserializeObject<Level>(text);

            foreach (Obj obj in level.objects)
            {
                GameObject gameObject = factory.Create(obj);
                gameState.AddObject(gameObject);
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
        }

        public Texture2D getBackground()
        {
            return background;
        }
    }
}
