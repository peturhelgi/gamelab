using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Util;


namespace TheGreatEscape.GameLogic
{
    class GameController
    {
        public GameEngine GameEngine;
        private KeyboardState _oldKeyboardState;
        private GamePadState _oldPlayerOneState;
        private GamePadState _oldPlayerTwoState;
        public Camera Camera;
        public bool DebugView { private set; get; }

        int _maxNumPlayers;

        public GameController(GameEngine gameEngine, Camera camera)
        {
            GameEngine = gameEngine;
            Camera = camera;
            _oldKeyboardState = Keyboard.GetState();
            _oldPlayerOneState = GamePad.GetState(PlayerIndex.One);
            _oldPlayerTwoState = GamePad.GetState(PlayerIndex.Two);
            _maxNumPlayers = 2;
            DebugView = false;
        }

        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse(Mouse.GetState());

            for (int i = 0; i < _maxNumPlayers; ++i)
            {
                HandleGamePad(GamePad.GetState(i), i);
            }

            HandleKeyboard(Keyboard.GetState());

            GameEngine.Update();

            // Handle the Camera
            AxisAllignedBoundingBox frame = new AxisAllignedBoundingBox(new Vector2(float.MaxValue), new Vector2(float.MinValue));
            List<AxisAllignedBoundingBox> attentions = GameEngine.GetAttentions();
            foreach (AxisAllignedBoundingBox a in attentions)
            {
                if (a == null)
                {
                    continue;
                }
                frame.Min = Vector2.Min(frame.Min, a.Min);
                frame.Max = Vector2.Max(frame.Max, a.Max);
            }
            Camera.SetCameraToRectangle(
                new Rectangle(
                    (int)frame.Min.X,
                    (int)frame.Min.Y,
                    (int)(frame.Max.X - frame.Min.X),
                    (int)(frame.Max.Y - frame.Min.Y)));
        }

        private void HandleMouse(MouseState ms)
        {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                MyDebugger.WriteLine(ms.Position.X);
                MyDebugger.WriteLine(ms.Position.Y);
            }
        }

        private void HandleKeyboard(KeyboardState state)
        {

            // START Handle GameAction
            MyDebugger.IsActive = state.IsKeyDown(Keys.P);
            GameManager.RenderDark = state.IsKeyUp(Keys.L);

            // Player 1
            if (_maxNumPlayers > 0)
            {
                // last parameter is the encoding for the direction the miner is walking/running in
                if (state.IsKeyDown(Keys.Right)) GameEngine.HandleInput(0, GameEngine.GameAction.walk, 1);
                if (state.IsKeyDown(Keys.Left)) GameEngine.HandleInput(0, GameEngine.GameAction.walk, -1);
                if (state.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Down)) GameEngine.HandleInput(0, GameEngine.GameAction.interact, 0);

                if (state.IsKeyDown(Keys.Up)) GameEngine.HandleInput(0, GameEngine.GameAction.jump, 0);
                if (state.IsKeyDown(Keys.Z)) GameEngine.HandleInput(0, GameEngine.GameAction.climb_up, 0);
                if (state.IsKeyDown(Keys.X)) GameEngine.HandleInput(0, GameEngine.GameAction.climb_down, 0);


                if (state.IsKeyDown(Keys.RightShift) && state.IsKeyDown(Keys.Right))
                    GameEngine.HandleInput(0, GameEngine.GameAction.run, 1);
                if (state.IsKeyDown(Keys.RightShift) && state.IsKeyDown(Keys.Left))
                    GameEngine.HandleInput(0, GameEngine.GameAction.run, -1);

                if (state.IsKeyDown(Keys.D1) && !_oldKeyboardState.IsKeyDown(Keys.D1))
                    GameEngine.HandleInput(0, GameEngine.GameAction.change_tool, 0);

            }

            // Player 2
            if (_maxNumPlayers > 1)
            {
                if (state.IsKeyDown(Keys.D)) GameEngine.HandleInput(1, GameEngine.GameAction.walk, 1);
                if (state.IsKeyDown(Keys.A)) GameEngine.HandleInput(1, GameEngine.GameAction.walk, -1);
                if (state.IsKeyDown(Keys.S) && !_oldKeyboardState.IsKeyDown(Keys.S)) GameEngine.HandleInput(1, GameEngine.GameAction.interact, 0);
                if (state.IsKeyDown(Keys.W)) GameEngine.HandleInput(1, GameEngine.GameAction.jump, 0);

                if (state.IsKeyDown(Keys.LeftShift) && state.IsKeyDown(Keys.D))
                    GameEngine.HandleInput(1, GameEngine.GameAction.run, 1);
                if (state.IsKeyDown(Keys.LeftShift) && state.IsKeyDown(Keys.A))
                    GameEngine.HandleInput(1, GameEngine.GameAction.run, -1);

                if (state.IsKeyDown(Keys.D2) && !_oldKeyboardState.IsKeyDown(Keys.D2))
                    GameEngine.HandleInput(1, GameEngine.GameAction.change_tool, 0);
            }
            // END Handle GameAction      
            _oldKeyboardState = state;
        }

        private void HandleGamePad(GamePadState gs, int player)
        {
            if (!gs.IsConnected)
            {
                return;
            }

            // START Handle GameAction
            // last parameter is the encoding for the direction the miner is walking/running in
            if (gs.ThumbSticks.Left.X > 0.5f) GameEngine.HandleInput(player, GameEngine.GameAction.walk, 1);
            if (gs.ThumbSticks.Left.X < -0.5) GameEngine.HandleInput(player, GameEngine.GameAction.walk, -1);
            if (gs.ThumbSticks.Left.Y < -0.5) GameEngine.HandleInput(player, GameEngine.GameAction.climb_down, 0);
            if (gs.ThumbSticks.Left.Y > 0.5) GameEngine.HandleInput(player, GameEngine.GameAction.climb_up, 0);

            if (gs.ThumbSticks.Left.X > 0.5f)
            {
                GameEngine.HandleInput(player,
                    (gs.IsButtonUp(Buttons.LeftStick) && gs.IsButtonUp(Buttons.LeftTrigger)) ? GameEngine.GameAction.walk
                    : GameEngine.GameAction.run, 1);
            }
            if (gs.ThumbSticks.Left.X < -0.5)
            {
                GameEngine.HandleInput(player,
                    gs.IsButtonUp(Buttons.LeftStick) && gs.IsButtonUp(Buttons.LeftTrigger) ? GameEngine.GameAction.walk
                    : GameEngine.GameAction.run, -1);
            }

            float x = gs.ThumbSticks.Right.X,
                y = gs.ThumbSticks.Right.Y;
            if (x * x + y * y >= 0.5f)
            {
                // Atan2 returns a value -PI < theta <= PI
                GameEngine.HandleInput(player, GameEngine.GameAction.look,
                    (float)Math.Atan2(-y, x));
            }

            if (player == 0)
            {
                if (gs.IsButtonDown(Buttons.RightTrigger) && !_oldPlayerOneState.IsButtonDown(Buttons.RightTrigger))
                    GameEngine.HandleInput(player, GameEngine.GameAction.interact, 0);
                if (gs.IsButtonDown(Buttons.RightShoulder) && !_oldPlayerOneState.IsButtonDown(Buttons.RightShoulder))
                    GameEngine.HandleInput(player, GameEngine.GameAction.change_tool, 0);
            }
            else
            {
                if (gs.IsButtonDown(Buttons.RightTrigger) && !_oldPlayerTwoState.IsButtonDown(Buttons.RightTrigger))
                    GameEngine.HandleInput(player, GameEngine.GameAction.interact, 0);
                if (gs.IsButtonDown(Buttons.RightShoulder) && !_oldPlayerTwoState.IsButtonDown(Buttons.RightShoulder))
                    GameEngine.HandleInput(player, GameEngine.GameAction.change_tool, 0);
            }
            if (gs.IsButtonDown(Buttons.A)) GameEngine.HandleInput(player, GameEngine.GameAction.jump, 0);
            // END Handle GameAction

            if (player == 0) _oldPlayerOneState = gs;
            else _oldPlayerTwoState = gs;
        }

    }
}
