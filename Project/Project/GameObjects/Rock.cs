﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Project.GameObjects
{
    class Rock : GameObject
    {
        public Rock(Vector2 position, Vector2 dimension, World world) {
            Position = position;
            Speed = new Vector2(0);
            Mass = 10;
            Box = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + dimension.X, position.Y + dimension.Y, 0));
            Visible = true;
            Body = BodyFactory.CreateRectangle(world, dimension.X, dimension.Y, 1f, position);
        }
    }
}
