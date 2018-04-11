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

namespace Project.Screens
{
    public class SplashScreen : GameScreen
    {
        public Image Image;

        public Vector2 Position;

        public SplashScreen() {
            Path = "Content/Load/SplashScreen.json";
        }
        //private ContentManager contentManager;
        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter, Keys.Z))
            {
                ScreenManager.Instance.ChangeScreen("TitleScreen", "Content/Load/TitleMenu");
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}