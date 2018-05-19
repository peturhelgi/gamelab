using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Renderer;

namespace TheGreatEscape.GameLogic.GameObjects
{
    public interface IGameObject
    {

        AxisAllignedBoundingBox BBox
        {
            get;
        }

        double Mass
        {
            get;
            set;
        }

        Vector2 Position
        {
            get;
            set;
        }

        Vector2 SpriteSize
        {
            get;
            set;
        }

        Vector2 Speed
        {
            get;
            set;
        }

        bool Visible
        {
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

        bool Active
        {
            get;
            set;
        }
    }

    public abstract class GameObject : IGameObject
    {

        /// <summary>
        /// Describes how the game object should be handled
        /// </summary>
        /// 
        private Texture2D _texture;

        public GameState.Handling Handling { get; set; }

        public GameObject(Vector2 position, Vector2 spriteSize)
        {
            InitialPosition = Position = position;
            InitialSpriteSize = SpriteSize = spriteSize;
            Handling = GameState.Handling.None;
        }
        public bool Falling { get; set; }
        public bool Active { get; set; }

        public void Enable()
        {
            Visible = true;
            Active = true;
        }

        public virtual void Interact(GameState gameState)
        {

        }
        public void Disable()
        {
            Visible = false;
            Active = false;
        }

        public virtual Vector2 Position { get; set; }

        public Vector2 InitialPosition { get; protected set; }

        public Vector2 InitialSpriteSize { get; protected set; }
        private Vector2 _speed, _spriteSize;
        public Vector2 InitialSpeed { get; protected set; }

        public virtual void Initialize()
        {
            Position = InitialPosition;
            Speed = InitialSpeed;
            SpriteSize = InitialSpriteSize;
        }

        public virtual Vector2 SpriteSize
        {
            get
            {
                return _spriteSize;
            }
            set
            {
                _spriteSize = value;
                if(InitialSpriteSize == null)
                {
                    InitialSpriteSize = _spriteSize;
                }
            }
        }

        public virtual Vector2 Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                if (InitialSpeed == null)
                {
                    InitialSpeed = _speed;
                }
            }
        }

        public double Mass { get; set; }

        public virtual AxisAllignedBoundingBox BBox
        {
            get
            {
                return new AxisAllignedBoundingBox(Position, Position + SpriteSize);
            }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                TextureString = _texture.Name;
            }
        }

        public bool Visible { get; set; }

        public string TextureString { get; set; }

        public List<Light> Lights { get; set; }

        public int Seed { get; set; }

        public TimeSpan LastUpdated { get; set; }

        public bool Moveable { get; set; }


        public static GameObject Clone(GameObject source)
        {
            return (GameObject)source.MemberwiseClone();
        }

        public abstract LevelManager.Obj GetObj();
    }
}
