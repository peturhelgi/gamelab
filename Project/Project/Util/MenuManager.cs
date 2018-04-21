using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Project.Screens;
using Project.Controls;

namespace Project.Util
{
    public class MenuManager
    {
        Menu menu;
        bool InTransition;

        GameController Controller;

        void Transition(GameTime gameTime)
        {
            if(InTransition)
            {
                float first, last;
                for(int i = 0; i < menu.Items.Count; ++i)
                {
                    menu.Items[i].Image.Update(gameTime);
                    first = menu.Items[0].Image.Alpha;
                    last = menu.Items[menu.Items.Count - 1].Image.Alpha;
                    if(first == 0.0f && last == 0.0f)
                    {
                        menu.ID = menu.Items[menu.ItemNumber].Link;
                    }
                    else if(first == 1.0f && last == 1.0f)
                    {
                        InTransition = false;
                        foreach(MenuItem item in menu.Items)
                        {
                            item.Image.RestoreEffects();
                        }
                    }
                }
            }
        }

        public MenuManager(MenuControls controller)
        {
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
            Controller = controller;
        }

        void menu_OnMenuChange(object sender, EventArgs e)
        {
            DataManager<Menu> menuManager = new DataManager<Menu>();
            menu.UnloadContent();
            menu = menuManager.Load(menu.ID);
            menu.LoadContent();
            Controller.Initialize(ref menu);        // send menu instance to controller
            menu.OnMenuChange += menu_OnMenuChange;
            menu.Transition(0.0f);

            foreach(MenuItem item in menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }

        public void LoadContent(string menuPath)
        {
            if(menuPath != String.Empty)
            {
                menu.ID = menuPath;
            }
        }

        public void UnloadContent()
        {
            menu.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if(!InTransition)
            {
                // Updates the states of the controller.
                Controller.HandleUpdate(gameTime);
                menu.Update(gameTime);
            }
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
