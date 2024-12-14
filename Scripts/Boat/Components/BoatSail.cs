using Barterta.InputTrigger;
using UnityEngine;

namespace Barterta.Boat.Components
{
    public class BoatSail : BoatComponent
    {
        private Vector3 TowardVector => transform.forward;
        [SerializeField] private float forceFactor = 10;
        public override Vector3 ProduceForceVector(Vector3 nowForceVector)
        {
            //Produce force vector toward the direction of the sail
            return TowardVector * forceFactor;
        }

        public override void OnHitIsland(Island.MONO.Island island)
        {
        
        }
        
        protected override bool CanInteractOnBoat(bool isLong, GrabTrigger trigger)
        {
            return true;
        }

        protected override void OnInteractOnBoat(GrabTrigger trigger)
        {
            //Rotate self for 45 degree
            transform.Rotate(Vector3.up, 45);
        }
    }
}