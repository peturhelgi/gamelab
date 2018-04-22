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


        public GameController(GameEngine gameEngine, Camera camera) {
            GameEngine = gameEngine;
            Camera = camera;
        }


        internal void HandleUpdate(GameTime gameTime)
        {
            GameEngine.gameTime = gameTime.TotalGameTime;
            HandleMouse( Mouse.GetState());
            GamePadState PlayerOneState = GamePad.GetState(PlayerIndex.One);
            if (PlayerOneState.IsConnected)
            {
                HandleGamePad(PlayerOneState);
                
                // There can only be a second player, if there is a first one
                GamePadState PlayerTwoState = GamePad.GetState(PlayerIndex.Two);
                if (PlayerTwoState.IsConnected)
                    HandleGamePad(PlayerTwoState);
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
            if (state.IsKeyDown(Keys.Right)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_right, 0);
            if (state.IsKeyDown(Keys.Left)) GameEngine.HandleInput(0, GameEngine.GameAction.walk_left, 0);
            if (state.IsKeyDown(Keys.Down)) GameEngine.HandleInput(0, GameEngine.GameAction.interact, 0);
            if (state.IsKeyDown(Keys.Up)) GameEngine.HandleInput(0, GameEngine.GameAction.jump, 0);

            // Player 2
            if (state.IsKeyDown(Keys.L)) GameEngine.HandleInput(1, GameEngine.GameAction.walk_right, 0);
            if (state.IsKeyDown(Keys.J)) GameEngine.HandleInput(1, GameEngine.GameAction.walk_left, 0);
            if (state.IsKeyDown(Keys.K)) GameEngine.HandleInput(1, GameEngine.GameAction.interact, 0);
            if (state.IsKeyDown(Keys.I)) GameEngine.HandleInput(1, GameEngine.GameAction.jump, 0);
            // END Handle GameAction




        }

        private void HandleGamePad(GamePadState gs)
        {
            if (gs.Buttons.Back == ButtonState.Pressed)
                // TODO: Add a changed GameState, to escape the game
                //Exit();
                return;
        }
        
    }
}
