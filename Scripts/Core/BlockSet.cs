using System.Collections.Generic;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Core
{
    public class BlockSet : MonoBehaviour
    {
        [Title("BlockMap")] public CenteredMap<GroundBlock> BlockMap = new(Constant.ChunkAndIsland.IslandMaxSize);
    

        public virtual bool TestBlockExist(Vector2Int coordinateT)
        {
            //Test if is null
            if (!BlockMap[coordinateT.x, coordinateT.y]) return false;
            return true;
        }

        public List<GroundBlock> GetAllEdgeBlock()
        {
            var temp = new List<GroundBlock>();
            foreach (var gb in BlockMap.Map)
                //if there is at least one empty
                if (gb && GetSurroundBlocksDir4(gb.coordinate).Count < 4)
                    temp.Add(gb);
            return temp;
        }

        public int GetBlockCount()
        {
            var count = 0;
            foreach (var gb in BlockMap.Map)
                if (gb)
                    count++;
            return count;
        }
    
        public List<Vector2> GetEmptyDir4(GroundBlock block)
        {
            var temp = new List<Vector2>();
            var coordinate = block.coordinate;
            foreach (var vector2 in Constant.Direction.Dir4)
                if (TestBlockExist(new Vector2Int(coordinate.x + vector2.x, coordinate.y + vector2.y)))
                    temp.Add(vector2);
            return temp;
        }

        public List<GroundBlock> GetSurroundBlocksDir8(Vector2Int coordinate)
        {
            //Detect four dir block
            var temp = new List<GroundBlock>();
            foreach (var vector2 in Constant.Direction.Dir8)
                if (TestBlockExist(new Vector2Int(coordinate.x + vector2.x, coordinate.y + vector2.y)))
                    temp.Add(BlockMap[coordinate.x + vector2.x, coordinate.y + vector2.y]);
            return temp;
        }

        public List<GroundBlock> GetSurroundBlocksDir4(Vector2Int coordinate)
        {
            //Detect four dir block
            var temp = new List<GroundBlock>();
            foreach (var vector2 in Constant.Direction.Dir4)
                if (TestBlockExist(new Vector2Int(coordinate.x + vector2.x, coordinate.y + vector2.y)))
                    temp.Add(BlockMap[coordinate.x + vector2.x, coordinate.y + vector2.y]);
            return temp;
        }

        //If groundable == null, it means get a empty block.
        public GroundBlock GetRandomSurroundStackableBlock(Vector2Int coordinate, Groundable groundable = null,
            bool secondTry = false)
        {
            var emptyBlocks = new List<GroundBlock>();
            foreach (var block in GetSurroundBlocksDir8(coordinate))
                if (block.groundablesOn.Count == 0 || (groundable && groundable.CanStackOn(block.groundablesOn[0])))
                    emptyBlocks.Add(block);

            if (emptyBlocks.Count > 0)
                return emptyBlocks[Random.Range(0, emptyBlocks.Count)];

            //Only one recurse
            if (!secondTry)
            {
                //Recursive call to find closest empty block
                foreach (var block in GetSurroundBlocksDir8(coordinate))
                    return GetRandomSurroundStackableBlock(block.coordinate, groundable, true);
            }

            return null;
        }

        public GroundBlock GetRandomEmptyBlock()
        {
            //Get all empty blocks
            var emptyBlocks = GetAllBlocks();
            emptyBlocks.RemoveAll(b => b.groundablesOn.Count > 0);
            return emptyBlocks[Random.Range(0, emptyBlocks.Count)];
        }

        public List<GroundBlock> GetAllBlocks()
        {
            var temp = new List<GroundBlock>();
            foreach (var gb in BlockMap.Map)
                if (gb)
                    temp.Add(gb);
            return temp;
        }
        
        public Vector2Int PosToCoordinate(Vector3 pos)
        {
            //Test if it's in range
            var vector3 = pos - transform.position;
            var vector2 = new Vector2(vector3.x, vector3.z);
            var vector2Raw = vector2 / Constant.ChunkAndIsland.BlockSize;
            var coordx = (int)((Mathf.Abs(vector2Raw.x) + .5f) * Mathf.Sign(vector2Raw.x));
            var coordy = (int)((Mathf.Abs(vector2Raw.y) + .5f) * Mathf.Sign(vector2Raw.y));
            var coord = new Vector2Int(coordx, coordy);
            return coord;
        }
    }
}   
