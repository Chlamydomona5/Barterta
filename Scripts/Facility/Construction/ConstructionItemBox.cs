using System.Linq;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using UnityEngine;

namespace Barterta.Facility.Construction
{
    public class ConstructionItemBox : FacilityComponent, IConsumeGroundable
    {
        private RequestUI requestUI;
        [SerializeField] private RequestUI requestUIPrefab;
        public bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
        {
            var tryAddItem = ((ConstructionSite)facility).CanAddItem(groundable.ID);
            return tryAddItem;
        }

        public void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null)
        {
        }

        public void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            ((ConstructionSite)facility).AddItem(groundable.ID);
            requestUI.SetCurrent(((ConstructionSite)facility).currentItems);
        }
        
        public override void Init(FacilityEntity parent, GroundBlock block)
        {
            base.Init(parent, block);
            requestUI = (RequestUI)GetComponentInChildren<PlayerStayUI>().uiInstance;
            requestUI.Init(((ConstructionSite)parent).Recipe.RequiredItems.ToList());
        }
    }
}