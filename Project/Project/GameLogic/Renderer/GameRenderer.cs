using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;
using static TheGreatEscape.GameLogic.GameState;

namespace TheGreatEscape.GameLogic.Renderer
{
    class GameRenderer
    {
        GraphicsDevice _graphicsDevice;
        GameState _gameState;
        SpriteBatch _spriteBatch;
        Texture2D _debugBox;
        Effect _lightingEffect;
        RenderTarget2D _renderTargetScene;
        RenderTarget2D _renderTargetLights;
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

            _renderTargetScene = new RenderTarget2D(_graphicsDevice,
                _graphicsDevice.PresentationParameters.BackBufferWidth,
                _graphicsDevice.PresentationParameters.BackBufferHeight);


            _lightRenderer = new LightRenderer(_graphicsDevice, content);

        }

        public void Draw(GameTime gameTime, int width, int height, Mode mode, Camera camera)
        {

            List<Light> lights = new List<Light>();
            Texture2D background = _gameState.GetBackground();

            // Render the scene
            _graphicsDevice.SetRenderTarget(_renderTargetScene);
            _graphicsDevice.Clear(Color.Gray);
            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                mode == Mode.DebugView ? BlendState.Opaque : null,
                null, null, null, null, camera.view);

            _spriteBatch.Draw(background, camera.GetCameraRectangle(background.Width, background.Height), Color.White);


            foreach (GameObject obj in _gameState.GetAll())
            {
                if (!obj.Active || !obj.Visible)
                {
                    continue;
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
                    if (obj is RockHook)
                    {
                        HangingRope hangingRope = (obj as RockHook).GetRope();
                        _spriteBatch.Draw(
                           mode == Mode.DebugView ? _debugBox : hangingRope.SecondTexture,
                           new Rectangle(
                               (int)hangingRope.Position.X,
                               (int)hangingRope.Position.Y,
                               (int)hangingRope.SpriteSize.X,
                               (int)hangingRope.SpriteSize.Y),
                           Color.White);
                    }
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

                if (obj.Lights is List<Light>)
                {
                    lights.AddRange(obj.Lights);
                }
            }

            _spriteBatch.End();


            foreach (Miner miner in _gameState.GetActors())
            {
                if (miner.Active)
                {
                    miner.CurrMotion.Update(gameTime);
                }
            }

            // Render the Lights
            _renderTargetLights = _lightRenderer.Draw(gameTime, width, height, lights, camera.view);

            _graphicsDevice.SetRenderTarget(null);

            _graphicsDevice.Clear(Color.Black);

            _lightingEffect.CurrentTechnique.Passes[0].Apply();

            if (GameManager.RenderDark)
            {
                _spriteBatch.Begin(effect: _lightingEffect);
            }
            else
            {
                _spriteBatch.Begin();
            }

            _lightingEffect.Parameters["LightMask"].SetValue(_renderTargetLights);


            _spriteBatch.Draw(_renderTargetScene, new Rectangle(0, 0, width, height), Color.White);

            _spriteBatch.End();

            // Draw the UI interface that displays the number of miners still available in the game
            _spriteBatch.Begin();
            int i = 0;
            foreach (var tool in _gameState.resources)
            {
                Enum.TryParse(tool.Key, out ExistingTools et);
                Texture2D toolTexture = _gameState.Tools[et].GetTexture();
                Vector2 textureSize = new Vector2(toolTexture.Width / 20, toolTexture.Height / 20);

                _spriteBatch.Draw(toolTexture, new Rectangle(100, 100 + i, (int)textureSize.X, (int)textureSize.Y), Color.White);
                _spriteBatch.DrawString(_gameState.GameFont, tool.Value.ToString(), new Vector2(100 + textureSize.X + 50,  100 + i + textureSize.Y / 3)
                    , Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                i += (int)textureSize.Y;
            }

            _spriteBatch.End();

        }
    }
}
