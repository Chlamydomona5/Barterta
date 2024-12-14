using System.Collections.Generic;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Island.SO.IslandType
{
    [CreateAssetMenu(fileName = "IslandType", menuName = "IslandType")]
    public class IslandForm : SerializedScriptableObject
    {
        public ToolScripts.IslandType type;

        public bool willRefreshFish = false;
        public bool willRefreshGroundResource = true;
        public bool willRefreshMonster = false;
        
        [SerializeField] private List<IslandFeature> _distanceToFeature = new();
        
        public IslandFeature GetIslandFeature(MONO.Island island)
        {
            for (int i = 0; i < Constant.ChunkAndIsland.IndexToDistance.Count; i++)
            {
                if (Constant.ChunkAndIsland.IndexToDistance[i] > island.distanceToCenter)
                {
                    //If the distance is smaller than the last one, return the last one
                    if(_distanceToFeature.Count > i)
                        return _distanceToFeature[i];
                    else
                        return _distanceToFeature[_distanceToFeature.Count - 1];
                }
            }
            return _distanceToFeature[_distanceToFeature.Count - 1];
        }
    }
}