using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheGreatEscape.GameLogic.Collision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheGreatEscape.GameLogic.Util;


namespace TheGreatEscape.GameLogic
{
    class GameController
    {
        public GameEngine GameEngine;
        public Camera Camera;
        public bool DebugView { private set; get; }
        
        int _maxNumPlayers;

        public GameController(GameEngine gameEngine, Camera camera) {
            GameEngine = gameEngine;
            Camera = camera;
            _maxNumPlayers = 2;

            DebugView = false;
        }

        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse( Mouse.GetState());

            for(int i = 0; i < _maxNumPlayers; ++i)
            {
                HandleGamePad(GamePad.GetState(i), i);
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
                MyDebugger.WriteLine(ms.Position.X);
                MyDebugger.WriteLine(ms.Position.Y);
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
            MyDebugger.IsActive = state.IsKeyDown(Keys.P);
            GameManager.RenderDark = state.IsKeyUp(Keys.L);
            // Player 1
            if(_maxNumPlayers > 0)
            {
                if(state.IsKeyDown(Keys.Right)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_right, 0);
                if(state.IsKeyDown(Keys.Left)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_left, 0);
                if(state.IsKeyDown(Keys.Down)) GameEngine.HandleInput(0, GameEngine.GameAction.interact, 0);
                if(state.IsKeyDown(Keys.Up)) GameEngine.HandleInput(0, GameEngine.GameAction.jump, 0);
                if (state.IsKeyDown(Keys.RightShift) && state.IsKeyDown(Keys.Right))
                    GameEngine.HandleInput(0, GameEngine.GameAction.run_right, 0);
                if (state.IsKeyDown(Keys.RightShift) && state.IsKeyDown(Keys.Left))
                    GameEngine.HandleInput(0, GameEngine.GameAction.run_left, 0);
            }

            // Player 2
            if(_maxNumPlayers > 1)
            {
                if(state.IsKeyDown(Keys.D)) GameEngine.HandleInput(1, GameEngine.GameAction.walk_right, 0);
                if(state.IsKeyDown(Keys.A)) GameEngine.HandleInput(1, GameEngine.GameAction.walk_left, 0);
                if(state.IsKeyDown(Keys.S)) GameEngine.HandleInput(1, GameEngine.GameAction.interact, 0);
                if(state.IsKeyDown(Keys.W)) GameEngine.HandleInput(1, GameEngine.GameAction.jump, 0);
                if (state.IsKeyDown(Keys.LeftShift) && state.IsKeyDown(Keys.D))
                    GameEngine.HandleInput(1, GameEngine.GameAction.run_right, 0);
                if (state.IsKeyDown(Keys.LeftShift) && state.IsKeyDown(Keys.A))
                    GameEngine.HandleInput(1, GameEngine.GameAction.run_left, 0);
            }
            // END Handle GameAction            
        }

        private void HandleGamePad(GamePadState gs, int player)
        {
            if(!gs.IsConnected)
            {
                return;
            }

            // START Handle GameAction
            if (gs.ThumbSticks.Left.X > 0.5f) GameEngine.HandleInput(player, GameEngine.GameAction.walk_right, 0);
            if (gs.ThumbSticks.Left.X < -0.5f) GameEngine.HandleInput(player, GameEngine.GameAction.walk_left, 0);

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
