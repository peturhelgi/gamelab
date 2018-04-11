using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

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

        Vector2 SpriteSize {
            get;
            set;
        }

        Vector2 Velocity {
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
        }

        bool Movable {
            get;
        }

        bool Visible {
            get;
            set;
        }
    }


    public abstract class GameObject : IGameObject {

        /// <summary>
        /// Loads the content of the GameObject, e.g. images, effects etc
        /// </summary>
        public virtual void LoadContent() {
            /* Override this in all inherited classes */
        }

        /// <summary>
        /// Unloads the content of the GameObject
        /// </summary>
        public virtual void UnloadContent() {
            /* Override this in all inherited classes */
        }
        /// <summary>
        /// Updates the state of the GameObject (deprecated)
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) {
            /* Override this in all inherited classes */
        }

        /// <summary>
        /// Draws the GameObject to the canvas
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            /* Override this in all inherited classes */
        }

        /// <summary>
        /// True if the gameobject is airborne
        /// </summary>
        public bool Falling { get; set; }
        
        /// <summary>
        /// True iff GameObject can be destroyed
        /// </summary>
        public bool Destroyable { get; protected set; }


        /// <summary>
        /// True if the GameObject can be moved
        /// </summary>
        public bool Movable { get; protected set; }


        /// <summary>
        /// True iff the GameObject should be drawn
        /// </summary>
        public bool Visible { get; set; }


        [JsonProperty("pos")]
        public Vector2 Position { get; set; }

        [JsonProperty("dim")]
        public Vector2 SpriteSize { get; set; }

        [JsonProperty("vel")]
        public Vector2 Velocity { get; set; }

        [JsonProperty("m")]
        public double Mass { get; set; }

        public Texture2D Texture { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public BoundingBox BBox {
            get {
                return new BoundingBox(new Vector3(Position,0), new Vector3(Position + SpriteSize, 0));
            }
        }

        [JsonProperty("texture")]
        public string TextureString { get; set; }
    }
}