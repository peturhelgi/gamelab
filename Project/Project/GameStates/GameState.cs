using Project.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Util;

namespace Project.GameStates
{

    public class GameState<T1, T2>
    {

        protected List<T1> Objects;
        public GameState()
        {
            Objects = new List<T1>();
        }

        public virtual void LoadContent(ref T2 obj) { }

        public virtual void UnloadContent() { Objects.Clear(); }

        /// <summary>
        /// Use: List<T> list = GetAll();
        /// Pre: Nothing
        /// Post: list is a List<T> with all components of the GameState
        /// </summary>
        /// <returns></returns>
        public virtual List<T1> GetAll() => Objects;
       /// <summary>
       /// TODO: Add summary
       /// </summary>
       /// <param name="obj"></param>
        public virtual void AddObject(T1 obj) {
            Objects.Add(obj);
        }

        /// <summary>
        /// TODO: Add summary
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public virtual List<T1> GetComponents(Type Type)
        {
            return new List<T1>();
        }
    }
}
