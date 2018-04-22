using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;

namespace Project.Controls {
    public class PlayingControls : GameController {
        GameEngine _gameEngine;
        public PlayingControls() { }
        public PlayingControls(Camera camera) : base(camera) { }
        public override void Initialize(params Object[] gameEngines) {
            _gameEngine = (GameEngine)gameEngines[0];
        }

        internal override void HandleUpdate(GameTime gameTime) {
            base.HandleUpdate(gameTime);
            _gameEngine.gameTimeSpan = gameTime.TotalGameTime;
            GamePadState PlayerOneState = GamePad.GetState(PlayerIndex.One);
            if(PlayerOneState.IsConnected) {
                HandleGamePad(PlayerOneState, gameTime);

                // There can only be a second player, if there is a first one
                GamePadState PlayerTwoState = GamePad.GetState(PlayerIndex.Two);
                if(PlayerTwoState.IsConnected) {
                    HandleGamePad(PlayerTwoState, gameTime);
                }
            }

            HandleKeyboard(Keyboard.GetState(), gameTime);
        }

        protected override void HandleKeyboard(KeyboardState state, GameTime gameTime)
        {
            // START Handle camera
            if(state.IsKeyDown(Keys.A)) Camera.HandleAction(Camera.Action.left);
            if(state.IsKeyDown(Keys.D)) Camera.HandleAction(Camera.Action.right);
            if(state.IsKeyDown(Keys.W)) Camera.HandleAction(Camera.Action.up);
            if(state.IsKeyDown(Keys.S)) Camera.HandleAction(Camera.Action.down);
            if(state.IsKeyDown(Keys.Z)) Camera.HandleAction(Camera.Action.zoom_out);
            if(state.IsKeyDown(Keys.X)) Camera.HandleAction(Camera.Action.zoom_in);
            // END Handle camera

            // START Handle GameAction
            if(state.IsKeyDown(Keys.Right)) {
                _gameEngine.HandleInput(
                    0, GameEngine.Action.walk_right, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.Left)) {
                _gameEngine.HandleInput(0, GameEngine.Action.walk_left, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.I)) {
                _gameEngine.HandleInput(0, GameEngine.Action.interact, 0, gameTime);
            }
            if(state.IsKeyDown(Keys.Space)) {
                _gameEngine.HandleInput(0, GameEngine.Action.jump, 0, gameTime);
            }

            // END Handle GameAction
        }
        protected override void HandleGamePad(GamePadState gs, GameTime gameTime)
        {           

            // pressing A makes the character jump
            if(gs.IsButtonDown(Buttons.A))
                _gameEngine.HandleInput(0, GameEngine.Action.jump, 0, gameTime);

            if(gs.IsButtonDown(Buttons.RightTrigger))
                _gameEngine.HandleInput(0, GameEngine.Action.interact, 0, gameTime);

            // Left thumbstick controls
            if(gs.ThumbSticks.Left.X < -0.5f)
                _gameEngine.HandleInput(0, GameEngine.Action.walk_left, 0, gameTime);
            if(gs.ThumbSticks.Left.X > 0.5f)
                _gameEngine.HandleInput(0, GameEngine.Action.walk_right, 0, gameTime);
        }
    }
}
