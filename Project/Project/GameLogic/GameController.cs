using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project.GameLogic.Collision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project.GameLogic
{
    class GameController
    {
        public GameEngine GameEngine;

        public Camera Camera;

        List<GamePadState> _padStates;
        int _numPlayers;


        public GameController(GameEngine gameEngine, Camera camera) {
            GameEngine = gameEngine;
            Camera = camera;
            _padStates = new List<GamePadState>();
        }

        /// <summary>
        /// Pre: GameEngine has loaded its map into its GameState.
        /// </summary>
        public void Initialize()
        { 
            _numPlayers = GameEngine.GameState.GetActors().Count;
            for(int i = 0; i < _numPlayers; ++i)
            {
                _padStates.Add(GamePad.GetState(i));
            }
        }

        /// <summary>
        /// Pre: Initialize() has been called.
        /// </summary>
        /// <param name="gameTime"></param>
        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse( Mouse.GetState());
            for(int i = 0; i < _numPlayers; ++i)
            {
                _padStates[i] = GamePad.GetState(i);
                if(!_padStates[i].IsConnected)
                {
                    break;
                }
                HandleGamePad(_padStates[i], i);
            }

            HandleKeyboard(Keyboard.GetState());

            GameEngine.Update();


            // Handle the Camera
            AxisAllignedBoundingBox frame = new AxisAllignedBoundingBox(new Vector2(float.MaxValue, float.MaxValue), new Vector2(float.MinValue, float.MinValue));
            List<AxisAllignedBoundingBox> attentions = GameEngine.GetAttentions();
            foreach (AxisAllignedBoundingBox a in attentions) {
                frame.Min = Vector2.Min(frame.Min, a.Min);
                frame.Max = Vector2.Max(frame.Max, a.Max);
            }
            Camera.SetCameraToRectangle(new Rectangle((int)frame.Min.X, (int)frame.Min.Y, (int)(frame.Max.X - frame.Min.X), (int)(frame.Max.Y - frame.Min.Y)));
        }

        private void HandleMouse(MouseState ms) {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine(ms.Position.X);
                Debug.WriteLine(ms.Position.Y);
            }
        }

        private void HandleKeyboard(KeyboardState state)
        {
            /*// START Handle camera
            if (state.IsKeyDown(Keys.A)) Camera.HandleAction(Camera.CameraAction.left);
            if (state.IsKeyDown(Keys.D)) Camera.HandleAction(Camera.CameraAction.right);
            if (state.IsKeyDown(Keys.W)) Camera.HandleAction(Camera.CameraAction.up);
            if (state.IsKeyDown(Keys.S)) Camera.HandleAction(Camera.CameraAction.down);
            if (state.IsKeyDown(Keys.Z)) Camera.HandleAction(Camera.CameraAction.zoom_out);
            if (state.IsKeyDown(Keys.X)) Camera.HandleAction(Camera.CameraAction.zoom_in);
            // END Handle camera*/

            // START Handle GameAction
            // Player 1
            if(_numPlayers > 0)
            {
                if(state.IsKeyDown(Keys.Right)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_right, 0);
                if(state.IsKeyDown(Keys.Left)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_left, 0);
                if(state.IsKeyDown(Keys.Down)) GameEngine.HandleInput(0, GameEngine.GameAction.interact, 0);
                if(state.IsKeyDown(Keys.Up)) GameEngine.HandleInput(0, GameEngine.GameAction.jump, 0);
                if(state.IsKeyDown(Keys.OemComma))
                {
                    GameEngine.HandleInput(0, GameEngine.GameAction.look, 0.05f);
                }
                if(state.IsKeyDown(Keys.OemPeriod))
                {
                    GameEngine.HandleInput(0, GameEngine.GameAction.look, -0.05f);
                }
            }


            // Player 2
            if(_numPlayers > 1)
            {
                if(state.IsKeyDown(Keys.D)) GameEngine.HandleInput(1, GameEngine.GameAction.walk_right, 0);
                if(state.IsKeyDown(Keys.A)) GameEngine.HandleInput(1, GameEngine.GameAction.walk_left, 0);
                if(state.IsKeyDown(Keys.S)) GameEngine.HandleInput(1, GameEngine.GameAction.interact, 0);
                if(state.IsKeyDown(Keys.W)) GameEngine.HandleInput(1, GameEngine.GameAction.jump, 0);

                if(state.IsKeyDown(Keys.Z))
                {
                    GameEngine.HandleInput(1, GameEngine.GameAction.look, 0.05f);
                }
                if(state.IsKeyDown(Keys.X))
                {
                    GameEngine.HandleInput(1, GameEngine.GameAction.look, -0.05f);
                }
            }
            // END Handle GameAction            
        }

        private void HandleGamePad(GamePadState gs, int player)
        {
            player %= _numPlayers;
            Debug.Assert(player == 0 || player == 1, "Only support two players indexed 0 or 1");

            // START Handle GameAction
            if (gs.ThumbSticks.Left.X > 0.5f) GameEngine.HandleInput(player, GameEngine.GameAction.walk_right, 0);
            if (gs.ThumbSticks.Left.X < -0.5) GameEngine.HandleInput(player, GameEngine.GameAction.walk_left, 0);

            GameEngine.HandleInput(player, GameEngine.GameAction.look, (float)Math.Atan2(gs.ThumbSticks.Right.Y, gs.ThumbSticks.Right.X));
            if (gs.IsButtonDown(Buttons.RightTrigger)) GameEngine.HandleInput(player, GameEngine.GameAction.interact, 0);
            if (gs.IsButtonDown(Buttons.A)) GameEngine.HandleInput(player, GameEngine.GameAction.jump, 0);
            // END Handle GameAction

            if (gs.IsButtonDown(Buttons.Back)) 
                // TODO: Add a changed GameState, to escape the game
                //Exit();
                return;
        }
        
    }
}
