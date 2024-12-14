﻿/*using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.Facility;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.NaturalResouce;
using UnityEngine;

public class WaterSubmitter : FacilityComponent, IPressInteractOnGroundEffector
{
    [SerializeField] private int ratio = 10;
    public bool Judge(bool isLong, GrabTrigger trigger)
    {
        if(trigger.HandBlock.groundablesOn.Count == 0) return false;
        var bowl = trigger.HandBlock.groundablesOn[0].GetComponent<WaterContainer>();
        return bowl && bowl.WaterType == WaterType.PureWater;
    }

    public void OnInteract(bool isLong, GrabTrigger trigger)
    {
        if (trigger.HandBlock.groundablesOn.Count == 0) return;
        var bowl = trigger.HandBlock.groundablesOn[0].GetComponent<WaterContainer>();
        if (bowl && bowl.WaterType == WaterType.PureWater)
        {
            bowl.WaterType = WaterType.Empty;
            HomeManager.I.ChangeBlackboardResource("water", bowl.WaterCount * ratio); 
        }
    }
}*/