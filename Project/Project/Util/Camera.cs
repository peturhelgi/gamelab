using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    class Camera
    {
        public Matrix view;
        private float _Zoom;
        private Vector2 _Position;


        public Camera(float zoom, Vector2 position) {
            view = Matrix.Identity;
            _Zoom = zoom;
            _Position = position;
            Refresh();

        }

        public void ZoomIn() {
            _Zoom *= 1.03f;
            Refresh();
        }

        public void ZoomOut() {
            _Zoom *= 0.97f;
            Refresh();
        }

        public void Translate(Vector2 t) {
            _Position += (t*_Zoom);
            Refresh();
        }
        

        private void Refresh()
        {
            view = Matrix.CreateTranslation(new Vector3(-_Position.X, -_Position.Y, 0)) * Matrix.CreateScale(_Zoom);
        }

    }
}
