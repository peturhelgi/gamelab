using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;

namespace TheGreatEscape.GameLogic.Renderer
{
    class GameRenderer
    {
        GraphicsDevice _graphicsDevice;
        GameState _gameState;
        SpriteBatch _spriteBatch;
        Texture2D _debugBox;
        Effect _lightingEffect;
        RenderTarget2D[] _renderTargetScene = new RenderTarget2D[2];
        RenderTarget2D[] _renderTargetLights = new RenderTarget2D[2];
        Texture2D _shaderTexture;
        LightRenderer _lightRenderer;
        public enum Mode { Normal, DebugView }

        public GameRenderer(GraphicsDevice graphicsDevice, GameState gameState, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _gameState = gameState;
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            _debugBox = content.Load<Texture2D>("Sprites/Misc/box");

            _lightingEffect = content.Load<Effect>("Lights/Lighting");

            _renderTargetScene[0] = new RenderTarget2D(_graphicsDevice,
                _graphicsDevice.PresentationParameters.BackBufferWidth,
                _graphicsDevice.PresentationParameters.BackBufferHeight);

            _renderTargetScene[1] = new RenderTarget2D(_graphicsDevice,
                _graphicsDevice.PresentationParameters.BackBufferWidth,
                _graphicsDevice.PresentationParameters.BackBufferHeight);

            _renderTargetLights[0] = new RenderTarget2D(_graphicsDevice,
                _graphicsDevice.PresentationParameters.BackBufferWidth,
                _graphicsDevice.PresentationParameters.BackBufferHeight);

            _renderTargetLights[1] = new RenderTarget2D(_graphicsDevice,
                _graphicsDevice.PresentationParameters.BackBufferWidth,
                _graphicsDevice.PresentationParameters.BackBufferHeight);

            _lightRenderer = new LightRenderer(_graphicsDevice, content);

        }

        public void Draw(GameTime gameTime, int width, int height, Mode mode, params Camera[] camera)
        {
            List<Light> lights = new List<Light>();
            List<Light> upperLights = new List<Light>();
            Texture2D background = _gameState.GetBackground();

            // Render the scene
            for (int t = 0; t < camera.Length; t++)
            {
                _graphicsDevice.SetRenderTarget(_renderTargetScene[t]);

                _graphicsDevice.Clear(Color.Gray);
                _spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    mode == Mode.DebugView ? BlendState.Opaque : null,
                    null, null, null, null, camera[t].view);

                _spriteBatch.Draw(background, camera[t].GetCameraRectangle(background.Width, background.Height), Color.White);

                foreach (GameObject obj in _gameState.GetAll())
                {
                    if (!obj.Active || !obj.Visible)
                    {
                        continue;
                    }
                    if (obj.Lights is List<Light>)
                    {
                        lights.AddRange(obj.Lights);
                    }
                    if (obj is Miner)
                    {
                        Miner m = obj as Miner;
                        Vector2 motionSize = obj.SpriteSize * new Vector2(m.CurrMotion.Scale.X, m.CurrMotion.Scale.Y);
                        Rectangle source = m.CurrMotion.SourceRectangle;
                        Rectangle destination = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)motionSize.X,
                            (int)motionSize.Y);
                        _spriteBatch.Draw(m.CurrMotion.Image, destination, source, Color.White, 0f, Vector2.Zero, m.Orientation, 0f);

                        Tool tool = m.Tool;
                        destination.Width = tool.GetTexture().Width / 20;
                        destination.Height = tool.GetTexture().Height / 20;
                        destination.Y -= 100;
                        destination.X -= (destination.Width - (int)motionSize.X) / 2;
                        _spriteBatch.Draw(tool.GetTexture(), destination, Color.White);
                    }
                    else
                    {
                        if (obj?.Texture != null)
                        {
                            _spriteBatch.Draw(
                                mode == Mode.DebugView ? _debugBox : obj.Texture,
                                new Rectangle(
                                    (int)obj.Position.X,
                                    (int)obj.Position.Y,
                                    (int)obj.SpriteSize.X,
                                    (int)obj.SpriteSize.Y),
                                Color.White);
                        }
                    }

                }
                _spriteBatch.End();

                 _lightRenderer.Draw(gameTime, width, height, lights, camera[t].view, ref _renderTargetLights[t]);
                _graphicsDevice.SetRenderTarget(null);
                //_graphicsDevice.Clear(Color.Black);
                _lightingEffect.CurrentTechnique.Passes[0].Apply();
                if (GameManager.RenderDark)
                {
                    _spriteBatch.Begin(effect: _lightingEffect);
                }
                else
                {
                    _spriteBatch.Begin();
                }

                _lightingEffect.Parameters["LightMask"].SetValue(_renderTargetLights[t]);
                _spriteBatch.Draw(_renderTargetScene[t], new Rectangle(0, t * height, width, height), Color.White);
                _spriteBatch.End();
                lights.Clear();
            }

            foreach (Miner miner in _gameState.GetActors())
            {
                if (miner.Active)
                {
                    miner.CurrMotion.Update(gameTime);
                }
            }

            // Render the Lights
            //_renderTargetLights[0] = _lightRenderer.Draw(gameTime, width, height, lights, camera[0].view);

            //_graphicsDevice.SetRenderTarget(null);
            //_graphicsDevice.Clear(Color.Black);


            //_lightingEffect.CurrentTechnique.Passes[0].Apply();


            //if (GameManager.RenderDark)
            //{
            //    _spriteBatch.Begin(effect: _lightingEffect);
            //}
            //else
            //{
            //    _spriteBatch.Begin();
            //}

            //_lightingEffect.Parameters["LightMask"].SetValue(_renderTargetLights[0]);
            //_spriteBatch.Draw(_renderTargetScene[0], new Rectangle(0, 0 * height, width, height), Color.White);
            ////_renderTargetLights[1] = _lightRenderer.Draw(gameTime, width, height, lights, camera[1].view);
            //_spriteBatch.Draw(_renderTargetScene[1], new Rectangle(0, 1 * height, width, height), Color.White);

            //_spriteBatch.End();
            /*
            _graphicsDevice.SetRenderTarget(_renderTargetScene[1]);
            _renderTargetLights2 = _lightRenderer.Draw(gameTime, width, height, lights, camera[1].view);
            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(Color.Black);
            */

            //if (GameManager.RenderDark)
            //{
            //    _spriteBatch.Begin(effect: _lightingEffect);
            //}
            //else
            //{
            //    _spriteBatch.Begin();
            //}
            //_lightingEffect.Parameters["LightMask"].SetValue(_renderTargetLights[1]);
            //_spriteBatch.Draw(_renderTargetScene[1], new Rectangle(0, 1 * height, width, height), Color.White);

            //_spriteBatch.End();

            // Draw the UI interface that displays the number of miners still available in the game
            _spriteBatch.Begin();
            int i = 0;
            int prevSprite = 0;
            foreach (var miner in _gameState.resources)
            {
                var toolFontSize = _gameState.GameFont.MeasureString(miner.Key + ":  ");
                _spriteBatch.DrawString(_gameState.GameFont, miner.Key + ":", new Vector2(100 + prevSprite, 100 + i)
                    , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_gameState.GameFont, miner.Value.ToString(), new Vector2(100 + prevSprite + toolFontSize.X, 100 + i)
                    , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                i += (int)toolFontSize.Y;
                prevSprite += (int)toolFontSize.X / 4;
            }

            _spriteBatch.End();

        }
    }
}
