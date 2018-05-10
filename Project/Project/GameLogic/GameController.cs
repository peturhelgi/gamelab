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

        enum Command
        {
            Up,
            Down,
            Left,
            Right,
            Jump,
            ChangeTool,
            Pause,
            Sprint,
            UseTool,
            Interact
        }

        Dictionary<Command, List<Buttons>> _buttons = new Dictionary<Command, List<Buttons>>();

        public GameEngine GameEngine;
        private KeyboardState _oldKeyboardState;
        private List<GamePadState> _oldPadStates, _newPadStates;
        public Camera Camera;
        public bool DebugView { private set; get; }

        int _maxNumPlayers, _direction;

        public GameController(GameEngine gameEngine, Camera camera)
        {
            _buttons[Command.Up] = new List<Buttons> { Buttons.DPadUp, Buttons.LeftThumbstickUp };
            _buttons[Command.Down] = new List<Buttons> { Buttons.DPadDown, Buttons.LeftThumbstickDown };
            _buttons[Command.Right] = new List<Buttons> { Buttons.DPadRight, Buttons.LeftThumbstickRight };
            _buttons[Command.Left] = new List<Buttons> { Buttons.DPadLeft, Buttons.LeftThumbstickLeft };

            _buttons[Command.Pause] = new List<Buttons> { Buttons.Start };

            _buttons[Command.Jump] = new List<Buttons> { Buttons.A };
            _buttons[Command.Interact] = new List<Buttons> { Buttons.X, Buttons.B};

            _buttons[Command.ChangeTool] = new List<Buttons> { Buttons.RightShoulder };
            _buttons[Command.Sprint] = new List<Buttons> { Buttons.LeftTrigger };
            _buttons[Command.UseTool] = new List<Buttons> { Buttons.RightTrigger };

            _maxNumPlayers = 2;

            GameEngine = gameEngine;
            Camera = camera;
            _oldPadStates = new List<GamePadState>();
            _newPadStates = new List<GamePadState>();

            _oldKeyboardState = Keyboard.GetState();
            for (int i = 0; i < _maxNumPlayers; ++i)
            {
                _oldPadStates.Add(GamePad.GetState(i));
                _newPadStates.Add(GamePad.GetState(i));
            }

            _direction = 1;

            DebugView = false;
        }

        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse(Mouse.GetState());

            for (int i = 0; i < _maxNumPlayers; ++i)
            {
                _oldPadStates[i] = _newPadStates[i];
                _newPadStates[i] = GamePad.GetState(i);
                HandleGamePad(i);
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

        private bool ButtonPressed(GamePadState old, GamePadState curr, List<Buttons> buttons)
        {
            foreach (var button in buttons)
            {
                if (old.IsButtonUp(button) && curr.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ButtonDown(GamePadState curr, List<Buttons> buttons)
        {
            foreach (var button in buttons)
            {
                if (curr.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ButtonReleased(GamePadState old, GamePadState curr, List<Buttons> buttons)
        {
            foreach (var button in buttons)
            {
                if (old.IsButtonDown(button) && curr.IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }


        private bool ButtonUp(GamePadState curr, List<Buttons> buttons)
        {
            foreach (var button in buttons)
            {
                if (curr.IsButtonDown(button))
                {
                    return false;
                }
            }
            return true;
        }
        private void HandleKeyboard(KeyboardState state)
        {

            // START Handle GameAction
            MyDebugger.IsActive = state.IsKeyDown(Keys.P);
            GameManager.RenderDark = state.IsKeyUp(Keys.L);

            bool running = false;
            // Player 1
            if (_maxNumPlayers > 0)
            {
                running = state.IsKeyDown(Keys.RightShift);
                // last parameter is the encoding for the direction the miner is walking/running in
                if (state.IsKeyDown(Keys.Down) && !_oldKeyboardState.IsKeyDown(Keys.Down))
                {
                    GameEngine.HandleInput(0, GameEngine.GameAction.interact, 0);
                    GameEngine.HandleInput(0, GameEngine.GameAction.use_tool, 0);
                }

                if (state.IsKeyDown(Keys.Up)) GameEngine.HandleInput(0, GameEngine.GameAction.jump, 0);
                if (state.IsKeyDown(Keys.OemComma)) GameEngine.HandleInput(0, GameEngine.GameAction.climb, -1);
                if (state.IsKeyDown(Keys.OemPeriod)) GameEngine.HandleInput(0, GameEngine.GameAction.climb, 1);

                if (state.IsKeyDown(Keys.Left))
                {
                    GameEngine.HandleInput(0,
                        running
                        ? GameEngine.GameAction.run
                        : GameEngine.GameAction.walk, -1);
                }

                if (state.IsKeyDown(Keys.Right))
                {
                    GameEngine.HandleInput(0,
                        running
                        ? GameEngine.GameAction.run
                        : GameEngine.GameAction.walk, 1);
                }

                if (state.IsKeyDown(Keys.D1) && !_oldKeyboardState.IsKeyDown(Keys.D1))
                    GameEngine.HandleInput(0, GameEngine.GameAction.change_tool, 0);
            }

            // Player 2
            if (_maxNumPlayers > 1)
            {
                running = state.IsKeyDown(Keys.LeftShift);

                if (state.IsKeyDown(Keys.S) && !_oldKeyboardState.IsKeyDown(Keys.S))
                {
                    GameEngine.HandleInput(1, GameEngine.GameAction.interact, 0);
                    GameEngine.HandleInput(1, GameEngine.GameAction.use_tool, 0);
                }
                if (state.IsKeyDown(Keys.W))
                {
                    GameEngine.HandleInput(1, GameEngine.GameAction.jump, 0);
                }

                if (state.IsKeyDown(Keys.D))
                {
                    GameEngine.HandleInput(1,
                        running
                        ? GameEngine.GameAction.run
                        : GameEngine.GameAction.walk, 1);
                }

                if (state.IsKeyDown(Keys.A))
                {
                    GameEngine.HandleInput(1,
                        running
                        ? GameEngine.GameAction.run
                        : GameEngine.GameAction.walk, -1);
                }

                if (state.IsKeyDown(Keys.D2) && !_oldKeyboardState.IsKeyDown(Keys.D2))
                    GameEngine.HandleInput(1, GameEngine.GameAction.change_tool, 0);

                if (state.IsKeyDown(Keys.Z))
                {
                    GameEngine.HandleInput(1, GameEngine.GameAction.climb, -1);
                }
                if (state.IsKeyDown(Keys.X))
                {
                    GameEngine.HandleInput(1, GameEngine.GameAction.climb, 1);
                }
            }
            // END Handle GameAction      
            _oldKeyboardState = state;
        }

        private void HandleGamePad(int player)
        {
            if (!_newPadStates[player].IsConnected)
            {
                return;
            }

            // START Handle GameAction
            // last parameter is the encoding for the direction the miner is walking/running in
            if (_newPadStates[player].ThumbSticks.Left.Y < -0.5
                || _newPadStates[player].IsButtonDown(Buttons.DPadDown))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.climb, 1);
            }
            if (_newPadStates[player].ThumbSticks.Left.Y > 0.5
                || _newPadStates[player].IsButtonDown(Buttons.DPadUp))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.climb, -1);
            }

            if (_newPadStates[player].ThumbSticks.Left.X > 0.5f
                || _newPadStates[player].IsButtonDown(Buttons.DPadRight))
            {
                _direction = 1;
            }

            if (_newPadStates[player].ThumbSticks.Left.X < -0.5
                || _newPadStates[player].IsButtonDown(Buttons.DPadLeft))
            {
                _direction = -1;
            }

            if (Math.Abs(_newPadStates[player].ThumbSticks.Left.X) > 0.5f)
            {
                GameEngine.HandleInput(player,
                    ButtonUp(_newPadStates[player], _buttons[Command.Left])
                    && ButtonUp(_newPadStates[player], _buttons[Command.Sprint])
                    ? GameEngine.GameAction.walk
                    : GameEngine.GameAction.run, _direction);
            }
            float x = _newPadStates[player].ThumbSticks.Right.X,
                y = _newPadStates[player].ThumbSticks.Right.Y;

            if (x * x + y * y >= 0.5f)
            {
                // Atan2 returns a value -PI < theta <= PI
                GameEngine.HandleInput(player, GameEngine.GameAction.look,
                    (float)Math.Atan2(-y, x));
            }

            if (ButtonPressed(_oldPadStates[player], _newPadStates[player], _buttons[Command.Interact]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.interact, 0);
            }

            if (ButtonPressed(_oldPadStates[player], _newPadStates[player], _buttons[Command.UseTool]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.use_tool, 0);
            }

            if (ButtonPressed(_oldPadStates[player], _newPadStates[player], _buttons[Command.ChangeTool]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.change_tool, 0);
            }

            if (ButtonPressed(_oldPadStates[player], _newPadStates[player], _buttons[Command.Jump]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.jump, 0);
            }
        }
    }
}
