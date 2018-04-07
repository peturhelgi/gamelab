using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameObjects
{
    class Ground : GameObject
    {
        public Ground(Vector2 position, Vector2 dimension, string TextureString) {
            Position = position;
            Speed = new Vector2(0);
            Mass = 1000;
            Dimension = dimension;
            Visible = true;
            //Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits((float)dimension.X), ConvertUnits.ToSimUnits((float)dimension.Y + (float)dimension.Y / 2f), 1f, ConvertUnits.ToSimUnits(position));
            this.TextureString = TextureString;

        }
    }
}
