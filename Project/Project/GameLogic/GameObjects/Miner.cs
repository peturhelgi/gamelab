using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.Renderer;
using System;
using System.Collections.Generic;
using TheGreatEscape.GameLogic.Util;
using TheGreatEscape.Libs;

namespace TheGreatEscape.GameLogic.GameObjects.Miner
{

    public enum MotionType { idle, walk_left, walk_right, run_left, run_right, jump };
    class Miner : GameObject
    {
        Tool _tool;

        public Dictionary<MotionType, MotionSpriteSheet> Motion;
        public MotionSpriteSheet CurrMotion;
        public SpriteEffects Orientation;
        public float xVel;

        public TimeSpan lastUpdated;
        public Miner(Vector2 position, Vector2 spriteSize, Vector2 speed, double mass, string textureString)
        {
            lastUpdated = new TimeSpan();


            // Game Engine / motion parameters
            Position = position;
            Speed    = speed;
            Mass     = mass;

            // Rendering
            SpriteSize = spriteSize; //the size of the spritesheet used to render
            Visible = true;
            TextureString = textureString;
            Lights = new List<Light>
            {
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Circular, this),
                new Light((SpriteSize * new Vector2(0.5f, 0.15f)), Vector2.Zero, LightRenderer.Lighttype.Directional, this)
            };
            Seed = SingleRandom.Instance.Next();

            // Motion sheets
            xVel = 0;
            InstantiateMotionSheets();
            Orientation = SpriteEffects.FlipHorizontally;
            //TODO: add a case when it fails to get that type of motion
            Motion.TryGetValue(MotionType.idle, out CurrMotion);

            _tool = new Pickaxe();
        }

        private void InstantiateMotionSheets() {
            MotionSpriteSheet mss;
            Motion = new Dictionary<MotionType, MotionSpriteSheet>();

            foreach (MotionType m in Enum.GetValues(typeof(MotionType)))
            {
                switch (m)
                {
                    case MotionType.idle:
                        mss = new MotionSpriteSheet(24, 42, MotionType.idle, new Vector2(1, 1));
                        break;
                    case MotionType.walk_left:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.2f, 1));
                        break;
                    case MotionType.walk_right:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.2f, 1));
                        break;
                    case MotionType.jump:
                        mss = new MotionSpriteSheet(12, 84, m, new Vector2(1.5f, 1.12f));
                        break;
                    case MotionType.run_left:
                        mss = new MotionSpriteSheet(12, 64, m, new Vector2(1.4f, 1));
                        break;
                    case MotionType.run_right:
                        mss = new MotionSpriteSheet(12, 64, m, new Vector2(1.4f, 1));
                        break;
                    default:
                        mss = null;
                        break;
                }

                Motion.Add(m, mss);
            }
        }

        public void SetMotionSprite(Texture2D sprite, MotionType m) 
        {
            if (Motion.TryGetValue(m, out MotionSpriteSheet mss))
            {
                mss.Image = sprite;
            }
        }

        private MotionType GetCurrentState() {

            if (this.xVel > 0)
            {
                if (this.Speed.Y < 0f)
                    return MotionType.jump;
                else if (this.xVel >= GameEngine.RunSpeed)
                    return MotionType.run_right;
                else
                    return MotionType.walk_right;
            }
            if (this.xVel < 0)
            {
                if (this.Speed.Y < 0)
                    return MotionType.jump;
                else if (this.xVel <= -GameEngine.RunSpeed)
                    return MotionType.run_left;
                else
                    return MotionType.walk_left;
            }
            if (this.Speed.Y != 0)
            {
                return MotionType.jump;
            }

            return MotionType.idle;

        }

        public void ChangeCurrentMotion()
        {
            MotionType m = GetCurrentState();

            switch (m)
            {
                case MotionType.walk_right:
                    this.Orientation = SpriteEffects.FlipHorizontally;
                    break;
                case MotionType.walk_left:
                    this.Orientation = SpriteEffects.None;
                    break;
                case MotionType.run_right:
                    this.Orientation = SpriteEffects.FlipHorizontally;
                    break;
                case MotionType.run_left:
                    this.Orientation = SpriteEffects.None;
                    break;

            }
           
            //TODO: add check when this TryGetValue fails
            Motion.TryGetValue(m, out CurrMotion);
            if (CurrMotion.DifferentMotionType(m))
            {
                CurrMotion.ResetCurrentFrame();
            }

        }

        public bool UseTool(List<GameObject> gameObjects) {
            _tool.Use(this, gameObjects);

            return true;
        }
    }
}
