using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Util {
    public class Tile {
        Vector2 position;
        Rectangle sourceRect;
        string state;

        public Rectangle SourceRect => sourceRect;

        public Vector2 Position => position;

        public void LoadContent(Vector2 position, Rectangle sourceRect, string state) {
            this.position = position;
            this.sourceRect = sourceRect;
            this.state = state;
        }
        public void UnloadContent() {

        }

        public void Update(GameTime gameTime, ref Miner miner) {
            if(state == "Solid") {
                Rectangle tileRect = new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)SourceRect.Width,
                    (int)SourceRect.Height
                    );

                Rectangle minerRect = new Rectangle(
                    (int)miner.Image.Position.X,
                    (int)miner.Image.Position.Y,
                    (int)miner.Image.SourceRect.Width,
                    (int)miner.Image.SourceRect.Height
                    );
                if(minerRect.Intersects(tileRect)) {
                    if(miner.Velocity.X < 0) {
                        miner.Image.Position.X = tileRect.Right;

                    } else if(miner.Velocity.X > 0) {
                        miner.Image.Position.X = tileRect.Left - miner.Image.SourceRect.Width;
                    } else if(miner.Velocity.Y < 0) {
                        miner.Image.Position.Y = tileRect.Bottom;
                    } else {
                        miner.Image.Position.Y = tileRect.Top - miner.Image.SourceRect.Height;
                    }
                }
            }
        }
    }
}
