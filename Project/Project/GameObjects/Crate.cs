using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace Project.GameObjects
{
    /// <summary>
    /// Inherits from GameObject.
    /// </summary>
    class Crate : GameObject
    {
        public Crate()
        {
            // Default values for crates
            this.Movable = true;
            this.Visible = true;
        }

        /// <summary>
        /// Respawns the crate.
        /// </summary>
        void Respawn()
        {
            // TODO: set variables to initial values

        }

        /// <summary>
        /// Destroys the crate
        /// </summary>
        void Destroy()
        {
            if (!this.Destroyable) return;

            // TODO: add destoy logic
        }

        /// <summary>
        /// Moves the crate
        /// </summary>
        void Move()
        {
            if (!this.Movable) return;

            //TODO: add move logic
        }

        /// <summary>
        /// Loads the contents of the crate, e.g. sprite, effects etc
        /// </summary>
        public override void LoadContent()
        {
            //TODO: Implement
        }
        /// <summary>
        /// Unloads the contents of the crate
        /// </summary>
        public override void UnloadContent()
        {
            //TODO: Implement
        }

        /// <summary>
        /// Updates the state of the crate (deprecated)
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //TODO: Implement
        }

        /// <summary>
        /// Draws the crate to the canvas
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //TODO: Implement
        }
    }
}
