using System;
using Barterta.Boat;
using Barterta.InputTrigger;
using Barterta.Island.MONO;
using Barterta.ItemGrid;
using Barterta.PointArrow;
using UnityEngine;

public class BoatPointArrowContainer : NothingComponent
{
    private PointArrowController _pointArrowController;

    private void Start()
    {
        _pointArrowController = GetComponent<PointArrowController>();
    }

    public override Vector3 ProduceForceVector(Vector3 nowForceVector)
    {
        return Vector3.zero;
    }

    public override void OnHitIsland(Island island)
    {
    }

    protected override bool CanInteractOnBoat(bool isLong, GrabTrigger trigger)
    {
        return false;
    }

    protected override void OnInteractOnBoat(GrabTrigger trigger)
    {
    }
}