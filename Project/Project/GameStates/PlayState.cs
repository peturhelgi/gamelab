using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Project.Util;
using Project.GameObjects;

namespace Project.GameStates
{
    public class PlayState : GameState<GameObject, Level>
    {

        public Image Background;
        protected List<Miner> Actors;
        protected List<GameObject> Solids;
        protected List<GameObject> Collectibles;

        public PlayState() : base()
        {
            Actors = new List<Miner>();
            Solids = new List<GameObject>();
            Collectibles = new List<GameObject>();
        }

        public override void LoadContent(ref Level level)
        {
            foreach(GameObject obj in level.Objects)
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

        public override List<GameObject> GetAll()
        {
            return base.GetAll();
        }

        public override void AddObject(GameObject obj)
        {
            Objects.Add(obj);

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

        public override List<GameObject> GetComponents(Type Type)
        {
            List<GameObject> list = new List<GameObject>();
            switch(Type.Name)
            {
                case "Miner":
                    // HACK: Concatenating Actors with list allows
                    //       converting to list<GameObject>
                    return Actors.Concat(list).ToList();

                case "Ground":
                    return Solids;

                default:
                    return Collectibles;
            }            
        }
    }
}