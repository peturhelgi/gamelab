using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;


using Microsoft.Xna.Framework;
namespace Project.ImageEffects
{
    public class SpriteSheetEffect : ImageEffect
    {

        public int FrameCounter;
        public int SwitchFrame;
        public Vector2 CurrentFrame;
        public Vector2 NumFrames;
        public int FrameWidth
        {
            get
            {
                return image.Texture == null ? 0 : image.Texture.Width / (int)NumFrames.X;
            }
        }

        public int FrameHeight
        {
            get
            {
                return image.Texture == null? 0 : image.Texture.Height / (int)NumFrames.Y;
            }
        }

        public SpriteSheetEffect()
        {
            NumFrames = new Vector2(3, 4);
            CurrentFrame = new Vector2(1, 0);
            SwitchFrame = 100;
            FrameCounter = 0;
        }

        public override void LoadContent(ref Image image)
        {
            base.LoadContent(ref image);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            if (image.IsActive)
            {
                FrameCounter += (int)gametime.ElapsedGameTime.TotalMilliseconds;
                if(FrameCounter >= SwitchFrame)
                {
                    FrameCounter = 0;
                    CurrentFrame.X++;

                    if(CurrentFrame.X * FrameWidth >= image.Texture.Width)
                    {
                        CurrentFrame.X = 0;
                    }
                }
            }
            else
            {
                CurrentFrame.X = 1;
            }

            image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth,
                (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
