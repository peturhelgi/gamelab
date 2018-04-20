using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project.Util;
using Project.Controls;


namespace Project.Screens {
    public class GameScreen {
        protected ContentManager content;
        [JsonIgnore]
        public Type Type;
        public string Path;
        protected GameController controller;

        public GameScreen() {
            Type = this.GetType();
            Path = "Content/Load/"
                + Type.ToString().Replace("Project.Screens.", "") + ".json";
        }

        public GameScreen(string path) {
            Type = this.GetType();
            Path = path;
        }

        public virtual void LoadContent() => content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");

        public virtual void UnloadContent() {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime) {
            controller.HandleUpdate(gameTime);
            InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
