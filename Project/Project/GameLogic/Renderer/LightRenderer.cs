using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic.Renderer
{
    public class LightRenderer
    {
        public enum Lighttype { Circular, Directional };
        GraphicsDevice _graphicsDevice;
        Texture2D _circularLight;
        Texture2D _directionalLight;
        RenderTarget2D _renderTarget;
        SpriteBatch _spriteBatch;



        public LightRenderer(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _circularLight = content.Load<Texture2D>("Lights/circular_light");
            _directionalLight = content.Load<Texture2D>("Lights/directional_light");
            _graphicsDevice = graphicsDevice;
            _renderTarget = new RenderTarget2D(
                _graphicsDevice, 
                _graphicsDevice.PresentationParameters.BackBufferWidth, 
                _graphicsDevice.PresentationParameters.BackBufferHeight);
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public RenderTarget2D Draw(int width, int height, List<Light> lights, Matrix camera)
        {
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: camera);
            foreach (Light light in lights)
            {
                switch (light.Type) {
                    case Lighttype.Circular:
                        _spriteBatch.Draw(_circularLight, light.Center - (_circularLight.Bounds.Size.ToVector2() * 1.5f), null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0);
                        break;

                    case Lighttype.Directional:
                        _spriteBatch.Draw(_directionalLight, light.Center - (_circularLight.Bounds.Size.ToVector2() * new Vector2(0.8f, 1.5f)), null, Color.White, light.Rotation, Vector2.Zero, 3f, SpriteEffects.None, 0);
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
        public LightRenderer.Lighttype Type;
        public float Rotation;

        public Light(Vector2 center, Vector2 direction, LightRenderer.Lighttype type)
        {
            Center = center;
            Direction = direction;
            Type = type;
            Rotation = (float)Math.Atan2(Direction.Y, Direction.X);
        }
        

    }
}
