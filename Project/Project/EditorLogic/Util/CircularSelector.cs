using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGreatEscape.EditorLogic.Util
{
    class CircularSelector
    {

        Texture2D _pickerWheel;
        List<GameObject> _objects;
        float _selectorAngle;
        public int SelectedElement;

        public CircularSelector(ContentManager content)
        {
            _pickerWheel = content.Load<Texture2D>("Sprites/Misc/picker_wheel");


        }

        public void Draw(SpriteBatch spriteBatch, Vector2 size, Vector2 position)
        {

            Vector2 itemSize = size / 6;
            Vector2 itemPosition = position + (size / 2) - (itemSize / 2);

            int distanceFromCenter = (int)((size.X / 2) * 0.7);


            spriteBatch.Draw(
                   _pickerWheel,
                   new Rectangle(position.ToPoint(), size.ToPoint()),
                   Color.Gray);

            int i = 0;

            float offsetAngle = (float)(2 * Math.PI / _objects.Count);
            SelectedElement = (int)Math.Floor((_selectorAngle/(2*Math.PI)) * _objects.Count);


            if (_objects == null)
            {
                return;
            }

            foreach (GameObject obj in _objects)
            {
                Vector2 direction = new Vector2((float)Math.Sin(offsetAngle * i + (offsetAngle/2)), (float)-Math.Cos(offsetAngle * i + (offsetAngle / 2)));
                direction.Normalize();

                Color c = Color.DarkGray;
                if (SelectedElement == i)
                {
                    c = Color.White;

                    spriteBatch.Draw(
                        obj.Texture,
                        new Rectangle((itemPosition).ToPoint(), itemSize.ToPoint()),
                        c
                    );
                }

                spriteBatch.Draw(
                    obj.Texture,
                    new Rectangle((itemPosition + (distanceFromCenter * direction)).ToPoint(), itemSize.ToPoint()),
                    c
                    );
                i++;
            }

        }

        public void SetObjects(List<GameObject> objects)
        {
            _objects = objects;
        }

        public void Update(Vector2 direction)
        {
            direction.Normalize();

            _selectorAngle = (float)Math.Atan(-direction.Y / direction.X);

            if (direction.X >= 0)
            {
                _selectorAngle += (float)(0.5 * Math.PI);
            }
            else
            {
                _selectorAngle += (float)(1.5 * Math.PI);
            }
        }
    }
}
