﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Renderer;

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

        bool Movable
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

    public abstract class GameObject : IGameObject {

        /// <summary>
        /// Describes how the game object should be handled
        /// </summary>
        /// 
        private Texture2D _texture;

        public GameState.Handling Handling { get; set; }

        public GameObject(Vector2 position, Vector2 spriteSize)
        {
            Position = position;
            SpriteSize = spriteSize;
            Handling = GameState.Handling.None;
        }
        public bool Falling { get; set; }
        public bool Active { get; set; }
        public GameObject(Vector2 position, Vector2 spriteSize, string texture)
        {
            Position = position;
            SpriteSize = spriteSize;
            TextureString = texture;
            Handling = GameState.Handling.None;
        }

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

        public virtual Vector2 SpriteSize { get; set; }

        public Vector2 Speed { get; set; }

        public double Mass { get; set; }

        public virtual AxisAllignedBoundingBox BBox { get {
                return new AxisAllignedBoundingBox(Position, Position + SpriteSize);
            } }

        public Texture2D Texture {
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

        public bool Movable { get; set; }


        public static GameObject Clone(GameObject source)
        {
            return (GameObject) source.MemberwiseClone();
        }

        public abstract LevelManager.Obj GetObj();
    }
}
