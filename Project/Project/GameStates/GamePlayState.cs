using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.Util;
using Project.GameObjects;

namespace Project.GameStates
{
    public class GamePlayState : GameState
    {

        public Image Background;
        protected List<Miner> Actors;
        protected List<GameObject> Solids;
        protected List<GameObject> Collectibles;

        public GamePlayState() : base()
        {
            Actors = new List<Miner>();
            Solids = new List<GameObject>();
            Collectibles = new List<GameObject>();
        }

        public override void LoadContent(Object level)
        {
            foreach(GameObject obj in (level as Level).Objects)
            {
                AddObject(obj);
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Actors.Clear();
            Solids.Clear();
            Collectibles.Clear();
        }

        public override List<GameObject> GetAll() => base.GetAll();

        public override void AddObject(GameObject obj)
        {
            base.AddObject(obj);

            if(obj is Miner)
            {
                Actors.Add((Miner)obj);
            }
            else if(obj is Ground)
            {
                Solids.Add(obj);
            }
            else
            {
                Collectibles.Add(obj);
            }
        }

        public override List<GameObject> GetComponents(Type type)
        {
            if(type == typeof(Miner))
            {
                List<GameObject> list = new List<GameObject>();
                return Actors.Concat(list).ToList();
            }
            else if(type == typeof(Ground))
            {
                return Solids;
            }
            else
            {
                return Collectibles;
            }
        }
    }
}