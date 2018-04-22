﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.GameLogic
{
    class Camera
    {
        public Matrix view;
        float _zoom;
        Vector2 _position;
        Vector2 _dimensions;

        public enum CameraAction { right, left, up, down, zoom_in, zoom_out };



        public Camera(float zoom, Vector2 position, Vector2 dimensions) {
            view = Matrix.Identity;
            _zoom = zoom;
            _position = position;
            _dimensions = dimensions;
            Refresh();

        }

        public void SetCameraToRectangle(Rectangle r) {
            int offset = 500;
            _position = new Vector2(r.X-offset, r.Y- offset);

            Vector2 dims = r.Size.ToVector2()+new Vector2(2*offset);
            Vector2 scales =    _dimensions/ dims;
            _zoom = Math.Min(scales.X, scales.Y);
            Refresh();
        }

        public void HandleAction(CameraAction action) {
            switch (action)
            {
                case (CameraAction.right):
                    Translate(new Vector2(2, 0));
                    break;

                case (CameraAction.left):
                    Translate(new Vector2(-2, 0));
                    break;

                case (CameraAction.up):
                    Translate(new Vector2(0, -2));
                    break;

                case (CameraAction.down):
                    Translate(new Vector2(0, 2));
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
            _zoom *= factor;
            Refresh();
        }

        public void Translate(Vector2 t) {
            _position += (t*_zoom);
            Refresh();
        }
        

        private void Refresh()
        {
            view = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) * Matrix.CreateScale(_zoom);
        }

    }
}