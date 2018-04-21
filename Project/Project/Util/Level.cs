using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Project.GameObjects;
using Project.Screens;
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
        protected ScreenManager ScreenManager;

        public Level() { }
        public void Initialize(ScreenManager screenManager)
        {
            ScreenManager = screenManager;
            foreach(GameObject obj in Objects)
            {
                obj.Initialize(ScreenManager);
            }
        }

        public void LoadContent() {

            Background.LoadContent(ScreenManager);
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
    }
}