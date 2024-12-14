using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Player;
using Barterta.ToolScripts;

namespace Barterta.NaturalResouce
{
    public class NatureMaterial : Groundable
    {
        public override void OnPutInWater(GrabTrigger trigger)
        {
            var island = trigger.GetComponent<GridDetector>().GetStandBlock().island;
            var coord = trigger.GetComponent<GridDetector>().targetCoordinate;
            var success =
                island.PlaceBlockAt(coord);
            if (success)
            {
                BeRemovedFromNowBlock();
                Destroy(gameObject);
            }
            else
                BoundDialog(trigger.GetComponent<DialogTrigger>());
        }

        public override bool CanPutInWater(GrabTrigger trigger)
        {
            var gridDetector = trigger.GetComponent<GridDetector>();
            if(gridDetector.targetBlock) return false;
            var island = gridDetector.GetStandBlock().island;
            var coord = gridDetector.targetCoordinate;
            var success = island.TestBlockInRange(coord);
            return success;
        }

        private void BoundDialog(DialogTrigger trigger)
        {
            trigger.SelfBark("boundtip");
        }
    }
}