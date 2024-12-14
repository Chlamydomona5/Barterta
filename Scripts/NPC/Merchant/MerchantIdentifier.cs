using System.Collections.Generic;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.NPC.Merchant
{
    [CreateAssetMenu(fileName = "MerchantIdentifier", menuName = "SO/MerchantIdentifier")]
    public class MerchantIdentifier : ScriptableObject
    {
        public List<ItemSet> requiredItemLists;
        //public List<Groundable> requiredItems;
        public List<Groundable> sellingItems;

        public BarterRequest GetRequest()
        {
            var reqList = requiredItemLists[Random.Range(0, requiredItemLists.Count)].list;
            var req = reqList[Random.Range(0, reqList.Count)];

            var rew = sellingItems[Random.Range(0, sellingItems.Count)];
            //number = rew's value / req's value - 1
            var num = Mathf.Clamp(rew.value / req.value - 1, 1, 100);
            return new BarterRequest(req, num, rew);
        }

        public BarterRequest GetRequestWithMerchandise(Groundable merchandise)
        {
            var reqList = requiredItemLists[Random.Range(0, requiredItemLists.Count)].list;
            var req = reqList[Random.Range(0, reqList.Count)];
            
            var rew = merchandise;
            //number = rew's value / req's value - 1
            var num = Mathf.Clamp(rew.value / (req.value * 2), 1, 100);
            return new BarterRequest(req, num, rew);
        }
    }
}