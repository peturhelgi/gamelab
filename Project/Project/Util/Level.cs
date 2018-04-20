using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Project.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Project.Util {
    public class Level {

        public int PrevLvl;
        public int LevelNr;
        public int NextLvl;
        public string PrevLvlName;
        public string LevelName;
        public string NextLvlName;
        public Image Background;
        public List<GameObject> Objects;

        public Level() { }

        public void LoadContent() {

            Background.LoadContent();
            foreach(GameObject obj in Objects) {
                obj.LoadContent();
            }            
        }

        public void UnloadContent() {
            Background.UnloadContent();
            foreach(GameObject obj in Objects) {
                obj.UnloadContent();
            }
        }

        public void Draw(SpriteBatch spriteBatch, string drawType) {
            Background.Draw(spriteBatch);
            //TODO: Consider if this is best practice
            foreach(GameObject obj in Objects) {
                obj.Draw(spriteBatch);
            }
        }
    }
}