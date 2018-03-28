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
        public Obj player1Start;
        public Obj player2Start;
        public Pos end;
        public List<Obj> rocks;
    }

    class Pos
    {
        public float x;
        public float y;
    }

    class Obj
    {
        public float w;
        public float h;
        public float x;
        public float y;
        public float vx;
        public float vy;
        public float m;
    }
}
