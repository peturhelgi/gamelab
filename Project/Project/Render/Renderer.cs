using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.GameObjects;
using Project.Screens;
using Project.Util;
using Project.GameStates;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Render
{
    public class Renderer<T1, T2>
    {
        
        public Renderer()
        {

        }

        public virtual void Initialize(ref GameState<T1, T2> gameState,
            ref Camera camera)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
