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

        public List<GameObject> Backup;
        public List<Miner> Actors;
        public List<GameObject> Collectibles;
        public List<GameObject> Destroyables;
        public List<GameObject> Interactables;
        public List<GameObject> NonSolids;
        public List<GameObject> Solids;
        public Dictionary<ExistingTools, Tool> Tools;
        public int Lives { private set; get; }
        /// <summary>
        /// The state of the current game.
        /// </summary>
        public enum State
        {
            Completed,
            Paused,
            Running,
            GameOver,
            Reset
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
        private List<GameObject> backgrounds;
        private List<GameObject> otherNoneSolids;
        private List<GameObject> grounds;
        private List<GameObject> otherSolids;

        CollisionDetector CollisionDetector;

        public GameState()
        {
            Backup = new List<GameObject>();
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

            Lives = 2;
            backgrounds = new List<GameObject>();
            otherNoneSolids = new List<GameObject>();
            grounds = new List<GameObject>();
            otherSolids = new List<GameObject>();
        }

        public void Respawn()
        {
            var allObjects = GetAll();
            //allObjects.Reverse();
            foreach (var obj in allObjects)
            {
                if (!(obj is Miner))
                {
                    Remove(obj, obj.Handling, true);
                }
            }
            foreach (var obj in Backup)
            {
                obj.Initialize();
                obj.Enable();
                if (obj is Miner)
                {
                    continue;
                }
                Add(obj);
            }
            Mode = State.Running;
        }

        public List<GameObject> GetAll()
        {
            return backgrounds
                .Concat(grounds)
                .Concat(otherNoneSolids)
                .Concat(Interactables)
                .Concat(Destroyables)
                .Concat(Actors)
                .Concat(otherSolids)
                .Concat(Collectibles)
                .ToList();
        }

        public void Remove(GameObject obj, Handling handling = Handling.Empty,
            bool disable = false)
        {
            if (obj == null)
            {
                return;
            }

            if (handling == Handling.Empty)
            {
                handling = obj.Handling;
            }
            bool moveToBackup = !(obj is Miner) && disable;
            obj.Handling = handling;
            switch (handling)
            {
                case Handling.Actor:
                    if (obj is Miner)
                    {
                        var heldObj = (obj as Miner).HeldObj;
                        if (heldObj != null)
                        {
                            ChangeHandling(heldObj, heldObj.Handling, Handling.Solid);
                        }
                        Remove(heldObj);
                        (obj as Miner).HeldObj = null;
                        (obj as Miner).Holding = false;

                        --Lives;
                        ResetMinersPosition();
                        if (Lives < Actors.Count(m => m.Active))
                            obj.Disable();
                        Respawn();
                    }
                    break;
                case Handling.Solid:
                    Solids.Remove(obj);
                    grounds.Remove(obj);
                    otherSolids.Remove(obj);
                    break;
                case Handling.None:
                    NonSolids.Remove(obj);
                    backgrounds.Remove(obj);
                    otherNoneSolids.Remove(obj);
                    break;
                case Handling.Interact:
                    Interactables.Remove(obj);
                    break;
                case Handling.Destroy:
                    Destroyables.Remove(obj);
                    moveToBackup = true;
                    break;
                case Handling.Collect:
                    Collectibles.Remove(obj);
                    moveToBackup = true;
                    break;
            }
            if (moveToBackup)
            {
                Backup.Add(obj);
                obj.Disable();
            }
        }
        public void Add(GameObject obj, Handling handling = Handling.Empty)
        {
            if (obj == null)
            {
                return;
            }
            if (obj.BBox.Max.Y > OutOfBoundsBottom)
            {
                OutOfBoundsBottom = obj.BBox.Max.Y;
            }
            if (handling == Handling.Empty)
            {
                handling = obj.Handling;
            }
            obj.Handling = handling;
            switch (handling)
            {
                case Handling.Actor:
                    if (obj is Miner)
                    {
                        Actors.Add(obj as Miner);
                    }
                    break;
                case Handling.Solid:
                    if (!Solids.Contains(obj)) Solids.Add(obj);
                    if (obj is Ground && !grounds.Contains(obj)) grounds.Add(obj);
                    else if (!otherSolids.Contains(obj)) otherSolids.Add(obj);
                    break;
                case Handling.None:
                    if (!NonSolids.Contains(obj)) NonSolids.Add(obj);
                    if (obj is PlatformBackground && !backgrounds.Contains(obj)) backgrounds.Add(obj);
                    if (!otherNoneSolids.Contains(obj)) otherNoneSolids.Add(obj);
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

        public void AddTool(ExistingTools et, Tool tool)
        {
            Tools.Add(et, tool);
        }
        public void ChangeHandling(GameObject obj, Handling before,
            Handling after)
        {
            if (obj != null && GetObjects(before).Contains(obj))
            {
                Remove(obj, before, false);
                Add(obj, after);
            }
        }

        public List<Miner> GetActors()
        {
            return Actors;
        }

        /// <summary>
        /// Remember: doesn't return actors
        /// </summary>
        /// <param name="handlings"></param>
        /// <returns></returns>
        public List<GameObject> GetObjects(params Handling[] handlings)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (var handling in handlings)
            {
                switch (handling)
                {
                    case Handling.Collect:
                        objects.AddRange(Collectibles);
                        break;
                    case Handling.Destroy:
                        objects.AddRange(Destroyables);
                        break;
                    case Handling.Interact:
                        objects.AddRange(Interactables);
                        break;
                    case Handling.None:
                        objects.AddRange(NonSolids);
                        break;
                    case Handling.Solid:
                        objects.AddRange(Solids);
                        break;
                }
            }

            return objects;
        }

        public void ResetMinersPosition()
        {
            foreach (Miner miner in Actors)
            {
                miner.Initialize();
            }
        }

        /// <summary>
        /// Tries to change the tool of the miner upon request or if he dies
        /// </summary>
        /// <param name="miner"></param>
        /// <param name="ForRemoving">set to true if the method is called when trying to remove the miner</param>
        /// <returns></returns>
        public bool CanChangeTool(Miner miner, bool ForRemoving = false)
        {
            int i = 0;
            if (miner.Tool != null)
            {
                for (i = 0; i < Resources.Count(); ++i)
                {
                    var ttl = Resources.ElementAt(i);
                    if (ttl.Key.Equals(miner.Tool.ToString()))
                        break;
                }
            }

            int newToolIndex = (i + 1) % Resources.Count();
            var newTool = Resources.ElementAt(newToolIndex);

            // loop continuously through the tools until the next non-empty one is found
            while (newTool.Value.Count == 0 && newToolIndex != i)
            {
                newToolIndex = (newToolIndex + 1) % Resources.Count();
                newTool = Resources.ElementAt(newToolIndex);
            }

            // if there is no tool available to switch just return

            if (newToolIndex == i && miner.Tool != null)
            {
                return false;
            }
            if(newTool.Value.Count <= 0)
            {
                return false;
            }
            Tool takenTool = newTool.Value.Last(),
                returnedTool = miner.Tool;
            TakeTool(takenTool);

            if (returnedTool != null && !ForRemoving)
            {
                GiveBackTool(returnedTool);
            }

            miner.Tool = takenTool;
            return true;
        }

        public bool GiveBackTool(Tool tool)
        {
            if (!tool.CanUseAgain)
            {
                return false;
            }
            Resources[tool.ToString()].Add(tool);
            return true;
        }

        public bool TakeTool(Tool tool)
        {
            string toolName = tool?.ToString();
            if (toolName == null || Resources[toolName].Count <= 0)
            {
                return false;
            }
            return Resources[toolName].Remove(tool);
        }

        public void SetBackground(Texture2D background)
        {
            Background = background;
        }

        public Texture2D GetBackground()
        {
            return Background;
        }

        public Level GetPureLevel()
        {
            Level level = new Level();

            level.levelnr = levelnr;
            level.levelname = levelname;
            level.prevlvl = prevlvl;
            level.prevlvlname = prevlvlname;
            level.nextlvl = nextlvl;
            level.nextlvlname = nextlvlname;
            level.background = background;

            level.objects = new List<Obj>();
            foreach (GameObject go in GetAll())
            {
                level.objects.Add(go.GetObj());
            }

            level.Resources = Resources;

            return level;
        }

    }
}
