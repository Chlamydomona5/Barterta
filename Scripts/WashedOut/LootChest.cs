/*using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.ToolScripts;

namespace Barterta.Interactable
{
    public class LootChest : Groundable, IShortInteractOnGroundEffector, ILongInteractOnGroundEffector
    {
        public Dictionary<Groundable, float> lootsPool;
        public int lootCount = 2;
    
        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (blockUnder.island.monsterController.IsEmpty)
            {
                for (int i = 0; i < lootCount; i++)
                {
                    var loot = Methods.GetRandomValueInDict(lootsPool);
                    var block = blockUnder.island.GetRandomSurroundStackableBlock(blockUnder.coordinate, loot);
                    Instantiate(loot).SetOn(block);
                }
        
                Destroy(gameObject);   
            }
            else
            {
                trigger.GetComponent<DialogTrigger>().SelfBark("stillMonster");
            }
            return true;
        }
    }
}*/