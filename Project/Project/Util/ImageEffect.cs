using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Project.Util
{
    public class ImageEffect
    {
        public bool IsActive;

        protected Image image;
        public ImageEffect()
        {
            IsActive = false;
        }

        public virtual void LoadContent(ref Image image)
        {
            this.image = image;
        }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gametime) { }
    }
}
