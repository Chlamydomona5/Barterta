using System;
using Barterta.ItemGrid;

namespace Barterta.NPC.Merchant
{
    [Serializable]
    public class BarterRequest
    {
        public Groundable requestItem;
        public int num;
        public Groundable rewardItem;

        public BarterRequest(Groundable req, int n, Groundable rew)
        {
            requestItem = req;
            num = n;
            rewardItem = rew;
        }
    }
}