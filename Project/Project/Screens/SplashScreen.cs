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
using Project.GameStates;
using Newtonsoft.Json;
using Project.Controls;
using Project.Render;


namespace Project.Screens
{
    public class SplashScreen : GameScreen
    {

        //TODO: Have a possibility for more Images
        public Image Image;
        public List<GameObject> Objects;
        public Vector2 Position;
        public Tuple<string, string> NextScreen;

        public SplashScreen()
        {
            Path = "Content/Load/SplashScreen.json";
            GameState = new GameState();
            Objects = new List<GameObject>();
            Renderer = new Renderer();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent(ScreenManager);
            GameState state = new GameState();

            foreach(var obj in Objects)
            {
                obj.LoadContent();
                state.AddObject(obj);
            }
            //GameObject obj = new GameObject();
            //TODO: init gameastate and add to json
            this.controller = new GameController(
                new Camera(0.0f, Vector2.Zero));
            Renderer.Initialize(ref state, ref controller.Camera);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
            foreach(var obj in Objects)
            {
                obj.UnloadContent();
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
            
            foreach(var obj in Objects){
                obj.Image.Update(gameTime);
            }

            KeyboardState state = Keyboard.GetState();
            // TODO: couple with default game controller
            if(controller.ButtonPressed(
                Buttons.A, Buttons.Start)
                || state.IsKeyDown(Keys.Space)
                || state.IsKeyDown(Keys.Enter))
            {
                ScreenManager.ChangeScreen(
                    NextScreen.Item1,
                    NextScreen.Item2);
            }

        }
    }
}