using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheGreatEscape.Util
{
    public class InputManager
    {
        List<GamePadState> _oldPadStates;
        List<GamePadState> _currPadStates;
        KeyboardState _oldKbState;
        KeyboardState _currKbState;
        int _num_pads;

        public InputManager(int num_players)
        {
            _oldPadStates = new List<GamePadState>();
            _currPadStates = new List<GamePadState>();
            _num_pads = num_players;
            for(int i = 0; i < num_players; ++i)
            {
                _oldPadStates.Add(GamePad.GetState(i));
                _currPadStates.Add(GamePad.GetState(i));
            }
            _oldKbState = Keyboard.GetState();
            _currKbState = Keyboard.GetState();
        }

        public void Reset()
        {
            for (int i = 0; i < _num_pads; ++i)
            {
                _oldPadStates[i] = GamePad.GetState(i);
                _currPadStates[i] = GamePad.GetState(i);
            }
            _oldKbState = Keyboard.GetState();
            _currKbState = Keyboard.GetState();
        }
        public bool IsConnected(int player)
        {
            return (0 <= player && player < _num_pads
                && _currPadStates[player].IsConnected);
        }

        public void Update()
        {
            for(int i = 0; i < _num_pads; ++i)
            {
                _oldPadStates[i] = _currPadStates[i];
                _currPadStates[i] = GamePad.GetState(i);
            }
            _oldKbState = _currKbState;
            _currKbState = Keyboard.GetState();
        }

        public Vector2 LeftThumb(int player)
        {
            if (player < 0 || player >= _num_pads)
            {
                return Vector2.Zero;
            }
            return _currPadStates[player].ThumbSticks.Left;
        }
        public Vector2 RightThumb(int player)
        {
            if (player < 0 || player >= _num_pads)
            {
                return Vector2.Zero;
            }
            return _currPadStates[player].ThumbSticks.Right;
        }

        public bool ButtonDown(int player, params Buttons[] buttons)
        {            
            if(player < 0 || player >= _num_pads)
            {
                return false;
            }
            foreach (var button in buttons)
            {
                if (_currPadStates[player].IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }


        public bool ButtonDown(int player, List<Buttons> buttons)
        {
            if (player < 0 || player >= _num_pads)
            {
                return false;
            }
            foreach (var button in buttons)
            {
                if (_currPadStates[player].IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }
        public bool ButtonUp(int player, params Buttons[] buttons)
        {
            if (player < 0 || player >= _num_pads)
            {
                return true;
            }
            foreach (var button in buttons)
            {
                if (_currPadStates[player].IsButtonDown(button))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ButtonUp(int player, List<Buttons> buttons)
        {
            if (player < 0 || player >= _num_pads)
            {
                return true;
            }
            foreach (var button in buttons)
            {
                if (_currPadStates[player].IsButtonDown(button))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ButtonPressed(int player, params Buttons[] buttons)
        {
            if (player < 0 || player >= _num_pads)
            {
                return false;
            }
            foreach (var button in buttons)
            {
                if (_currPadStates[player].IsButtonDown(button)
                    && _oldPadStates[player].IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonPressed(int player, List<Buttons> buttons)
        {
            if (player < 0 || player >= _num_pads)
            {
                return false;
            }
            foreach (var button in buttons)
            {
                if (_currPadStates[player].IsButtonDown(button)
                    && _oldPadStates[player].IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonReleased(int player, params Buttons[] buttons)
        {
            if (player < 0 || player >= _num_pads)
            {
                return false;
            }
            foreach (var button in buttons)
            {
                if (_currPadStates[player].IsButtonUp(button)
                    && _oldPadStates[player].IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_oldKbState.IsKeyUp(key)
                    && _currKbState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_oldKbState.IsKeyDown(key)
                    && _currKbState.IsKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_currKbState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyUp(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (_currKbState.IsKeyDown(key))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
