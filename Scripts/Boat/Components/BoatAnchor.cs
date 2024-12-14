using Barterta.InputTrigger;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Boat.Components
{
    public class BoatAnchor : BoatComponent
    {
        [SerializeField,ReadOnly] private bool isAnchored;
    
        public override Vector3 ProduceForceVector(Vector3 nowForceVector)
        {
            return isAnchored ? -nowForceVector : Vector3.zero;
        }

        public override void OnHitIsland(Island.MONO.Island island)
        {
            isAnchored = false;
        }
    
        public void Anchor(bool isAnchor)
        {
            isAnchored = isAnchor;
        }

        protected override bool CanInteractOnBoat(bool isLong, GrabTrigger trigger)
        {
            return true;
        }

        protected override void OnInteractOnBoat(GrabTrigger trigger)
        {
            Anchor(!isAnchored);
        }
    }
}