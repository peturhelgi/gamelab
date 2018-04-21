using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.Util.Collision;
using Project.Util;
using Project.Screens;
using Newtonsoft.Json;

namespace Project.GameObjects
{
    interface IGameObject
    {
        AxisAlignedBoundingBox BBox
        {
            get;
        }

        double Mass { get; set; }

        Vector2 Position { get; set; }

        Vector2 Velocity { get; set; }

        bool Falling { get; }

        bool Movable { get; }

        bool Visible { get; set; }

        Image Image { get; set; }
    }

    public abstract class GameObject : IGameObject
    {
        protected ScreenManager ScreenManager;
        public GameObject() {
            Velocity = Vector2.Zero;
            Position = Vector2.Zero;
            Visible = true;
            Mass = 0;
            Falling = false;
        }

        public void Initialize(ScreenManager screenManager)
        {
            ScreenManager = screenManager;
        }
        public Image Image { get; set; }

        /// <summary>
        /// Loads the content of the GameObject, e.g. images, effects etc
        /// </summary>
        public void LoadContent()
        {
            if(this.Image != null)
            {
                this.Image.LoadContent(ScreenManager);
                this.Position = Image.Position;
            }
        }

        /// <summary>
        /// Unloads the content of the GameObject
        /// </summary>
        public virtual void UnloadContent() => this.Image?.UnloadContent();

        /// <summary>
        /// Updates the positinon of the GameObject's sprite
        /// TODO: Reconsider
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            if(Image != null)
            {
                Image.Position = Position;
            }
        }

        /// <summary>
        /// True if the gameobject is airborne
        /// </summary>
        public bool Falling { get; set; }

        /// <summary>
        /// True iff the GameObject should be drawn
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// True iff GameObject can be destroyed
        /// </summary>
        public bool Destroyable { get; protected set; }

        /// <summary>
        /// True if the GameObject can be moved
        /// </summary>
        public bool Movable { get; protected set; }

        /// <summary>
        /// Position of the GameObject
        /// </summary>
        [JsonIgnore]
        public Vector2 Position { get; set; }

        /// <summary>
        /// Velocity of the gameobject
        /// </summary>
        [JsonProperty("vel")]
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The mass of the GameObject
        /// </summary>
        [JsonProperty("m")]
        public double Mass { get; set; }
    
        public AxisAlignedBoundingBox BBox
        {
            get
            {
                // BUG: Could this be the reason for a weird collision?
                return new AxisAlignedBoundingBox(Position, Position + Image.SpriteSize);
            }
        }

        public Texture2D Texture { get; set; }

        public string TextureString { get; set; }
    }
}
