using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project.Util {
    public class Camera {
        public Matrix view;
        private float _Zoom;
        private Vector3 _Position, translation;

        int panSpeed = 2;
        float zoomSpeed = 0.03f;

        public enum Action { right, left, up, down, zoom_in, zoom_out };

        public Camera(float zoom, Vector2 position) {
            view = Matrix.Identity;
            _Zoom = zoom;
            //_Position = position;
            _Position.X = position.X;
            _Position.Y = position.Y;
            _Position.Z = translation.Z = 0;
            Refresh();
        }

        public void HandleAction(Action action) {
            translation.X = translation.Y = 0;
            switch(action) {
                case (Action.right):
                    translation.X = panSpeed;
                    Translate(translation);
                    break;

                case (Action.left):
                    translation.X = -panSpeed;
                    Translate(translation);
                    break;

                case (Action.up):
                    translation.Y = -panSpeed;
                    Translate(translation);
                    break;

                case (Action.down):
                    translation.Y = panSpeed;
                    Translate(translation);
                    break;

                case (Action.zoom_in):
                    Zoom(1.0f + zoomSpeed);
                    break;

                case (Action.zoom_out):
                    Zoom(1.0f - zoomSpeed);
                    break;

                default:
                    return;
            }
            Refresh();
        }

        public void Zoom(float factor) {
            _Zoom *= factor;
        }

        public void Translate(Vector3 t) {
            _Position += (t * _Zoom);
        }

        private void Refresh() {
            view = Matrix.CreateTranslation(-_Position)
                * Matrix.CreateScale(_Zoom);
        }

    }
}
