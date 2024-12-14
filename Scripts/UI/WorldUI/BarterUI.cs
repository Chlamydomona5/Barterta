using Barterta.ItemGrid;
using Barterta.NPC.Merchant;
using Barterta.UI.ScreenUI;
using Barterta.UI.UIManage;
using UnityEngine;

namespace Barterta.UI.WorldUI
{
    public class BarterUI : UIObject
    {
        [SerializeField] private ItemWithCountUI requestUI;
        [SerializeField] private ItemWithCountUI merchandiseUI;
        private IconInfo _iconInfo;
    
        private void Start()
        {
            _iconInfo = Resources.Load<IconInfo>("IconInfo");
        }
    
        public void SetBarterUI(BarterRequest barterRequest)
        {
            requestUI.SetItemWithCount(new IdWithCount(barterRequest.requestItem.ID, barterRequest.num), _iconInfo);
            merchandiseUI.SetItemWithCount(new IdWithCount(barterRequest.rewardItem.ID, 1), _iconInfo);
        }
    }
}