using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.Util;
using TheGreatEscape.Menu;

namespace TheGreatEscape.EditorLogic
{
    public class ParameterMenu<T> : Screen
    {
        protected SpriteBatch _spriteBatch;

        protected List<Option> Options;
        protected int CurrentPosition = 0;

        public ParameterMenu(GraphicsDevice graphicsDevice,
            MenuManager manager):
            base(graphicsDevice, null)
        {
            //var a = obj.GetType().GetProperties();
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            Input = new TheGreatEscape.Util.InputManager(1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Input.ButtonDown(0, Buttons.DPadDown)
                || Input.ButtonDown(1, Buttons.DPadDown))
            {
                CurrentPosition = ++CurrentPosition % Options.Count;
            }
            if (Input.ButtonDown(0, Buttons.DPadUp)
                || Input.ButtonDown(0, Buttons.DPadUp))
            {
                CurrentPosition = (--CurrentPosition + Options.Count) % Options.Count;
            }

            if (Input.ButtonDown(0, Buttons.DPadLeft)
                || Input.ButtonDown(1, Buttons.DPadLeft))
            {
                CurrentPosition = ++CurrentPosition % Options.Count;
            }
            if (Input.ButtonDown(0, Buttons.DPadRight)
                || Input.ButtonDown(0, Buttons.DPadRight))
            {
                CurrentPosition = (--CurrentPosition + Options.Count) % Options.Count;
            }
        }

        public override void Draw(GameTime gameTime, int widht, int height)
        {
            _graphicsDevice.Clear(Color.Gray);
            _spriteBatch.Begin();

            _spriteBatch.DrawString(_manager.MenuFont, "_title",
                new Vector2(50f, 50f), Color.White, 0f, new Vector2(), 0.5f,
                new SpriteEffects(), 0f);
            for (var i = 0; i < Options.Count; i++)
            {
                _spriteBatch.DrawString(_manager.MenuFont, Options[i].Value,
                    new Vector2(i == CurrentPosition ? 100 : 50,
                    (i * 100.0f) + 150),
                    Color.White, 0f, new Vector2(),
                    i == CurrentPosition ? 1f : 0.75f, new SpriteEffects(), 0f);
            }
            _spriteBatch.End();
        }
    }

    public sealed class Option
    {
        public string Name, Value;
        public Option(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
