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
        public virtual void LoadContent() {
            /* Override this in all inherited classes */
        }
        public virtual void UnloadContent() {
            /* Override this in all inherited classes */
        }
        public virtual void Update(GameTime gameTime) {
            /* Override this in all inherited classes */
        }
        public virtual void Draw(SpriteBatch spriteBatch) {
            /* Override this in all inherited classes */
        }

        public bool Falling { get; set; }
        
        public bool Destroyable { get; set; }

        public bool Movable { get;  set; }

        public bool Visible { get; set; }

        [JsonProperty("pos")]
        public Vector2 Position { get; set; }

        [JsonProperty("dim")]
        public Vector2 SpriteSize { get; set; }

        [JsonProperty("vel")]
        public Vector2 Velocity { get; set; }

        [JsonProperty("m")]
        public double Mass { get; set; }

        [JsonProperty("texture")]
        public Texture2D Texture { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public BoundingBox BBox {
            get {
                return new BoundingBox(new Vector3(Position,0), new Vector3(Position + SpriteSize, 0));
            }
        }

        public string TextureString { get; set; }
    }
}