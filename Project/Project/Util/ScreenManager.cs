using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Screens;
using Newtonsoft.Json;

namespace Project.Util
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        [JsonIgnore]
        public Vector2 Dimensions { set; get; }
        [JsonIgnore]
        public ContentManager Content { private set; get; }
        [JsonIgnore]
        public GraphicsDevice GraphicsDevice;
        [JsonIgnore]
        public SpriteBatch SpriteBatch;

        DataManager<GameScreen> gameScreenManager;
        GameScreen currentScreen, nextScreen;

        public Image Image;
        [JsonIgnore]
        public bool InTranstition { get; private set; }


        public static ScreenManager Instance {
            get {
                if (instance == null)
                {
                    DataManager<ScreenManager> data = new DataManager<ScreenManager>();
                    instance = data.Load("Content/Load/ScreenManager.json");
                }
                return instance;
            }
        }

        // TODO: Things
        public void ChangeScreen(string screenName) {
            nextScreen = (GameScreen)Activator.CreateInstance(Type.GetType("Project.Screens."+ screenName));
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            InTranstition = true;
        }

        void Transition(GameTime gameTime)
        {
            if (InTranstition)
            {
                Image.Update(gameTime);
                if(Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = nextScreen;
                    gameScreenManager.Type = currentScreen.Type;
                    if (File.Exists(currentScreen.Path))
                    {
                        currentScreen = gameScreenManager.Load(currentScreen.Path);
                    }
                    currentScreen.LoadContent();
                }
                else if(Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    InTranstition = false;
                }

            }
        }
        public ScreenManager() {
            Dimensions = new Vector2(640, 480);
            currentScreen = new SplashScreen();
            gameScreenManager = new DataManager<GameScreen>
            {
                Type = currentScreen.Type
            };
            currentScreen = gameScreenManager.Load("Content/Load/SplashScreen.json");
        }

        public void LoadContent(ContentManager Content) {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }

        public void UnloadContent(){
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime) {
            currentScreen.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            currentScreen.Draw(spriteBatch);
            if (InTranstition)
            {
                Image.Draw(spriteBatch);
            }
        }
    }
}
