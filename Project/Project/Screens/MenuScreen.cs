using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.Util;
using Project.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Screens {
    class MenuScreen : GameScreen {
        MenuManager menuManager;
        public MenuScreen(string path) {
            controller = new MenuControls();
            menuManager = new MenuManager();
            this.Path = path;
        }

        public override void LoadContent() {
            base.LoadContent();
            menuManager.LoadContent(this.Path);
        }

        public override void UnloadContent() {
            base.UnloadContent();
            menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
