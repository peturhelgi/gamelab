using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
namespace Project.GameObjects {
    public class Ground : GameObject {
        public Ground() : base() {
            this.Velocity = Vector2.Zero;
            this.Mass = 1000;
            this.Visible = true;
        }
    }
}
