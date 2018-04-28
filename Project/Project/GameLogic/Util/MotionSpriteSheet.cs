using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic.GameObjects.Miner;

namespace TheGreatEscape.GameLogic.Util {
    class MotionSpriteSheet {

        public int FrameCounter;
        public int SwitchFrame;
        public Vector2 CurrentFrame;
        public Vector2 NumFrames;
        public Texture2D Image;
        public bool IsActive;
        public Rectangle SourceRectangle;
        public MotionType SheetType;

        public int FrameWidth {
            get {
                return Image == null ? 0 : Image.Width / (int)NumFrames.X;
            }
        }

        public int FrameHeight {
            get {
                return Image == null ? 0 : Image.Height / (int)NumFrames.Y;
            }
        }

        public MotionSpriteSheet(int NumberOfFrames, int MotionFPS, MotionType SpriteMotionType) {
            NumFrames = new Vector2(NumberOfFrames, 1);
            CurrentFrame = new Vector2(0, 0);
            SwitchFrame = MotionFPS;
            FrameCounter = 0;
            IsActive = true;
            SheetType = SpriteMotionType;
        }

        public void ResetCurrentFrame() {
            CurrentFrame = new Vector2(0, 0);
            FrameCounter = 0;
        }

        public bool DifferentMotionType(MotionType motion) {
            return motion != SheetType;
        }
        //public void LoadContent(ref Image image) {
        //    base.LoadContent(ref image);
        //}

        //public void UnloadContent() {
        //    base.UnloadContent();
        //}

        public void Update(GameTime gametime) {
            //base.Update(gametime);
            if (IsActive) {
                FrameCounter += (int)gametime.ElapsedGameTime.TotalMilliseconds;
                if (FrameCounter >= SwitchFrame) {
                    FrameCounter = 0;
                    CurrentFrame.X++;

                    if (CurrentFrame.X == NumFrames.X) {
                        CurrentFrame.X = 0;
                    }
                }
            }
            else {
                CurrentFrame.X = 0;
            }

            SourceRectangle = new Rectangle((int)CurrentFrame.X * FrameWidth,
                (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
