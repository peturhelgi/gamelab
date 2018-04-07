using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Util
{
    public class MenuManager
    {
        Menu menu;
        public MenuManager()
        {
            menu = new Menu();
            menu.OnMenuChange += menu_OnMenuChange;
        }

        void menu_OnMenuChange(object sender, EventArgs e)
        {
            DataManager<Menu> menuManager = new DataManager<Menu>();
            menu.UnloadContent();
            // Add transitino
            menu = menuManager.Load(menu.ID);
            menu.LoadContent();
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
            menu.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
