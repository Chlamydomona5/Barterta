using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using EPOOutline;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Tool
{
    public abstract class Tool : Durable, IPressInteractOnHandEffector, IMoveToHand
    {
        public bool onlyFaceWater;
        [ReadOnly] public float boostTime;
        public string animName;
        
        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (durability > 0 || infiniteDurability)
            {
                if ((onlyFaceWater && trigger.detector.targetBlock == null) || !onlyFaceWater)
                    trigger.GetComponent<ToolTrigger>().StartBoosting(this, trigger.detector.targetBlock);
                if (onlyFaceWater && trigger.detector.targetBlock != null)
                    trigger.GetComponent<DialogTrigger>().SelfBark("onlyfacewater");
            }
        }

        public abstract bool Judge(bool isLong, GrabTrigger trigger);

        public abstract bool Effect(GroundBlock target, ToolTrigger trigger);

        public virtual void BoostTimer(float time)
        {
            
        }

        protected void BoostCost()
        {
            DurabilityChange(-1);
        }



        public virtual void OnMove(bool isToHand, GrabTrigger trigger, GroundBlock block)
        {
        }
    }
}