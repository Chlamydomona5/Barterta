using Barterta.Boat;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Player;
using Barterta.Tool;
using UnityEngine;

public class Shovel : Tool
{
    public override bool Judge(bool isLong, GrabTrigger trigger)
    {
        return false;
    }

    public override bool Effect(GroundBlock target, ToolTrigger trigger)
    {
        if(!target || target.Equals(trigger.GetComponent<GridDetector>().GetStandBlock())) return false;
        if(target.groundablesOn.Count > 0) return false;
        target.DestoryAll();
        
        if (target is BoatBlock)
        {
            //Remove from boat
            var boatBlock = ((BoatBlock)target);
            boatBlock.belongBoat.RemoveBlock(boatBlock.boatCoordinate);
        }
        //Remove from island
        if(target.island)
        {
            target.island.RemoveBlock(target.Coordinate);
        }
        //Destroy
        Destroy(target.gameObject);
        return false;
    }
}