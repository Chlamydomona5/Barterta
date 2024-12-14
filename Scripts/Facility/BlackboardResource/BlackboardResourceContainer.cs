using System.Collections.Generic;
using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Facility.BlackboardResource
{
    public class BlackboardResourceContainer : FacilityEntity, IConsumeGroundable
    {
        public string resourceName;
        [SerializeField] private List<float> levelToCapacityList = new() { 20, 50, 100 };

        public override void Start()
        {
            base.Start();
            HomeManager.I.ChangeBlackboardResourceCapacity(resourceName, levelToCapacityList[Level]);
            HomeManager.I.StartBlackboardResource(resourceName);
        }

        public override void LevelUp()
        {
            base.LevelUp();
            HomeManager.I.ChangeBlackboardResourceCapacity(resourceName, levelToCapacityList[Level]);
        }
        
        public virtual bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
        {
            return groundable.GetComponent<BlackboardResourceMaterial>();
        }

        public virtual void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null)
        {
        }

        public virtual void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            var material = groundable.GetComponent<BlackboardResourceMaterial>();
            HomeManager.I.ChangeBlackboardResource(material.resourceName, material.amount);
        }
    }
}