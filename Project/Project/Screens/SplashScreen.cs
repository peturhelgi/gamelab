using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Project.Screens
{
    public class SplashScreen : GameScreen
    {

        Texture2D image;
        string path;


        public override void LoadContent()
        {
            base.LoadContent();
            path = "Assets/SplashScreen.scale-200.png";
            image = content.Load<Texture2D>(path);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Vector2.Zero, Color.White);
        }
    }
}
