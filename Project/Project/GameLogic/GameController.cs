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
            ChangeTool,
            Down,
            Interact,
            Jump,
            Left,
            Pause,
            Right,
            Sprint,
            Up,
            UseTool
        }
        enum Direction
        {
            Up = -1,
            Down = 1,
            Left = Up,
            Right = Down
        }

        Dictionary<Command, List<Buttons>> _buttons =
            new Dictionary<Command, List<Buttons>>();

        Dictionary<Command, List<Keys>> _keys = 
            new Dictionary<Command, List<Keys>>();

        public GameEngine GameEngine;
        private KeyboardState _oldKeyboardState, _newKeyboardState;
        private List<GamePadState> _oldPadStates, _newPadStates;
        public Camera Camera;
        public bool DebugView { private set; get; }

        int _maxNumPlayers, _direction;

        public GameController(GameEngine gameEngine, Camera camera)
        {
            _buttons[Command.Up] = new List<Buttons> { Buttons.DPadUp};
            _buttons[Command.Down] = new List<Buttons> { Buttons.DPadDown };
            _buttons[Command.Right] = new List<Buttons> { Buttons.DPadRight};
            _buttons[Command.Left] = new List<Buttons> { Buttons.DPadLeft };

            _buttons[Command.Pause] = new List<Buttons> { Buttons.Start };

            _buttons[Command.Jump] = new List<Buttons> { Buttons.A };
            _buttons[Command.Interact] = new List<Buttons> { Buttons.X };

            _buttons[Command.ChangeTool] = new List<Buttons> { Buttons.RightShoulder };
            _buttons[Command.Sprint] = new List<Buttons> { Buttons.RightTrigger };
            _buttons[Command.UseTool] = new List<Buttons> { Buttons.B };

            _keys[Command.Up] = new List<Keys> { Keys.OemComma, Keys.Z};
            _keys[Command.Down] = new List<Keys> { Keys.OemPeriod, Keys.X };
            _keys[Command.Right] = new List<Keys> { Keys.Right, Keys.D };
            _keys[Command.Left] = new List<Keys> { Keys.Left, Keys.A };
            _keys[Command.Sprint] = new List<Keys> { Keys.RightShift, Keys.LeftShift };

            _keys[Command.Jump] = new List<Keys> { Keys.Up, Keys.W };
            _keys[Command.Pause] = new List<Keys> { Keys.Space, Keys.Space };

            _keys[Command.ChangeTool] = new List<Keys> { Keys.D1, Keys.D2 };

            _keys[Command.Interact] = new List<Keys> {Keys.Down, Keys.S  };
            _keys[Command.UseTool] = new List<Keys> { Keys.Down, Keys.S };

            _maxNumPlayers = 2;

            GameEngine = gameEngine;
            Camera = camera;
            _oldPadStates = new List<GamePadState>();
            _newPadStates = new List<GamePadState>();

            _oldKeyboardState = Keyboard.GetState();
            _newKeyboardState = Keyboard.GetState();
            for (int i = 0; i < _maxNumPlayers; ++i)
            {
                _oldPadStates.Add(GamePad.GetState(i));
                _newPadStates.Add(GamePad.GetState(i));
            }

            _direction = (int)Direction.Right;
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
            _oldKeyboardState = _newKeyboardState;
            _newKeyboardState = Keyboard.GetState();
            HandleKeyboard();

            GameEngine.Update();

            // Handle the Camera
            AxisAllignedBoundingBox frame = new AxisAllignedBoundingBox(
                new Vector2(float.MaxValue), new Vector2(float.MinValue));
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

        private bool ButtonPressed(int player, Command command)
        {
            if (!(_oldPadStates[player].IsConnected
                && _newPadStates[player].IsConnected))
            {
                return false;
            }
            foreach (var button in _buttons[command])
            {
                if (_oldPadStates[player].IsButtonUp(button)
                    && _newPadStates[player].IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ButtonDown(int player, Command command)
        {
            if (!_newPadStates[player].IsConnected)
            {
                return false;
            }
            foreach (var button in _buttons[command])
            {
                if (_newPadStates[player].IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ButtonReleased(int player, Command command)
        {
            if (!(_oldPadStates[player].IsConnected
                && _newPadStates[player].IsConnected))
            {
                return false;
            }

            foreach (var button in _buttons[command])
            {
                if (_oldPadStates[player].IsButtonDown(button)
                    && _newPadStates[player].IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }


        private bool ButtonUp(int player, Command command)
        {
            foreach (var button in _buttons[command])
            {
                if (_newPadStates[player].IsButtonDown(button))
                {
                    return false;
                }
            }
            return true;
        }

        private bool KeyUp(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_newKeyboardState.IsKeyDown(key))
                {
                    return false;
                }
            }
            return true;
        }

        private bool KeyDown(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_newKeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        private bool KeyPressed(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_oldKeyboardState.IsKeyUp(key)
                    && _newKeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        private bool KeyReleased(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_oldKeyboardState.IsKeyDown(key)
                    && _newKeyboardState.IsKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        private void HandleKeyboard()
        {
            // START Handle GameAction
            MyDebugger.IsActive = KeyDown(Keys.P);
            GameManager.RenderDark = KeyUp(Keys.L);

            for(int player = 0; player < _maxNumPlayers; ++player)
            {
                bool running = KeyDown(_keys[Command.Sprint][player]);
                // last parameter is the encoding for the direction the miner is walking/running in
                if (KeyPressed(_keys[Command.Interact][player]))
                {
                    GameEngine.HandleInput(player, GameEngine.Action.interact, 0);
                    GameEngine.HandleInput(player, GameEngine.Action.use_tool, 0);
                }

                if (KeyDown(_keys[Command.Jump][player]))
                {
                    GameEngine.HandleInput(player, GameEngine.Action.jump, 0);
                }
                if (KeyDown(_keys[Command.Up][player]))
                {
                    GameEngine.HandleInput(
                        player, 
                        GameEngine.Action.climb,
                        (int)Direction.Up);
                    
                }
                if (KeyDown(_keys[Command.Down][player]))
                {
                    GameEngine.HandleInput(
                        player, 
                        GameEngine.Action.climb, 
                        (int)Direction.Down);
                }

                if (KeyDown(_keys[Command.Left][player]))
                {
                    GameEngine.HandleInput(
                        player,
                        running
                        ? GameEngine.Action.run
                        : GameEngine.Action.walk,
                        (int)Direction.Left);
                }

                if (KeyDown(_keys[Command.Right][player]))
                {
                    GameEngine.HandleInput(
                        player,
                        running
                        ? GameEngine.Action.run
                        : GameEngine.Action.walk, 
                        (int)Direction.Right);
                }

                if (KeyPressed(_keys[Command.ChangeTool][player]))
                    GameEngine.HandleInput(
                        player, 
                        GameEngine.Action.change_tool, 0);
            }
        }

        private void HandleGamePad(int player)
        {
            if (!_newPadStates[player].IsConnected)
            {
                return;
            }

            var leftThumb = _newPadStates[player].ThumbSticks.Left;
            var rightThumb = _newPadStates[player].ThumbSticks.Right;

            // START Handle GameAction
            // last parameter is the encoding for the direction the miner is walking/running in
            if (leftThumb.Y < -0.5 || ButtonDown(player, Command.Down))
            {
                GameEngine.HandleInput(player, GameEngine.Action.climb, 1);
            }

            if (leftThumb.Y > 0.5 || ButtonDown(player, Command.Up))
            {
                GameEngine.HandleInput(player, GameEngine.Action.climb, -1);
            }
            bool moving = false;
            if (leftThumb.X > 0.5f || ButtonDown(player, Command.Right))
            {
                _direction = 1;
                moving = true;
            }

            if (leftThumb.X < -0.5 || ButtonDown(player, Command.Left))
            {
                _direction = -1;
                moving = true;
            }

            if (moving)
            {
                GameEngine.HandleInput(player,
                    ButtonUp(player, Command.Sprint)
                    ? GameEngine.Action.walk
                    : GameEngine.Action.run, _direction);
            }
            float x = rightThumb.X, y = rightThumb.Y;

            if (x * x + y * y >= 0.5f)
            {
                // Atan2 returns a value -PI < theta <= PI
                GameEngine.HandleInput(player, GameEngine.Action.look,
                    (float)Math.Atan2(-y, x));
                return;
            }

            if (ButtonPressed(player, Command.Interact))
            {
                GameEngine.HandleInput(player, GameEngine.Action.interact, 0);
                return;
            }

            if (ButtonDown(player, Command.UseTool))
            {
                GameEngine.HandleInput(player, GameEngine.Action.use_tool, 0);
                return;
            }

            if (ButtonPressed(player, Command.ChangeTool))
            {
                GameEngine.HandleInput(player, GameEngine.Action.change_tool, 0);
                return;
            }

            if (ButtonDown(player, Command.Jump))
            {
                GameEngine.HandleInput(player, GameEngine.Action.jump, 0);
                return;
            }
        }
    }
}
