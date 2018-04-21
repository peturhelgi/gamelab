using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;

namespace Project.GameStates
{

    /// <summary>
    /// Constructor for a GameState, storing a list of T objects
    /// </summary>
    /// <typeparam name="T">Type of objects to be stored in the 
    /// GameState</typeparam>
    public class GameState
    {

        protected List<GameObject> Objects;
        public GameState(List<GameObject> objects = null)
        {
            Objects = objects == null ? new List<GameObject>() : objects;
        }

        public virtual void LoadContent(Object obj) { }

        public virtual void UnloadContent() => Objects.Clear();

        /// <summary>
        /// Use: List<T> list = GetAll();
        /// Pre: Nothing
        /// Post: list is a List<T> with all components of the GameState
        /// </summary>
        /// <returns></returns>
        public virtual List<GameObject> GetAll() => Objects;
        /// <summary>
        /// TODO: Add summary
        /// </summary>
        /// <param name="obj"></param>
        public virtual void AddObject(GameObject obj) => Objects.Add(obj);

        /// <summary>
        /// TODO: Add summary
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public virtual List<GameObject> GetComponents(Type Type) 
            => new List<GameObject>();
    }
}
