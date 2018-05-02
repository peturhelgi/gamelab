using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic {
    public class GameState : Level
    {
        public List<Miner> Actors;
        public List<GameObject> Solids;
        public List<GameObject> Collectibles;
        public bool Completed;
        private Texture2D Background;

        CollisionDetector CollisionDetector;


        public GameState()
        {
            Actors = new List<Miner>();
            Solids = new List<GameObject>();
            Collectibles = new List<GameObject>();
            CollisionDetector = new CollisionDetector();
            Completed = false;
        }

        public List<GameObject> GetAll()
        {
            return Actors.Concat(Solids).Concat(Collectibles).ToList();
        }

        public void AddObject(GameObject obj)
        {
            if(obj is Miner)
            {
                AddActor(obj as Miner);
            }
            else if(obj is Ground)
            {
                AddSolid(obj);
            }
            else if(obj is Rock)
            {
                AddSolid(obj);
            }
            else if(obj is Door)
            {
                AddDoor(obj as Door);
            }

        }

        public void AddActor(Miner actor)
        {
            Actors.Add(actor);
        }

        public List<Miner> GetActors()
        {
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

        public void AddDoor(Door door)
        {
            Solids.Add(door);
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
