using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Boat
{
    public abstract class BoatComponent : Groundable, IBeSettled, IPressInteractOnGroundEffector
    {
        [SerializeField, ReadOnly]
        public Boat belongBoat;
        /// <summary>
        /// The Bigger the priority is, the later the component will be calculated
        /// </summary>
        public int priority = 5;
        public abstract Vector3 ProduceForceVector(Vector3 nowForceVector);
        public virtual void OnSettled(GroundBlock block)
        {
            //If the block is a boat block, add this component to the boat
            if (block is BoatBlock)
            {
                var boatBlock = (BoatBlock) block;
                belongBoat = boatBlock.belongBoat; 
                belongBoat.AddComponent(this);
            }
            //Else, if belongBoat exists, remove this component from the boat
            else if(belongBoat)
            {
                belongBoat.RemoveComponent(this);
                belongBoat = null;
            }
        }

        public abstract void OnHitIsland(Island.MONO.Island island);
        
        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            if(belongBoat)
                return CanInteractOnBoat(isLong, trigger);
            return false;
        }
        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (belongBoat) OnInteractOnBoat(trigger);
        }

        protected abstract bool CanInteractOnBoat(bool isLong, GrabTrigger trigger);
        
        protected abstract void OnInteractOnBoat(GrabTrigger trigger);
    }
}