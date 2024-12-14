using Barterta.InputTrigger;
using Barterta.ItemGrid;

namespace Barterta.Tool
{
    public class FishingRod : Tool
    {
        private FishingTrigger _trigger;
        public float astable = 1;
        public float pullConstant = 1;

        public override bool Judge(bool isLong, GrabTrigger trigger)
        {
            return !trigger.detector.targetBlock;
        }

        public override bool Effect(GroundBlock target, ToolTrigger trigger)
        {
            _trigger.StartFishing(boostTime, astable, pullConstant);
            BoostCost();
            return false;
        }

        public override void BoostTimer(float time)
        {
            _trigger.SetBuoySign(time);
        }

        public override void OnMove(bool isToHand, GrabTrigger trigger, GroundBlock block)
        {
            base.OnMove(isToHand, trigger, block);
            if(isToHand)
                _trigger = trigger.GetComponent<FishingTrigger>();
        }
    }
}