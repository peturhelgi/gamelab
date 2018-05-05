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

            foreach (Light light in lights)
            {
                Simplex.Noise.Seed = light.Owner.Seed;
                float flicker = Simplex.Noise.CalcPixel1D(
                    gametime.TotalGameTime.Milliseconds / 25, 0.1f);
                flicker *= offset;
                offset += 0.2f;

                float pulse = 1 / (flicker / 5000 + 1.0f),
                    brightness = 1 / (flicker / 1000 + 1.0f),
                    rotation = 0.0f,
                    PI = 3.1415926535f;

                Texture2D currentTexture = null;
                bool render = true;
                switch (light.Type)
                {
                    case Lighttype.Circular:
                        currentTexture = _circularLight;
                        rotation = 0.0f;
                        break;

                    case Lighttype.Directional:
                        currentTexture = _directionalLight;
                        rotation = (light.Owner as Miner).LookAt;
                        if ((light.Owner as Miner).Orientation != SpriteEffects.FlipHorizontally)
                        {
                            
                            rotation = PI - rotation;
                        }
                        break;
                    default:
                        render = false;
                        break;
                }
                if (render)
                {
                    _spriteBatch.Draw(
                            currentTexture,
                            light.Owner.Position + light.Offset, // position,
                            null, // source rectangle
                            Color.White * brightness,
                            rotation, // rotation in radians, positive is cw
                            _lightSize * light.Origin, // origin in image coords
                            light.Scale, // scale of the image
                            SpriteEffects.None,
                            0); // layerdepth
                }
            }

            _spriteBatch.End();
            return _renderTarget;
        }
    }

    public class Light
    {
        public Vector2 Offset;
        public Vector2 Origin;
        public Vector2 Direction;
        public Vector2 Scale { private set; get; }
        public LightRenderer.Lighttype Type;
        public GameObject Owner;

        public Light(Vector2 offset, Vector2 direction,
            LightRenderer.Lighttype type, GameObject owner,
            Vector2 scale, Vector2 origin)
        {
            Offset = offset;
            Direction = direction;
            Type = type;
            Owner = owner;
            Scale = scale;
            Origin = origin;
        }
    }
}
