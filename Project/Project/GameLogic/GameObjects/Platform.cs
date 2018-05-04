using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Collision;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic.GameObjects
{
    class Platform : GameObject
    {
        public bool Activate;
        public int ActivationId;
        public float Displacement;
        private Vector2 _displacementStep;
        private float _maxHeight;
        private float _minHeight;
        public CollisionDetector CollisionDetector;
        private bool _movingInYdir;


        public Platform(Vector2 position, Vector2 spriteSize, string textureString, float displacement, string dir, int actId)
            : base(position, spriteSize)
        {

            TextureString = textureString;
            Position = position;
            SpriteSize = spriteSize;
            Falling = false;

            Speed = Vector2.Zero;
            Mass = 10;
            Visible = true;
            LastUpdated = new TimeSpan();
            Moveable = false;
            Activate = false;
            Displacement = displacement;
            ActivationId = actId;
            CollisionDetector = new CollisionDetector();

            if (dir == "y")
            {
                _movingInYdir = true;
                _displacementStep = new Vector2(0, 5);
                _maxHeight = Position.Y - Displacement;
                _minHeight = Position.Y;
            }
            else if (dir == "x")
            {
                _movingInYdir = false;
                _displacementStep = new Vector2(-5, 0);
                _maxHeight = Position.X + Displacement;
                _minHeight = Position.X;
            }
        }

        public override AxisAllignedBoundingBox BBox
        {
            get
            {
                return new AxisAllignedBoundingBox(
                    new Vector2(Position.X, Position.Y + SpriteSize.Y*0.9f), Position + SpriteSize);
            }
        }

        public void Move(GameState gamestate)
        {
            List<GameObject> actors = new List<GameObject>();
            foreach(Miner miner in gamestate.GetActors())
            {
                actors.Add(miner as GameObject);
            }
            AxisAllignedBoundingBox interactionBBox = new AxisAllignedBoundingBox(
                new Vector2(BBox.Min.X, BBox.Min.Y - 0.5f),
                new Vector2(BBox.Max.X, BBox.Max.Y));
            List<GameObject> collisions = CollisionDetector.FindCollisions(interactionBBox, actors);

            if (!Activate)
            {
                // moving down or left
                if ((_movingInYdir && Position.Y < _minHeight) || (!_movingInYdir && Position.X > _minHeight))
                {
                    Position = Position + _displacementStep;
                    foreach (GameObject c in collisions)
                    {
                        if(c.Position.Y+c.SpriteSize.Y  < Position.Y + SpriteSize.Y) 
                            c.Position = c.Position + _displacementStep;
                    }
                }
            }
            else
            {
                // moving up or right
                if ((_movingInYdir && Position.Y > _maxHeight) || (!_movingInYdir && Position.X < _maxHeight))
                {
                    Position = Position - _displacementStep;
                    foreach (GameObject c in collisions)
                    {
                        if (c.Position.Y + c.SpriteSize.Y < Position.Y + SpriteSize.Y)
                            c.Position = c.Position - _displacementStep;
                    }
                }
            }
        }
    }
}

