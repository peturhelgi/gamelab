using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.ImageEffects;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Util
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;
        public string Effects;
        public Texture2D Texture;
        public bool IsActive;
        public FadeEffect FadeEffect;

        Vector2 origin;
        ContentManager content;
        RenderTarget2D renderTarget;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectList;


        void SetEffect<T> (ref T effect)
        {
            if(effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T));
            } 
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            effectList.Add(effect.GetType().ToString().Replace("Project.ImageEffects.", ""), 
                (effect as ImageEffect));
        }

        public void ActivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = true;
                var obj = this;
                (effectList[effect] as ImageEffect).LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = false;
                (effectList[effect] as ImageEffect).UnloadContent();
            }
            
        }

        public Image() {
            Path = Text = String.Empty;
            FontName = "Fonts/Orbitron";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            Effects = string.Empty;
            effectList = new Dictionary<string, ImageEffect>();
        }

        public void LoadContent()
        {
            content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");
            if(Path != String.Empty) { Texture = content.Load<Texture2D>(Path); }

            font = content.Load<SpriteFont>(FontName);
            Vector2 dim = Vector2.Zero;
            if(Texture != null)
            {
                dim.X += Texture.Width;
                dim.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            } else
            {
                dim.Y = font.MeasureString(Text).Y;
            }

            dim.X += font.MeasureString(Text).X;
            if(SourceRect == Rectangle.Empty)
            {
                SourceRect = new Rectangle(0, 0, (int)dim.X, (int)dim.Y);
            }
            renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice,
                (int)dim.X, (int)dim.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
            {
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            Texture = renderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref this.FadeEffect);
            if(Effects != String.Empty)
            {
                string[] effects = Effects.Split(':');
                foreach(string effect in effects)
                {
                    ActivateEffect(effect);
                }
            }
        }

        public void UnloadContent()
        {
            content.Unload();
            foreach(var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(var effect in effectList)
            {
                if(effect.Value.IsActive)
                    effect.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRect.Width / 2, SourceRect.Height/2);
            //if(Texture != null)
            spriteBatch.Draw(Texture, origin + Position, SourceRect, Color.White * Alpha,
                0.0f, origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
