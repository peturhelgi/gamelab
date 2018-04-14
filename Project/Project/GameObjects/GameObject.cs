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

        Vector2 SpriteSize { get; set; }

        Vector2 Velocity { get; set; }

        string TextureString { get; set; }

        Texture2D Texture { get; set; }

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
        public void LoadContent() => this.Image?.LoadContent();

        /// <summary>
        /// Unloads the content of the GameObject
        /// </summary>
        public virtual void UnloadContent() => Image.UnloadContent();

        /// <summary>
        /// Updates the state of the GameObject (deprecated)
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) {
            // Let GameEngine handle the update
        }

        /// <summary>
        /// Draws the GameObject to the canvas
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch) {
            this.Image?.Draw(spriteBatch);
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
        [JsonProperty("pos")]
        public Vector2 Position { get; set; }

        /// <summary>
        /// Size of the GameObject's sprite
        /// </summary>
        [JsonProperty("dim")]
        public Vector2 SpriteSize { get; set; }

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
        /// The name of the texture to be used as a sprite
        /// </summary>
        [JsonProperty("texture")]
        public string TextureString { get; set; }

        /// <summary>
        /// The loaded texture for the sprite
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// type of the GameObject (to be deprecated, use $type instead)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The GameObject's bounding box
        /// </summary>
        public BoundingBox BBox {
            get {
                return new BoundingBox(new Vector3(Position, 0), new Vector3(Position + SpriteSize, 0));
            }
        }

    }
}