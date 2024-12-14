using System.Runtime.ConstrainedExecution;
using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.Facility.BlackboardResource;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.NaturalResouce;
using UnityEngine;

public class WaterResourceContainer : BlackboardResourceContainer, IPressInteractOnGroundEffector
{
    [SerializeField] private int ratio = 10;

    public override bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
    {
        return false;
    }

    public override void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
    {
    }
    
    public bool Judge(bool isLong, GrabTrigger trigger)
    {
        if(trigger.HandBlock.groundablesOn.Count == 0) return false;
        var bowl = trigger.HandBlock.groundablesOn[0].GetComponent<WaterContainer>();
        return bowl && bowl.WaterType == WaterType.PureWater;
    }

    public void OnInteract(bool isLong, GrabTrigger trigger)
    {
        Debug.Log("Interact");
        if (trigger.HandBlock.groundablesOn.Count == 0) return;
        var bowl = trigger.HandBlock.groundablesOn[0].GetComponent<WaterContainer>();
        if (bowl && bowl.WaterType == WaterType.PureWater)
        {
            bowl.WaterType = WaterType.Empty;
            HomeManager.I.ChangeBlackboardResource("water", bowl.WaterCount * ratio); 
        }
    }
}