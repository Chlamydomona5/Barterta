using Barterta.Core;
using Barterta.Island.SO;
using Barterta.Island.SO.IslandType;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Island.MONO
{
    public class NaturalIsland : Island
    {
        public IslandHeart.IslandHeart heart;

        public void InsIslandHeart()
        {
            var temp = Instantiate(Resources.Load<Groundable>("IslandHeart/IslandHeart"));
            temp.SetOn(BlockMap[0, 0]);
            heart = temp.GetComponent<IslandHeart.IslandHeart>();
        }

        public override void Init(IslandForm form, ElfAttribute attr, Vector3 center, Chunk ck)
        {
            //Set Center
            centerPoint = center;
            transform.position = center;
            //Set feature
            islandForm = form;
            attribute = attr;
            base.Init(form, attr, center, ck);
            InsIslandHeart();
        }
    }
}