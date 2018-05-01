
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.GameLogic.GameObjects.Miner;
using TheGreatEscape.LevelManager;

using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGreatEscape.GameLogic
{
    class GameState : Level
    {
        public List<Miner> Actors;
        public List<GameObject> Solids;
        public List<GameObject> Collectibles;
        private Texture2D Background;
        CollisionDetector CollisionDetector;


        public GameState() {
            Actors = new List<Miner>();
            Solids = new List<GameObject>();
            Collectibles = new List<GameObject>();
            CollisionDetector = new CollisionDetector();
        }

        public List<GameObject> GetAll() {
            return Actors.Concat(Solids).Concat(Collectibles).ToList();
        }


        public void AddActor(Miner actor)
        {
            Actors.Add(actor);
        }

        public List<Miner> GetActors() {
            return Actors;
        }


        public void AddSolid(GameObject solid)
        {
            Solids.Add(solid);
        }
        public List<GameObject> GetSolids()
        {
            return Solids;
        }
        public void RemoveSolid(GameObject solid)
        {
            Solids.Remove(solid);
        }


        public void AddCollectible(GameObject collectible)
        {
            Collectibles.Add(collectible);
        }
        public List<GameObject> GetCollectibles()
        {
            return Collectibles;
        }
        public void RemoveCollectible(GameObject collectible)
        {
            Collectibles.Remove(collectible);
        }

        public void SetBackground(Texture2D background) {
            Background = background;
        }
        
        public Texture2D GetBackground() {
            return Background;
        }

    }
}
