using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;

namespace TheGreatEscape.GameLogic.Renderer
{
    public class LightRenderer
    {
        public enum Lighttype { Circular, Directional };
        GraphicsDevice _graphicsDevice;
        Texture2D _circularLight;
        Vector2 _lightSize;
        Texture2D _directionalLight;
        RenderTarget2D _renderTarget;
        SpriteBatch _spriteBatch;



        public LightRenderer(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _circularLight = content.Load<Texture2D>("Lights/circular_light");
            _lightSize = _circularLight.Bounds.Size.ToVector2();
            _directionalLight = content.Load<Texture2D>("Lights/directional_light");
            _graphicsDevice = graphicsDevice;
            _renderTarget = new RenderTarget2D(
                _graphicsDevice, 
                _graphicsDevice.PresentationParameters.BackBufferWidth, 
                _graphicsDevice.PresentationParameters.BackBufferHeight);
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public RenderTarget2D Draw(GameTime gametime, int width, int height, 
            List<Light> lights, Matrix camera)
        {
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: camera);
            float offset = 0.8f;
            Vector2 lightCenter;
            foreach (Light light in lights)
            {
                Simplex.Noise.Seed = light.Owner.Seed;
                float flicker = Simplex.Noise.CalcPixel1D(gametime.TotalGameTime.Milliseconds / 25, 0.1f);
                flicker *= offset;
                offset += 0.2f;
                float pulse = 1 / (flicker / 5000 + 1.0f);
                float brightness = 1 / (flicker / 1000 + 1.0f);
                lightCenter = light.Center + light.Owner.Position
                    - (_lightSize * light.Scale * pulse * light.Skew);
                switch (light.Type)
                {
                    case Lighttype.Circular:
                        _spriteBatch.Draw(_circularLight, 
                            lightCenter, 
                            null, 
                            Color.White * brightness, 
                            0f, 
                            Vector2.Zero, 
                            light.Scale * 2.0f * pulse, 
                            SpriteEffects.None, 0);
                        break;
                    //new Vector2(0.8f, 1.5f)
                    case Lighttype.Directional:
                        _spriteBatch.Draw(_directionalLight, 
                            lightCenter, 
                            null, 
                            Color.White * brightness, 
                            0f, 
                            Vector2.Zero,
                            3.0f * light.Scale * pulse, 
                            SpriteEffects.None, 0);
                        break;
                }
            }

            _spriteBatch.End();
            return _renderTarget;
        }
    }

    public class Light
    {
        public Vector2 Center;
        public Vector2 Direction;
        public Vector2 Skew { private set; get; }
        public LightRenderer.Lighttype Type;
        public float Scale { private set; get; }
        public GameObject Owner;

        public Light(Vector2 center, Vector2 direction, 
            LightRenderer.Lighttype type, GameObject owner,
            Vector2 skew, float scale = 1.0f)
        {
            Center = center;
            Direction = direction;
            Type = type;
            Owner = owner;
            Skew = skew;
            Scale = scale;
        }
    }
}
