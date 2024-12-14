using Barterta.InputTrigger;
using Barterta.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Hat
{
    public class HatContainer : MonoBehaviour
    {
        [SerializeField, ReadOnly] private Hat hatOn;
        [SerializeField] private Transform headTransform;
        private Outfit _outfit;
    
        private void Start()
        {
            _outfit = GetComponentInChildren<Outfit>();
        }
    
        public void SetHat(Hat hat, GrabTrigger trigger)
        {
            _outfit.SetActiveHair(hatOn != null);

            var handBlock = hat.blockUnder;
            hat.BeRemovedFromNowBlock();
        
            if(hatOn != null)
            {
                hatOn.SetOn(handBlock);
                hatOn.SetModelPosition(false);
                trigger.AdjustHoldPosition();
            }

            hat.transform.SetParent(headTransform);
        
            hat.transform.localPosition = Vector3.zero;
            hat.SetModelPosition(true);
        
            hatOn = hat;
        }
    
    
    }
}