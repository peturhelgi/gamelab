using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    class Level
    {
        public int levelnr;
        public string levelname;
        public int prevlvl;
        public string prevlvlname;
        public int nextlvl;
        public string nextlvlname;
        public string background;

        public List<Obj> objects;
    }



    class Obj
    {

        [JsonProperty("dim")]
        public Vector2 SpriteSize;

        [JsonProperty("pos")]
        public Vector2 Position;

        [JsonProperty("vel")]
        public Vector2 Velocity;

        [JsonProperty("m")]
        public float Mass;

        [JsonProperty("type")]
        public string Type;

        [JsonProperty("texture")]
        public string Texture;
    }
}
