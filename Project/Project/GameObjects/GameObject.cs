using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Project.GameObjects
{
    interface IGameObject
    {
        BoundingBox BBox {
            get;
        }

        double Mass {
            get;
            set;
        }

        Vector2 Position
        {
            get;
            set;
        }

        Vector2 Dimension
        {
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

        string TextureString
        {
            get;
            set;
        }

        Texture2D Texture {
            get;
            set;
        }


    }

    abstract class GameObject : IGameObject {



        public Vector2 Position { get; set; }
        public Vector2 Dimension { get; set; }

        public Vector2 Speed { get; set; }

        public double Mass { get; set; }

        public BoundingBox BBox { get {
                return new BoundingBox(new Vector3(Position,0), new Vector3(Position+Dimension, 0));
                
            } }

        public Texture2D Texture { get; set; }

        public bool Visible { get; set; }
        public string TextureString { get; set; }
    }
}
