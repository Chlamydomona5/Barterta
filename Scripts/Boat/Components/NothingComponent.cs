using Barterta.Boat;
using Barterta.InputTrigger;
using Barterta.Island.MONO;
using UnityEngine;

public class NothingComponent : BoatComponent
{
    public override Vector3 ProduceForceVector(Vector3 nowForceVector)
    {
        return Vector3.zero;
    }

    public override void OnHitIsland(Island island)
    {
    }

    protected override void OnInteractOnBoat(GrabTrigger trigger)
    {
    }
    
    protected override bool CanInteractOnBoat(bool isLong, GrabTrigger trigger)
    {
        return false;
    }
}