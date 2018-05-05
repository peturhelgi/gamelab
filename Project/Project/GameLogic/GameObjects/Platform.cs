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
        public Vector2 DisplacementStep;
        public float MaxHeight;
        public float MinHeight;
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
                DisplacementStep = new Vector2(0.0f, 5.0f);
                MaxHeight = Position.Y - Displacement;
                MinHeight = Position.Y;
            }
            else if (dir == "x")
            {
                _movingInYdir = false;
                DisplacementStep = new Vector2(-5.0f, 0.0f);
                MaxHeight = Position.X + Displacement;
                MinHeight = Position.X;
            }
        }

        public override AxisAllignedBoundingBox BBox
        {
            get
            {
                return new AxisAllignedBoundingBox(
                    new Vector2(Position.X, Position.Y + SpriteSize.Y * 0.9f), Position + SpriteSize);
            }
        }

        public bool IsMovingInY() { return _movingInYdir; }

        /* public void Move(GameState gamestate)
        {
            //List<GameObject> actors = new List<GameObject>();
            //foreach(Miner miner in gamestate.GetActors())
            //{
            //    actors.Add(miner as GameObject);
            //}
            float offset = 0.4f;
            AxisAllignedBoundingBox interactionBBox = new AxisAllignedBoundingBox(
                new Vector2(BBox.Min.X, BBox.Min.Y - offset),
                new Vector2(BBox.Max.X, BBox.Min.Y));
            List<GameObject> collisions = CollisionDetector.FindCollisions(interactionBBox, gamestate.GetAll());

            if (!Activate)
            {
                // moving down or left
                if ((_movingInYdir && Position.Y < MinHeight) || (!_movingInYdir && Position.X > MinHeight))
                {
                    Position = Position + DisplacementStep;
                    foreach (GameObject c in collisions)
                    {
                        if (c is Platform) continue;   
                        if (_movingInYdir && (c.Position.Y + c.SpriteSize.Y> Position.Y + SpriteSize.Y)) continue;
                        if (!_movingInYdir && (c.Position.X > Position.X + SpriteSize.X)) continue;
                        c.Position = c.Position + DisplacementStep;
                        c.Falling = false;
                        c.Speed = Vector2.Zero;
                        if ((c is Miner) && (c as Miner).Holding)
                        {
                            (c as Miner).HeldObj.Position = (c as Miner).HeldObj.Position + DisplacementStep;
                        }
                    }
                }
            }
            else
            {
                // moving up or right
                if ((_movingInYdir && Position.Y > MaxHeight) || (!_movingInYdir && Position.X < MaxHeight))
                {
                    Position = Position - DisplacementStep;
                    foreach (GameObject c in collisions)
                    {
                        if (c is Platform) continue;
                        if (_movingInYdir && (c.Position.Y + c.SpriteSize.Y > Position.Y + SpriteSize.Y)) continue;
                        if (!_movingInYdir && (c.Position.X + c.SpriteSize.X < Position.X)) continue;
                        c.Position = c.Position - DisplacementStep;
                        c.Falling = false;
                        c.Speed = Vector2.Zero;
                        if ((c is Miner) && (c as Miner).Holding)
                        {
                            (c as Miner).HeldObj.Position = (c as Miner).HeldObj.Position - DisplacementStep;
                        }
                    }
                }
            }
        }*/
    }
}

