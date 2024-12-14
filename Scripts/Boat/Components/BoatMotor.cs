using Barterta.Boat;
using Barterta.InputTrigger;
using Barterta.Island.MONO;
using UnityEngine;

public class BoatMotor : BoatComponent
{
    [SerializeField] private float force = 1;
    public override Vector3 ProduceForceVector(Vector3 nowForceVector)
    {
        return nowForceVector.normalized * force;
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