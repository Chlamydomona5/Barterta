using Barterta.Core;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Boat
{
    public class BoatBlock : GroundBlock
    {
        public override BlockSet BlockSet => island ? island : belongBoat;
        public override Vector2Int Coordinate => island ? coordinate : boatCoordinate;

        public Boat belongBoat;
        public Vector2Int boatCoordinate;

        private void GenerateBoat()
        {
            //Log
            //Debug.Log("Generate a boat");
            //Create a parent object
            var boat = new GameObject("Boat");
            boat.transform.position = transform.position;
            transform.SetParent(boat.transform);
        
            belongBoat = boat.AddComponent<Boat>();
            belongBoat.Init(island);
        
            boat.GetComponent<Boat>().BlockMap[0, 0] = this;
            boatCoordinate = new(0, 0);
        }

        public override void BePlaced()
        {
            base.BePlaced();
            GetBoat(coordinate);
        }

        /// <summary>
        /// Check if there's boat block around, if not, generate a boat.
        /// </summary>
        /// <param name="coordniate"></param>
        private void GetBoat(Vector2Int coordniate)
        {
            if(!CheckNeighbor(coordniate))
                GenerateBoat();
        }

        private bool CheckNeighbor(Vector2Int coordniate)
        {
            //Find if there's boat block around
            foreach (var dir in Constant.Direction.Dir4)
            {
                var block = island.BlockMap[dir + coordniate];
                if (block && block is BoatBlock)
                {
                    belongBoat = ((BoatBlock)block).belongBoat;
                    boatCoordinate = ((BoatBlock)block).boatCoordinate - dir;
                    belongBoat.AddBlock(boatCoordinate, this);
                
                    //Log
                    //Debug.Log("Find a boat, Add at " + boatCoordinate);
                    return true;
                }
            }

            return false;
        }


    }
}