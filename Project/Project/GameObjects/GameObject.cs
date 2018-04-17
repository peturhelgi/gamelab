using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

using Project.Util;

namespace Project.GameObjects {
    interface IGameObject {
        BoundingBox BBox { get; }

        double Mass { get; set; }

        Vector2 Position { get; set; }

        Vector2 Velocity { get; set; }

        bool Falling { get; }

        bool Movable { get; }

        bool Visible { get; set; }

        Image Image { get; set; }
    }


    public abstract class GameObject : IGameObject {

        public Image Image { get; set; }
        /// <summary>
        /// Loads the content of the GameObject, e.g. images, effects etc
        /// </summary>
        public void LoadContent() {            
            if(this.Image != null) {
                this.Image.LoadContent();
                this.Position = Image.Position;
            }
        }

        /// <summary>
        /// Unloads the content of the GameObject
        /// </summary>
        public virtual void UnloadContent() => this.Image?.UnloadContent();

        /// <summary>
        /// Updates the state of the GameObject (deprecated)
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) {
            if(this.Image != null) {
                this.Image.Position = this.Position;
            }
        }

        /// <summary>
        /// Draws the GameObject to the canvas
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) => 
            this.Image?.Draw(spriteBatch);

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

        /// <summary>
        /// The GameObject's bounding box
        /// </summary>
        public BoundingBox BBox {
            get {
                Vector3 min = new Vector3(Position, 0),
                    max = new Vector3(Position + this.Image.SpriteSize, 0);
                return new BoundingBox(min, max);
            }
        }
    }
}