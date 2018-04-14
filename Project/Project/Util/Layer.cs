using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;

using Project.GameObjects;

namespace Project.Util {
    public class Layer {

        public class TileMap {
            public List<string> Row;
            public TileMap() {
                Row = new List<string>();
            }
        }

        public TileMap Tile;
        public Image Image;
        public string SolidTiles;
        public string OverlayTiles;

        List<Tile> underlayTiles, overlayTiles;
        string state;

        public Layer() {
            Image = new Image();
            underlayTiles = new List<Tile>();
            overlayTiles = new List<Tile>();
            SolidTiles = OverlayTiles = String.Empty;
        }

        public void LoadContent(Vector2 tileDimensions) {
            Image.LoadContent();
            Vector2 position = -tileDimensions;

            foreach(string row in Tile.Row) {
                string[] split = row.Split(']');
                position.X = -tileDimensions.X;
                position.Y += tileDimensions.Y;
                foreach(string s in split) {
                    if(!s.Equals(String.Empty)) {
                        position.X += tileDimensions.X;
                        if(!s.Contains("x")) {
                            state = "Passive";
                            Tile tile = new Tile();

                            string str = s.Replace("[", string.Empty);
                            int val1 = int.Parse(str.Substring(0, str.IndexOf(':')));
                            int val2 = int.Parse(str.Substring(str.IndexOf(':') + 1));


                            if(SolidTiles.Contains("[" + val1.ToString() + ":" + val2.ToString() + "]")) {
                                state = "Solid";
                            }

                            tile.LoadContent(position, new Rectangle(
                                val1 * (int)tileDimensions.X, val2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y), state);

                            if(OverlayTiles.Contains("[" + val1.ToString() + ":" + val2.ToString() + "]")) {
                                overlayTiles.Add(tile);
                            } else {
                                underlayTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void UnloadContent() {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Miner miner) {
            //Image.Update(gameTime);
            foreach(Tile tile in underlayTiles) {
                tile.Update(gameTime, ref miner);
            }
            foreach(Tile tile in overlayTiles) {
                tile.Update(gameTime, ref miner);
            }
        }

        public void Draw(SpriteBatch spriteBatch, string drawType) {
            List<Tile> tiles;
            if(drawType == "Underlay") { tiles = underlayTiles; } else { tiles = overlayTiles; }

            foreach(Tile tile in tiles) {
                Image.Position = tile.Position;
                Image.SourceRect = tile.SourceRect;
                Image.Draw(spriteBatch);
            }
        }
    }
}
