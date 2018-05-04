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
        public float DisplacementY;
        private float _displacementStep = 5.0f;
        private float _maxHeight;
        private float _minHeight;
        public CollisionDetector CollisionDetector;


        public Platform(Vector2 position, Vector2 spriteSize, string textureString, float displacementY, int actId)
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
            DisplacementY = displacementY;
            ActivationId = actId;
            _maxHeight = Position.Y - DisplacementY;
            _minHeight = Position.Y;
            CollisionDetector = new CollisionDetector();
        }

        public void Move(GameState gamestate)
        {
            List<GameObject> actors = new List<GameObject>();
            foreach(Miner miner in gamestate.GetActors())
            {
                actors.Add(miner as GameObject);
            }
            List<GameObject> collisions = CollisionDetector.FindCollisions(this.BBox, actors);

            if (!Activate)
            {
                if (Position.Y < _minHeight)
                {
                    Position = new Vector2(Position.X, Position.Y + _displacementStep);
                    foreach (GameObject c in collisions)
                    {
                        if(c.Position.Y+c.SpriteSize.Y  < Position.Y) 
                            c.Position = new Vector2(c.Position.X, c.Position.Y + _displacementStep);
                    }
                }
            }
            else
            {
                if (Position.Y > _maxHeight)
                {
                    Position = new Vector2(Position.X, Position.Y - _displacementStep);
                    foreach (GameObject c in collisions) c.Position = new Vector2(c.Position.X, c.Position.Y - _displacementStep);
                }
            }
        }
    }
}

