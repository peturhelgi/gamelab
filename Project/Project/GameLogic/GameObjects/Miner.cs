using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameLogic.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Project.Libs;
using TheGreatEscape.GameLogic.Util;

namespace Project.GameLogic.GameObjects.Miner
{

    public enum MotionType { idle, walk, run, jump };
    class Miner : GameObject
    {
        Tool _tool;
        MotionType _motionType;

        public Dictionary<MotionType, MotionSpriteSheet> Motion;
        public MotionSpriteSheet CurrMotion;
        public SpriteEffects Orientation;

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



            InstantiateMotionSheets();
            Orientation = SpriteEffects.FlipHorizontally;
            _motionType = MotionType.walk;
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
                        mss = new MotionSpriteSheet(24, 42, MotionType.idle);
                        break;
                    case MotionType.walk:
                        mss = new MotionSpriteSheet(11, 100, MotionType.walk);
                        break;
                    case MotionType.run:
                        mss = new MotionSpriteSheet(12, 88, MotionType.run);
                        break;
                    //TODO: fix the jump sprite, has a small artefact
                    case MotionType.jump:
                        mss = new MotionSpriteSheet(12, 60, MotionType.jump);
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



        public void ChangeCurrentMotion()
        {
            MotionType m = MotionType.idle;
            //TODO: Improve fix for corner case when miner is walking while in air
            if (m == MotionType.walk && CurrMotion.SheetType == MotionType.jump)
            {
                return;
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
