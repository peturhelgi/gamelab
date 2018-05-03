using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.LevelManager;
using System;
using TheGreatEscape.GameLogic.Util;

namespace TheGreatEscape.GameLogic {
    public class GameState : Level
    {
        public enum ExistingTools { pickaxe };

        public List<Miner> Actors;
        public List<GameObject> Solids;
        public List<GameObject> NonSolids;
        public List<GameObject> Collectibles;
        public Dictionary<ExistingTools, Tool> Tools;

        public bool Completed;
        private Texture2D Background;

        CollisionDetector CollisionDetector;


        public GameState()
        {
            Actors = new List<Miner>();
            Solids = new List<GameObject>();
            NonSolids = new List<GameObject>();
            Collectibles = new List<GameObject>();
            Tools = new Dictionary<ExistingTools, Tool>();

            CollisionDetector = new CollisionDetector();
            Completed = false;
        }

        public List<GameObject> GetAll() {
            return Actors.Concat(Solids).Concat(Collectibles).Concat(NonSolids).ToList();
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
            else if(obj is Crate)
            {
                AddSolid(obj);
            }
            else if (obj is Ladder)
            {
                AddNonSolid(obj);
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

        public void AddNonSolid(GameObject nonsolid)
        {
            NonSolids.Add(nonsolid);
        }
        public List<GameObject> GetNonSolids()
        {
            return NonSolids;
        }
        public void RemoveNonSolid(GameObject nonsolid)
        {
            NonSolids.Remove(nonsolid);
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

        public void InstantiateTools()
        {

            ToolFactory toolFactory = new ToolFactory();
            Tool tool;
            foreach (ExistingTools et in Enum.GetValues(typeof(ExistingTools)))
            {
                tool = toolFactory.Create(new Obj { Type = et.ToString() });
                Tools.Add(et, tool);
            }
        }

        public void SetBackground(Texture2D background) {
            Background = background;
        }
        
        public Texture2D GetBackground() {
            return Background;
        }

    }
}
