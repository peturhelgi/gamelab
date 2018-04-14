﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Screens {
    class TitleScreen : GameScreen {
        MenuManager menuManager;
        public TitleScreen(string path) {
            menuManager = new MenuManager();
            this.Path = path;
            //this.Path = "Content/Load/TitleMenu.json";
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
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }
    }
}