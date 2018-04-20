using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.Screens;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project.Util {
    public class Menu {
        public event EventHandler OnMenuChange;
        protected ScreenManager ScreenManager;
        public string Axis, Effects;
        public List<MenuItem> Items;

        int itemNumber;
        string id;

        public int ItemNumber => itemNumber;

        public string ID
        {
            get => id;
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        public void Transition(float alpha) {
            foreach(MenuItem item in Items) {
                item.Image.IsActive = true;
                item.Image.Alpha = alpha;
                item.Image.FadeEffect.Increase = (alpha == 0.0f);
            }
        }

        /// <summary>
        /// Gives information about the alignment of the menu items
        /// </summary>
        /// <returns>True if elements stack vertically</returns>
        public bool IsVertical() => Axis == "Y";
        /// <summary>
        /// Gives information about the alignment of the menu items
        /// </summary>
        /// <returns>True if elements stack horizontally</returns>
        public bool IsHorizontal() => Axis == "X";


        /// <summary>
        /// Use: AlignMenuItems()
        /// Pre: Initialize() has been called for this instance
        /// Post: The menu items have been aligned according to Axis
        /// </summary>
        void AlignMenuItems() {
            Vector2 dimensions = Vector2.Zero;
            foreach(MenuItem item in Items) {
                dimensions += new Vector2(item.Image.SourceRect.Width,
                    item.Image.SourceRect.Height);
            }

            dimensions = new Vector2(
                (ScreenManager.Dimensions.X - dimensions.X) / 2,
                (ScreenManager.Dimensions.Y - dimensions.Y) / 2);

            foreach(MenuItem item in Items) {
                if(Axis == "X") {
                    item.Image.Position = new Vector2(
                        dimensions.X,
                        (ScreenManager.Dimensions.Y - item.Image.SourceRect.Height) / 2);
                } else if(Axis == "Y") {
                    item.Image.Position = new Vector2(
                        (ScreenManager.Dimensions.X - item.Image.SourceRect.Width) / 2,
                        dimensions.Y);
                }

                dimensions += new Vector2(item.Image.SourceRect.Width,
                    item.Image.SourceRect.Height);
            }
        }

        /// <summary>
        /// Increases ItemNumber by one, in a circular fashin
        /// </summary>
        public void NextItem() => itemNumber = (++itemNumber) % Items.Count;

        /// <summary>
        /// Decreases ItemNumber by one, in a circular fashin
        /// </summary>
        public void PreviousItem() => itemNumber = (--itemNumber) % Items.Count;

        public Menu() {
            id = String.Empty;
            itemNumber = 0;
            Effects = String.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void Initialize(ScreenManager screenManager)
        {
            ScreenManager = screenManager;
        }

        /// <summary>
        /// Use: LoadContent();
        /// Pre: Inititalize has been called for this instance
        /// Post: All menu items have been loaded and aligned
        /// </summary>
        public void LoadContent() {
            string[] split = Effects.Split(':');
            foreach(MenuItem item in Items) {
                item.Image.LoadContent(ScreenManager);
                foreach(string effect in split) {
                    item.Image.ActivateEffect(effect);
                }
            }
            AlignMenuItems();
        }

        public void UnloadContent() {
            foreach(MenuItem item in Items) {
                item.Image.UnloadContent();
            }
        }

        public void Update(GameTime gameTime) {
            /*KeyboardState state = Keyboard.GetState();
            if(IsHorizontal()) {
                if(InputManager.Instance.KeyPressed(Keys.Right)) {
                    ++itemNumber;
                } else if(InputManager.Instance.KeyPressed(Keys.Left)) {
                    --itemNumber;
                }
            } else if(IsVertical()) {
                if(InputManager.Instance.KeyPressed(Keys.Down)) {
                    ++itemNumber;
                } else if(InputManager.Instance.KeyPressed(Keys.Up)) {
                    --itemNumber;
                }
            }*/

            if(itemNumber < 0) { itemNumber = Items.Count - 1; }
            if(itemNumber >= Items.Count) { itemNumber = 0; }

            for(int i = 0; i < Items.Count; ++i) {
                if(i == itemNumber) {
                    Items[i].Image.IsActive = true;
                } else {
                    Items[i].Image.IsActive = false;
                }
                Items[i].Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(MenuItem item in Items) {
                item.Image.Draw(spriteBatch);
            }
        }
    }
}
