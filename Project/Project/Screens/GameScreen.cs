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
        protected ScreenManager ScreenManager;

        public GameScreen() {
            Type = this.GetType();
            Path = "Content/Load/"
                + Type.ToString().Replace("Project.Screens.", "") + ".json";
        }

        public void Initialize(ScreenManager screenManager)
        {
            ScreenManager = screenManager;
        }

        /// <summary>
        /// Use: LoadContent
        /// Pre: ScreenManager is not null
        /// Post: The GameScreen has an initialized ContentManager in content
        /// </summary>
        public virtual void LoadContent() => content = new ContentManager(
                ScreenManager.Content.ServiceProvider, "Content");

        public virtual void UnloadContent() {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime) {
            controller.HandleUpdate(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
