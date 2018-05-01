using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic.Collision;
using Project.GameLogic.Renderer;
using System.Collections.Generic;

namespace Project.GameLogic.GameObjects
{
    public interface IGameObject
    {
        AxisAllignedBoundingBox BBox {
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

        Vector2 SpriteSize {
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

        Texture2D Texture
        {
            get;
            set;
        }

        bool Falling
        {
            get;
            set;
        }

        List<Light> Lights
        {
            get;
            set;
        }

         int Seed
        {
            get;
            set;
        }

        bool Movable
        {
            get; set;
        }
    }

    public abstract class GameObject : IGameObject {


        public bool Falling { get; set; }
        public GameObject(Vector2 position, Vector2 spriteSize, string texture)
        {
            Position = position;
            SpriteSize = spriteSize;
            TextureString = texture;
        }

        public Vector2 Position { get; set; }

        public Vector2 SpriteSize { get; set; }

        public Vector2 Speed { get; set; }

        public double Mass { get; set; }

        public AxisAllignedBoundingBox BBox { get {
                return new AxisAllignedBoundingBox(Position, Position + SpriteSize);
            } }

        public Texture2D Texture { get; set; }

        public bool Visible { get; set; }

        public bool Movable { get; set; }

        public string TextureString { get; set; }

        public List<Light> Lights { get; set; }

        public int Seed { get; set; }
    }
}
