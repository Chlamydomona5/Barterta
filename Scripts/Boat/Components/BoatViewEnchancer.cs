using Barterta.Boat;
using Barterta.InputTrigger;
using Barterta.Island.MONO;
using Barterta.ItemGrid;
using UnityEngine;

public class BoatViewEnchancer : NothingComponent
{
    public override void OnSettled(GroundBlock block)
    {
        if(belongBoat) belongBoat.hasViewEnchancer = false;
        base.OnSettled(block);
        if(belongBoat) belongBoat.hasViewEnchancer = true;
    }
    
}