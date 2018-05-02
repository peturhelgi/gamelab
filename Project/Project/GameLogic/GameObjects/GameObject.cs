using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Renderer;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace TheGreatEscape.GameLogic.GameObjects
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

        TimeSpan LastUpdated
        {
            get;
            set;
        }

        bool Moveable
        {
            get;
            set;
        }


    }

    public abstract class GameObject : IGameObject {
        
        public GameObject(Vector2 position, Vector2 spriteSize)
        {
            Position = position;
            SpriteSize = spriteSize;
        }
        public bool Falling { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 SpriteSize { get; set; }

        public Vector2 Speed { get; set; }

        public double Mass { get; set; }

        public AxisAllignedBoundingBox BBox { get {
                return new AxisAllignedBoundingBox(Position, Position + SpriteSize);
            } }

        public Texture2D Texture { get; set; }

        public bool Visible { get; set; }

        public string TextureString { get; set; }

        public List<Light> Lights { get; set; }

        public int Seed { get; set; }

        public TimeSpan LastUpdated { get; set; }

        public bool Moveable { get; set; }


        public static GameObject Clone(GameObject source)
        {
            return (GameObject) source.MemberwiseClone();
        }
    }
}
