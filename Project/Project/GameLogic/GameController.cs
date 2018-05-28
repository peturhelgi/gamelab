using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.Util;


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

        Dictionary<Command, List<Buttons>> _buttons = new Dictionary<Command, List<Buttons>>();
        Dictionary<Command, List<Keys>> _keys = new Dictionary<Command, List<Keys>>();

        protected InputManager Input;
        public GameEngine GameEngine;
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
            _buttons[Command.Interact] = new List<Buttons> { Buttons.X };

            _buttons[Command.ChangeTool] = new List<Buttons> { Buttons.RightShoulder };
            _buttons[Command.Sprint] = new List<Buttons> { Buttons.RightTrigger };
            _buttons[Command.UseTool] = new List<Buttons> { Buttons.B };

            _keys[Command.Up] = new List<Keys> { Keys.OemPeriod, Keys.X };
            _keys[Command.Down] = new List<Keys> { Keys.OemComma, Keys.Z };
            _keys[Command.Right] = new List<Keys> { Keys.Right, Keys.D };
            _keys[Command.Left] = new List<Keys> { Keys.Left, Keys.A };

            _keys[Command.Pause] = new List<Keys> { Keys.Space, Keys.Escape };

            _keys[Command.Jump] = new List<Keys> { Keys.Up, Keys.W };
            _keys[Command.Interact] = new List<Keys> { Keys.Down, Keys.S };
            _keys[Command.UseTool] = new List<Keys> { Keys.Down, Keys.S };

            _keys[Command.ChangeTool] = new List<Keys> { Keys.D1, Keys.D2 };
            _keys[Command.Sprint] = new List<Keys> { Keys.RightShift, Keys.LeftShift };

            _maxNumPlayers = 2;

            GameEngine = gameEngine;
            Camera = camera;


            Input = new InputManager(_maxNumPlayers);
            _direction = 1;

            DebugView = false;
        }

        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse(Mouse.GetState());
            Input.Update();

            for (int i = 0; i < _maxNumPlayers; ++i)
            {
                HandleGamePad(i);
            }

            HandleKeyboard();
            SetCamera();
            GameEngine.Update();           
        }

        private void SetCamera()
        {
            Rectangle _frame = new Rectangle(
                Camera.FocusBox.Location,
                Camera.MinBox.ToPoint());
            float leftMost = float.MaxValue,
                rightMost = float.MinValue,
                topMost = float.MaxValue,
                bottomMost = float.MinValue;

            List<AxisAllignedBoundingBox> attentions = GameEngine.GetAttentions();
            foreach (AxisAllignedBoundingBox a in attentions)
            {
                if (a == null)
                {
                    continue;
                }
                leftMost = Math.Min(leftMost, a.Min.X);
                rightMost = Math.Max(rightMost, a.Max.X);
                topMost = Math.Min(topMost, a.Min.Y);
                bottomMost = Math.Max(bottomMost, a.Max.Y);
            }

            float width = Math.Max(rightMost - leftMost, Camera.MinBox.X);
            float height = Math.Max(bottomMost - topMost, Camera.MinBox.Y);

            if(_frame.Left < leftMost && rightMost < _frame.Right)
            {
                leftMost = _frame.Left;
                rightMost = _frame.Right;
            }
            if(_frame.Top < topMost && bottomMost < _frame.Bottom)
            {
                topMost = _frame.Top;
                bottomMost = _frame.Bottom;
            }
            if (rightMost > _frame.Right)
            {
                leftMost = rightMost - width;
            }
            if (bottomMost > _frame.Bottom)
            {
                topMost = bottomMost - height;
            }
            Camera.SetCameraToRectangle(
                new Rectangle(
                    (int)leftMost,
                    (int)topMost,
                    (int)width,
                    (int)height)
            );
        }

        private void HandleMouse(MouseState ms)
        {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                MyDebugger.WriteLine(ms.Position.X);
                MyDebugger.WriteLine(ms.Position.Y);
            }
        }

        private void HandleKeyboard()
        {

            // START Handle GameAction
            MyDebugger.IsActive = Input.KeyDown(Keys.P);
            GameManager.RenderDark = Input.KeyUp(Keys.L);

            bool running = false;            
            for(int player = 0; player < _maxNumPlayers; ++player)
            {
                running = Input.KeyDown(_keys[Command.Sprint][player]);
                // last parameter is the encoding for the direction the miner is walking/running in
                if (Input.KeyPressed(_keys[Command.Interact][player]))
                {
                    GameEngine.HandleInput(player, GameEngine.GameAction.interact, 0);
                    GameEngine.HandleInput(player, GameEngine.GameAction.use_tool, 0);
                }

                if (Input.KeyDown(_keys[Command.Jump][player]))
                    GameEngine.HandleInput(player, GameEngine.GameAction.jump, 0);
                if (Input.KeyDown(_keys[Command.Up][player]))
                    GameEngine.HandleInput(player, GameEngine.GameAction.climb, -1);
                if (Input.KeyDown(_keys[Command.Down][player]))
                    GameEngine.HandleInput(player, GameEngine.GameAction.climb, 1);

                if (Input.KeyDown(_keys[Command.Left][player]))
                {
                    GameEngine.HandleInput(player,
                        running
                        ? GameEngine.GameAction.run
                        : GameEngine.GameAction.walk, -1);
                }
                else if (Input.KeyDown(_keys[Command.Right][player]))
                {
                    GameEngine.HandleInput(player,
                        running
                        ? GameEngine.GameAction.run
                        : GameEngine.GameAction.walk, 1);
                }

                if (Input.KeyPressed(_keys[Command.ChangeTool][player]))
                    GameEngine.HandleInput(player, GameEngine.GameAction.change_tool, 0);
            }
            // END Handle GameAction      
        }

        private void HandleGamePad(int player)
        {
            if (!Input.IsConnected(player))
            {
                return;
            }

            // START Handle GameAction
            // last parameter is the encoding for the direction the miner is walking/running in

            if (Input.ButtonDown(player, _buttons[Command.Jump]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.jump, 0);
            }
            if (Input.LeftThumb(player).Y < -0.5
                || Input.ButtonDown(player, Buttons.DPadDown))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.climb, 1);
            }
            if (Input.LeftThumb(player).Y > 0.5
                || Input.ButtonDown(player, Buttons.DPadUp))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.climb, -1);
            }

            if (Input.LeftThumb(player).X > 0.5f
                || Input.ButtonDown(player, Buttons.DPadRight))
            {
                _direction = 1;
            }

            if (Input.LeftThumb(player).X < -0.5
                || Input.ButtonDown(player, Buttons.DPadLeft))
            {
                _direction = -1;
            }

            if (Math.Abs(Input.LeftThumb(player).X) > 0.65f)
            {
                GameEngine.HandleInput(player,
                   Input.ButtonUp(player, _buttons[Command.Sprint])
                    ? GameEngine.GameAction.walk
                    : GameEngine.GameAction.run, _direction);
            }
            float x = Input.LeftThumb(player).X,
                y = Input.LeftThumb(player).Y;

            if (x * x + y * y >= 0.5f)
            {
                // Atan2 returns a value -PI < theta <= PI
                GameEngine.HandleInput(player, GameEngine.GameAction.look,
                    (float)Math.Atan2(-y, x));
            }

            if (Input.ButtonPressed(player, _buttons[Command.Interact]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.interact, 0);
            }

            if (Input.ButtonDown(player, _buttons[Command.UseTool]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.use_tool, 0);
            }

            if (Input.ButtonPressed(player, _buttons[Command.ChangeTool]))
            {
                GameEngine.HandleInput(player, GameEngine.GameAction.change_tool, 0);
            }
        }
    }
}
