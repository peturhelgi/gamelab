using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;

namespace Project.GameObjects
{
    interface IGameObject
    {
        BoundingBox Box {
            get;
            set;
        }

        double Mass {
            get;
            set;
        }

        Vector2 Position {
            get;
            set;
        }

        Vector2 Speed {
            get;
            set;
        }

        bool Visible {
            get;
            set;
        }

        Body Body{
            get;
            set;
        }

    }

    abstract class GameObject : IGameObject {
        
        public Vector2 Position { get; set; }

        public Vector2 Speed { get; set; }

        public double Mass { get; set; }

        public BoundingBox Box { get; set; }

        public Texture2D Texture { get; set; }

        public bool Visible { get; set; }

        public Body Body { get; set; }

    }
}
