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
using Project.Screens;

namespace Project.Util {
    public class Image {
        public float Alpha;
        public string Text, FontName, Path, Effects;
        [JsonProperty("pos")]
        public Vector2 Position;
        [JsonProperty("dim")]
        public Vector2 SpriteSize;
        public Vector2 Scale;
        public Rectangle SourceRect;
        public Texture2D Texture;
        public bool FullScreen;
        public bool IsActive;
        public FadeEffect FadeEffect;
        public SpriteSheetEffect SpriteSheetEffect;

        public Vector2 Origin;
        ContentManager content;
        RenderTarget2D renderTarget;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectList;

        public Vector2 BBox {
            get {
                return new Vector2(0, 0);
            }
        }

        void SetEffect<T>(ref T effect) {
            if(effect == null) {
                effect = (T)Activator.CreateInstance(typeof(T));
            } else {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            String effectName = effect.GetType().ToString();
            effectName = effectName.Substring(effectName.LastIndexOf(".") + 1);
            effectList.Add(effectName, (effect as ImageEffect));
        }

        public void ActivateEffect(string effect) {
            if(effectList.ContainsKey(effect)) {
                effectList[effect].IsActive = true;
                var obj = this;
                (effectList[effect] as ImageEffect).LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect) {
            if(effectList.ContainsKey(effect)) {
                effectList[effect].IsActive = false;
                (effectList[effect] as ImageEffect).UnloadContent();
            }
        }

        public void StoreEffects() {
            Effects = String.Empty;
            foreach(var effect in effectList) {
                if(effect.Value.IsActive) {
                    Effects += effect.Key + ":";
                }
            }
            if(Effects != String.Empty) {
                Effects.Remove(Effects.Length - 1);
            }
        }

        public void RestoreEffects() {
            foreach(var effect in effectList) {
                DeactivateEffect(effect.Key);
            }
            string[] split = Effects.Split(':');
            foreach(string effect in split) {
                ActivateEffect(effect);
            }
        }

        public Image() {
            Alpha = 1.0f;
            effectList = new Dictionary<string, ImageEffect>();
            Effects = string.Empty;
            FontName = "Fonts/Orbitron";
            FullScreen = false;
            Path = Text = String.Empty;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            SpriteSize = Vector2.Zero;
            SourceRect = Rectangle.Empty;
        }

        public void LoadContent() {
            content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");
            if(Path != String.Empty) { Texture = content.Load<Texture2D>(Path); }

            font = content.Load<SpriteFont>(FontName);
            Vector2 dim = Vector2.Zero;
            if(Texture != null) {
                if(SpriteSize.X != 0) {
                    Scale.X *= (float)(SpriteSize.X / Texture.Width);
                }
                dim.X += Texture.Width;
                if(SpriteSize.Y != 0) {
                    Scale.Y *= (float)(SpriteSize.Y / Texture.Height);
                }
                dim.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            } else {
                dim.Y = font.MeasureString(Text).Y;
            }
            dim.X += font.MeasureString(Text).X;

            if(FullScreen) {
                dim = ScreenManager.Instance.Dimensions;
            }

            if(SourceRect == Rectangle.Empty) {
                SourceRect = new Rectangle(
                    0, 0,
                    (int)dim.X, (int)dim.Y);
            }

            renderTarget = new RenderTarget2D(
                ScreenManager.Instance.GraphicsDevice, (int)dim.X, (int)dim.Y);

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if(Texture != null) {
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero,
                    Color.White);
            }
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text,
                Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            Texture = renderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref this.FadeEffect);
            SetEffect<SpriteSheetEffect>(ref this.SpriteSheetEffect);

            if(Effects != String.Empty) {
                string[] effects = Effects.Split(':');
                foreach(string effect in effects) {
                    ActivateEffect(effect);
                }
            }
        }

        public void UnloadContent() {
            content.Unload();
            foreach(var effect in effectList) {
                DeactivateEffect(effect.Key);
            }
        }

        public void Update(GameTime gameTime) {
            foreach(var effect in effectList) {
                if(effect.Value.IsActive) {
                    effect.Value.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {

            try {
                //Origin.X = SpriteSize.X / 2;
                //Origin.Y = SpriteSize.Y / 2;
                Origin.X = 0;
                Origin.Y = 0;

                //Origin.X = SourceRect.Width / 2 / Scale.X;
                //Origin.Y = SourceRect.Height / 2 / Scale.Y;
                spriteBatch.Draw(Texture, Origin + Position, SourceRect,
                    Color.White * Alpha, 0.0f, Origin, Scale,
                    SpriteEffects.None, 0.0f);
            } catch(ArgumentNullException) { }
        }
    }
}
