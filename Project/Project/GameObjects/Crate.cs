using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace Project.GameObjects {
    /// <summary>
    /// Inherits from GameObject.
    /// </summary>
    public class Crate : GameObject {
        public Crate() {
            // Default values for crates
            this.Movable = true;
            this.Visible = true;
        }

        /// <summary>
        /// Respawns the crate.
        /// </summary>
        void Respawn() {
            // TODO: set variables to initial values

        }

        /// <summary>
        /// Destroys the crate
        /// </summary>
        void Destroy() {
            if(!this.Destroyable) { return; }

            // TODO: add destoy logic
        }

        /// <summary>
        /// Moves the crate
        /// </summary>
        void Move() {
            if(!this.Movable) { return; }

            //TODO: add move logic
        }

        /// <summary>
        /// Updates the state of the crate (deprecated)
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            //TODO: Implement
        }
    }
}
