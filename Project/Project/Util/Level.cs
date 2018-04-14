using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Project.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Project.Util {
    public class Level {

        public int PrevLvl;
        public int LevelNr;
        public int NextLvl;
        public string PrevLvlName;
        public string LevelName;
        public string NextLvlName;
        public string Background;

        public List<GameObject> Objects;
        public List<Layer> Layer;
        public Vector2 TileDimensions;

        public Level() {
            Layer = new List<Layer>();
            TileDimensions = Vector2.Zero;
        }

        public void LoadContent() {
            foreach(GameObject obj in Objects) {
                obj.LoadContent();
            }
            foreach(Layer layer in Layer) {
                layer.LoadContent(TileDimensions);
            }
        }

        public void UnloadContent() {
            foreach(GameObject obj in Objects) {
                obj.UnloadContent();
            }
            foreach(Layer layer in Layer) {
                layer.UnloadContent();
            }
        }

        public void Update(GameTime gameTime, ref Miner miner) {

            foreach(Layer layer in Layer) {
                layer.Update(gameTime, ref miner);
            }

            //TODO: Consider if this is best practice
            foreach(GameObject obj in Objects) {
                obj.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, string drawType) {
            foreach(Layer layer in Layer) {
                layer.Draw(spriteBatch, drawType);
            }

            //TODO: Consider if this is best practice
            foreach(GameObject obj in Objects) {
                obj.Draw(spriteBatch);
            }
        }
    }

    /// <summary>
    /// This class will be deprecated
    /// </summary>
    public class Obj {

        [JsonProperty("dim")]
        public Vector2 SpriteSize;

        [JsonProperty("pos")]
        public Vector2 Position;

        [JsonProperty("vel")]
        public Vector2 Velocity;

        [JsonProperty("m")]
        public float Mass;

        //Obsolete when using DataManager
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("texture")]
        public string Texture;

        public List<Layer> Layer;
        public Vector2 TileDimensions;
    }
}
