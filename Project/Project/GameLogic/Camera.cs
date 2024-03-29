﻿using System;
using Microsoft.Xna.Framework;

namespace TheGreatEscape.GameLogic
{
    public class Camera
    {
        public Matrix view;
        float _zoom;
        Vector2 _position;
        Vector2 _dimensions;
        Vector2 _minBox = new Vector2(700, 500);
        Rectangle _rectangle;
        private Rectangle _focusBox;
        public Vector2 MinBox
        {
            get
            {
                return _minBox;
            }
        }
        public Rectangle FocusBox {
            get
            {
                return _focusBox;
            }
            set
            {
                _focusBox = value;
                _focusBox.Size = Vector2.Max(
                    _focusBox.Size.ToVector2(), 
                    _minBox).ToPoint();
            }
        }

        public enum CameraAction { right, left, up, down, zoom_in, zoom_out };


        public Camera(float zoom, Vector2 position, Vector2 dimensions)
        {
            view = Matrix.Identity;
            _zoom = zoom;
            _position = position;
            _dimensions = dimensions;
            Refresh();
            
        }

        public void SetCameraToRectangle(Rectangle rect)
        {
            Vector2 offset = new Vector2(500, 300);
            _position = new Vector2(rect.X - offset.X, rect.Y - offset.Y);

            Vector2 dims = Vector2.Max(rect.Size.ToVector2(), _minBox) + 2 * offset;
            Vector2 scales = _dimensions / dims;
            _zoom = Math.Min(scales.X, scales.Y);
            _rectangle = new Rectangle(_position.ToPoint(), dims.ToPoint());
            Refresh();
        }

        public Rectangle GetCameraRectangle(int backgroundW, int backgroundH)
        {
            _rectangle.Height = Math.Max(backgroundH, _rectangle.Height);
            _rectangle.Width = Math.Max(backgroundW, _rectangle.Width);
            return _rectangle;
        }
        public void HandleAction(CameraAction action)
        {
            switch (action)
            {
                case (CameraAction.right):
                    Translate(new Vector2(20, 0) / _zoom);
                    break;

                case (CameraAction.left):
                    Translate(new Vector2(-20, 0) / _zoom);
                    break;

                case (CameraAction.up):
                    Translate(new Vector2(0, -20) / _zoom);
                    break;

                case (CameraAction.down):
                    Translate(new Vector2(0, 20) / _zoom);
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



        public void Zoom(float factor)
        {
            _zoom *= factor;
            Refresh();
        }

        public void Translate(Vector2 t)
        {
            _position += (t * _zoom);
            Refresh();
        }


        private void Refresh()
        {
            view = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) * Matrix.CreateScale(_zoom);
        }

    }
}
