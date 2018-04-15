using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
namespace Project.GameObjects {
    public class Ground : GameObject {
        public Ground() {
            this.Velocity = new Vector2(0);
            this.Mass = 1000;
            this.Visible = true;
        }
    }
}
