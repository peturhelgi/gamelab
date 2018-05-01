﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;
using System;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Util;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Draw(GameTime gameTime, int width, int height, Mode mode, Matrix camera)
        {

            List<Light> lights = new List<Light>();

            // Render the scene
            _graphicsDevice.SetRenderTarget(_renderTargetScene);
            _graphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin(
                SpriteSortMode.Deferred, 
                mode == Mode.DebugView ? BlendState.Opaque : null, 
                null, null, null, null, camera);
            foreach(GameObject obj in _gameState.GetAll())
            {
                if(obj.Visible)
                {
                    _spriteBatch.Draw(mode == Mode.DebugView ? _debugBox : obj.Texture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.SpriteSize.X, (int)obj.SpriteSize.Y), Color.White);
                }
                if(obj.Lights is List<Light>)
                {
                    lights.AddRange(obj.Lights);
                }
            }
            _spriteBatch.End();

            // Render the Lights
            _renderTargetLights = _lightRenderer.Draw(gameTime, width, height, lights, camera);

            _graphicsDevice.SetRenderTarget(null);

            _graphicsDevice.Clear(Color.Black);

            _lightingEffect.CurrentTechnique.Passes[0].Apply();

            if(GameManager.RenderDark)
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

        }
    }
}
