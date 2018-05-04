﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace TheGreatEscape.LevelManager {
    public class Level
    {
        public int levelnr;
        public string levelname;
        public int prevlvl;
        public string prevlvlname;
        public int nextlvl;
        public string nextlvlname;
        public string background;

        public List<Obj> objects;
        public Dictionary<string, int> resources;
    }



    public class Obj
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

        [JsonProperty("tool")]
        public string Tool;
    }
}
