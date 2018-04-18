using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Project.GameObjects;
using Newtonsoft.Json;
using Project.Controls;


namespace Project.Screens {
    public class SplashScreen : GameScreen {

        //TODO: Have a possibility for more Images
        public Image Image;
        public Vector2 Position;
        public Tuple<string, string> NextScreen;

        public SplashScreen() {
            Path = "Content/Load/SplashScreen.json";
        }

        public override void LoadContent() {
            base.LoadContent();
            Image.LoadContent();
            this.controller = new GameController();
        }

        public override void UnloadContent() {
            base.UnloadContent();
            Image.UnloadContent();
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            Image.Update(gameTime);

            // TODO: couple with default game controller
            if(InputManager.Instance.KeyPressed(Keys.Enter, Keys.Space)) {
                ScreenManager.Instance.ChangeScreen(
                    NextScreen.Item1, 
                    NextScreen.Item2);
            }

        }
        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            Image.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}