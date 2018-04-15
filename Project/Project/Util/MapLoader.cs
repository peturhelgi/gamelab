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

namespace Project.Util {
    class MapLoader {
        public Texture2D background;
        public ContentManager ContentManager;


        public MapLoader(ContentManager contentManager) {
            ContentManager = contentManager;
        }

        public void LoadMapContent(GameState gameState) {

            background = ContentManager.Load<Texture2D>("Sprites/Backgrounds/Background1");
            // TODO possibly add a hashed Map to only load every Texture once
            foreach(GameObject obj in gameState.GetAll()) {
                obj.Texture = ContentManager.Load<Texture2D>(obj.TextureString);
            }
        }

        public Texture2D getBackground() {
            return background;
        }
    }
}
