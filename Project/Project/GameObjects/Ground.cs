using Microsoft.Xna.Framework;
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
    class Ground : GameObject
    {
        public Ground(Vector2 position, Vector2 dimension, World world) {
            Position = position;
            Speed = new Vector2(0);
            Mass = 1000;
            Box = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + dimension.X, position.Y + dimension.Y, 0));
            Visible = true;
            Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits((float)dimension.X), ConvertUnits.ToSimUnits((float)dimension.Y + (float)dimension.Y / 2f), 1f, ConvertUnits.ToSimUnits(position));
            Body.BodyType = BodyType.Static;
            Body.Restitution = 0.4f;
            Body.Friction = 0.5f;
        }
    }
}
