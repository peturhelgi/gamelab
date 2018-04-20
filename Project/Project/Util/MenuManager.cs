using System;
using System.Collections.Generic;
using System.Linq;
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
            // Updates the states of the controller.
            Controller.HandleUpdate(gameTime);
            if(!InTransition)
            {
                if(menu.IsVertical())
                {
                    if(Controller.ButtonPressed(
                       (Buttons)MenuControls.Instructions.Down,
                       (Buttons)MenuControls.Instructions.DownStick))
                    {
                        menu.NextItem();
                    }
                    else if(Controller.ButtonPressed(
                        (Buttons)MenuControls.Instructions.Up,
                        (Buttons)MenuControls.Instructions.UpStick))
                    {
                        menu.PreviousItem();
                    }
                }

                else if(menu.IsHorizontal())
                {
                    if(Controller.ButtonPressed(
                       (Buttons)MenuControls.Instructions.Right,
                       (Buttons)MenuControls.Instructions.RightStick))
                    {
                        menu.NextItem();
                    }
                    else if(Controller.ButtonPressed(
                        (Buttons)MenuControls.Instructions.Left,
                        (Buttons)MenuControls.Instructions.LeftStick))
                    {
                        menu.PreviousItem();
                    }
                }
                menu.Update(gameTime);
            }

            if(Controller.ButtonPressed(
                (Buttons)MenuControls.Instructions.Select)
                && !InTransition)
            {
                switch(menu.Items[menu.ItemNumber].LinkId.ToLower())
                {
                    case "screen":
                        ScreenManager.Instance.ChangeScreen(
                            menu.Items[menu.ItemNumber].LinkType,
                            menu.Items[menu.ItemNumber].Link);
                        break;
                    case "menu":
                        InTransition = true;
                        menu.Transition(1.0f);
                        foreach(MenuItem item in menu.Items)
                        {
                            item.Image.StoreEffects();
                            item.Image.ActivateEffect("FadeEffect");
                        }
                        break;
                    default:
                        break;
                }
            }
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
