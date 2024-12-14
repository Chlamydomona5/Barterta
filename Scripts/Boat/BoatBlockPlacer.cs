using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Player;
using UnityEngine;

namespace Barterta.Boat
{
    public class BoatBlockPlacer : Groundable
    {
        [SerializeField] private BoatBlock boatBlockPrefab;
        public void PlaceAt(Island.MONO.Island island, Vector2Int coordinate)
        {
            island.PlaceBoatBlock(coordinate, boatBlockPrefab);
            Destroy(gameObject);
        }

        public override void OnPutInWater(GrabTrigger trigger)
        {
            var island = trigger.GetComponent<GridDetector>().GetStandBlock().island;
            PlaceAt(island, trigger.GetComponent<GridDetector>().targetCoordinate);
        }

        public override bool CanPutInWater(GrabTrigger trigger)
        {
            return !trigger.detector.targetBlock;
        }
    }
}
