﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameObjects;
using Project.GameObjects.Miner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VelcroPhysics.Dynamics;

namespace Project.Util
{
    class MapLoader
    {
        public List<GameObject> gameObjects;
        public ContentManager contentManager;
        public readonly World world;

        //static MapLoader() {
        //    MapLoader.world = new World(new Vector2(0, 9.82f));
        //}
        public MapLoader(List<GameObject> gameObjects, ContentManager contentManager) {
            this.gameObjects = gameObjects;
            this.contentManager = contentManager;

            // Create a world with gravity.
            world = new World(new Vector2(0, 9.82f));
            //this.world = world;
        }

        public GameState initMap(string levelName) {
            GameState gameState = new GameState();

            string text = contentManager.Load<string>(levelName);
            Level level = JsonConvert.DeserializeObject<Level>(text);

            foreach (Obj obj in level.objects) {

                switch (obj.Type) {
                    case "miner":
                        Miner miner = new Miner(obj.Position,obj.Velocity, obj.Mass, new BoundingBox(), world);
                        gameObjects.Add(miner);
                        gameState.addMiner1(miner);
                        break;
                    case "rock":
                        Rock rock = new Rock(obj.Position, obj.Dimension, world);
                        gameObjects.Add(rock);
                        gameState.addRock(rock);
                        break;
                    case "ground":
                        Ground ground = new Ground(obj.Position, obj.Dimension, world);
                        gameObjects.Add(ground);
                        gameState.addGround(ground);
                        break;
                    case "end":
                        break;

                    default:
                        Console.WriteLine("Object of Type "+obj.Type+" not implemented.");
                        break;
                }
            }

            return gameState;
        }

        public void loadMapContent(GameState gameState) {
            gameState.getMiner1().Texture = contentManager.Load<Texture2D>("Miner");
            foreach (Rock rock in gameState.getRocks()) {
                rock.Texture = contentManager.Load<Texture2D>("Rock");
            }

            foreach (Ground gnd in gameState.getGround()) {
                gnd.Texture = contentManager.Load<Texture2D>("Ground");
            }

        }
    }
}
