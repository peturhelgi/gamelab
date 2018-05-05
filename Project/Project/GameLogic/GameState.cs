using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using TheGreatEscape.GameLogic.GameObjects;
using TheGreatEscape.LevelManager;

namespace TheGreatEscape.GameLogic
{
    public class GameState : Level
    {
        public enum ExistingTools { pickaxe, rope };

        public SpriteFont GameFont;

        public List<Miner> Actors;
        public List<GameObject> Collectibles;
        public List<GameObject> Destroyables;
        public List<GameObject> Interactables;
        public List<GameObject> NonSolids;
        public List<GameObject> Solids;
        public Dictionary<ExistingTools, Tool> Tools;

        /// <summary>
        /// The state of the current game.
        /// </summary>
        public enum State
        {
            Completed,
            Paused,
            Running,
            GameOver
        }

        /// <summary>
        /// Determines how a game object is handled.
        /// This allows to change the behaviour and role of the game object.
        /// </summary>
        public enum Handling
        {
            Empty,
            None,
            Actor,
            Collect,
            Destroy,
            Interact,
            Solid
        }

        public bool Completed;
        private Texture2D Background;
        public float OutOfBoundsBottom;
        public State Mode;

        CollisionDetector CollisionDetector;

        public GameState()
        {
            Actors = new List<Miner>();
            Collectibles = new List<GameObject>();
            Destroyables = new List<GameObject>();
            Interactables = new List<GameObject>();
            NonSolids = new List<GameObject>();
            Solids = new List<GameObject>();
            Tools = new Dictionary<ExistingTools, Tool>();

            CollisionDetector = new CollisionDetector();
            Completed = false;
            OutOfBoundsBottom = float.MinValue;
            Mode = State.Running;
        }

        public List<GameObject> GetAll()
        {
            return Actors
                .Concat(Collectibles)
                .Concat(Destroyables)
                .Concat(Interactables)
                .Concat(NonSolids)
                .Concat(Solids)
                .ToList();
        }

        public void Remove(GameObject obj, Handling handling = Handling.Empty)
        {
            if(obj == null)
            {
                return;
            }
            if(handling == Handling.Empty)
            {
                handling = obj.Handling;
            }
            obj.Handling = handling;
            switch(handling)
            {
                case Handling.Actor:
                    if(obj is Miner)
                    {
                        ResetMinersPosition();
                        if (ShouldRemoveMinerFromScreen(obj as Miner))
                            obj.Disable();
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
        public void Add(GameObject obj, Handling handling = Handling.Empty)
        {
            if(obj == null)
            {
                return;
            }
            if(obj.BBox.Max.Y > OutOfBoundsBottom)
            {
                OutOfBoundsBottom = obj.BBox.Max.Y;
            }
            if(handling == Handling.Empty)
            {
                handling = obj.Handling;
            }
            obj.Handling = handling;
            switch(handling)
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

        // removes a miner from the screen when there are no more available tools to switch to
        public bool ShouldRemoveMinerFromScreen(Miner miner)
        {
            if (CanChangeTool(miner, true))
                return false;

            return true;
        }
        
        public void ResetMinersPosition()
        {
            foreach (Miner miner in Actors)
            {
                miner.ResetPosition();
            }
        }

        // second argument is if the method is called when trying to remove the miner
        public bool CanChangeTool(Miner miner, bool ForRemoving) 
        {
            int i;
            for (i = 0; i < resources.Count(); ++i)
            {
                var ttl = resources.ElementAt(i);
                if (ttl.Key.Equals(miner.Tool.ToString()))
                    break;
            }

            int newToolIndex = (i + 1) % resources.Count();
            var newTool = resources.ElementAt(newToolIndex);
            var oldTool = resources.ElementAt(i);

            // loop continuously through the tools until the next non-empty one is found
            while (newTool.Value == 0 && newToolIndex != i)
            {
                newToolIndex = (newToolIndex + 1) % resources.Count();
                newTool = resources.ElementAt(newToolIndex);
            }

            // if there is no tool available to switch just return
            if (newToolIndex == i)
                return false;

            resources[newTool.Key]--;
            if (!ForRemoving)
                resources[oldTool.Key]++;
            miner.Tool = (new ToolFactory()).Create(new Obj { Type = newTool.Key});
            return true;
        }

        public void SetBackground(Texture2D background)
        {
            Background = background;
        }

        public Texture2D GetBackground() {
            return Background;
        }

    }
}
