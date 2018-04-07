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

        public enum CameraAction { right, left, up, down, zoom_in, zoom_out };



        public Camera(float zoom, Vector2 position) {
            view = Matrix.Identity;
            _Zoom = zoom;
            _Position = position;
            Refresh();

        }

        public void HandleAction(CameraAction action) {
            switch (action)
            {
                case (CameraAction.right):
                    Translate(new Vector2(1, 0));
                    break;

                case (CameraAction.left):
                    Translate(new Vector2(-1, 0));
                    break;

                case (CameraAction.up):
                    Translate(new Vector2(0, -1));
                    break;

                case (CameraAction.down):
                    Translate(new Vector2(0, 1));
                    break;

                case (CameraAction.zoom_in):
                    Zoom(1.03f);
                    break;

                case (CameraAction.zoom_out):
                    Zoom(0.97f);
                    break;

                default:
                    break;
            }

        }



        public void Zoom(float factor) {
            _Zoom *= factor;
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
