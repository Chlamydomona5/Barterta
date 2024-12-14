/*using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;

namespace Barterta.Facility.BlackboardResource
{
    public class BlackboardResourceSubmitter : FacilityComponent, IConsumeGroundable
    {
        
        public bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
        {
            return groundable.GetComponent<BlackboardResourceMaterial>();
        }

        public void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null)
        {
        }

        public void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            var material = groundable.GetComponent<BlackboardResourceMaterial>();
            HomeManager.I.ChangeBlackboardResource(material.resourceName, material.amount);
        }
    }
}*/