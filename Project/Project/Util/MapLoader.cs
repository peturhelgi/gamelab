using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.GameObjects;
using Project.GameObjects.Miner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    class MapLoader
    {
        public List<GameObject> gameObjects;
        public ContentManager contentManager;

        public MapLoader(List<GameObject> gameObjects, ContentManager contentManager) {
            this.gameObjects = gameObjects;
            this.contentManager = contentManager;
        }

        //TODO: Pass file with map description
        public GameState initMap() {
            GameState gameState = new GameState();

            Vector2 startingPosition;
            Vector2 rockPosition;
            Miner miner;
            Rock rock;
            startingPosition = new Vector2(210, 250);
            rockPosition = new Vector2(900, 600);
            miner = new Miner(startingPosition, new Vector2(0.0f), 80.0, new BoundingBox());
            rock = new Rock(rockPosition, 300, 215);
            gameObjects.Add(miner);
            gameObjects.Add(rock);

            gameState.addMiner1(miner);
            gameState.addRock(rock);
            
            return gameState;
        }

        public void loadMapContent(GameState gameState) {
            gameState.getMiner1().Texture = contentManager.Load<Texture2D>("Miner_hands_in_pants");
            foreach (Rock rock in gameState.getRocks()) {
                rock.Texture = contentManager.Load<Texture2D>("Rock");
            }
        }
    }
}
