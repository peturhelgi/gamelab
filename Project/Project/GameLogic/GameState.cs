using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic
{
    public class GameState : Level
    {
        public List<Miner> Actors;
        public List<GameObject> Collectibles;
        public List<GameObject> Destroyables;
        public List<GameObject> Interactables;
        public List<GameObject> NonSolids;
        public List<GameObject> Solids;

        public enum State
        {
            Completed,
            Paused,
            Running
        }

        public enum Action
        {
            Add,
            Remove
        }
        public enum Handling
        {
            None,
            Actor,
            Collect,
            Destroy,
            Interact,
            Solid
        }

        public bool Completed;
        private Texture2D Background;
        public float OutOfBounds;
        public State Mode;

        CollisionDetector CollisionDetector;

        public GameState()
        {
            Actors = new List<Miner>();
            Collectibles = new List<GameObject>();
            Destroyables = new List<GameObject>();
            NonSolids = new List<GameObject>();
            Interactables = new List<GameObject>();
            Solids = new List<GameObject>();
            CollisionDetector = new CollisionDetector();
            Completed = false;
            OutOfBounds = float.MinValue;
            Mode = State.Running;
        }

        public List<GameObject> GetAll()
        {
            return Actors
                .Concat(Solids)
                .Concat(Collectibles)
                .Concat(NonSolids)
                .ToList();
        }

        public void SetObject(GameObject obj, Action action = Action.Add)
        {
            if(obj == null)
            {
                return;
            }
            if(action == Action.Add)
            {
                if(obj.BBox.Max.Y > OutOfBounds)
                {
                    OutOfBounds = obj.BBox.Max.Y;
                }
                Add(obj);
            }
            else if(action == Action.Remove)
            {
                Remove(obj);
            }
        }

        protected void Remove(GameObject obj)
        {
            switch(obj.Handling)
            {
                case Handling.Actor:
                    if(obj is Miner)
                    {
                        Actors.Remove(obj as Miner);
                    }
                    break;
                case Handling.Solid:
                    Solids.Remove(obj);
                    break;
                case Handling.None:
                    NonSolids.Remove(obj);
                    break;
                case Handling.Interact:
                    Interactables.Remove(obj);
                    break;
                case Handling.Destroy:
                    Destroyables.Remove(obj);
                    break;
                case Handling.Collect:
                    Collectibles.Remove(obj);
                    break;
            }
        }
        protected void Add(GameObject obj)
        {
            switch(obj.Handling)
            {
                case Handling.Actor:
                    if(obj is Miner)
                    {
                        Actors.Add(obj as Miner);
                    }
                    break;
                case Handling.Solid:
                    Solids.Add(obj);
                    break;
                case Handling.None:
                    NonSolids.Add(obj);
                    break;
                case Handling.Interact:
                    Interactables.Add(obj);
                    break;
                case Handling.Destroy:
                    Destroyables.Add(obj);
                    break;
                case Handling.Collect:
                    Collectibles.Add(obj);
                    break;
            }
        }

        public List<Miner> GetActors()
        {
            return Actors;
        }

        public List<GameObject> GetObjects(Handling handling)
        {
            switch(handling)
            {
                case Handling.Collect:
                    return Collectibles;
                case Handling.Destroy:
                    return Destroyables;
                case Handling.Interact:
                    return Interactables;
                case Handling.None:
                    return NonSolids;
                case Handling.Solid:
                    return Solids;
                default:
                    return new List<GameObject>();
            }
        }

        public void SetBackground(Texture2D background)
        {
            Background = background;
        }

        public Texture2D GetBackground()
        {
            return Background;
        }

    }
}
