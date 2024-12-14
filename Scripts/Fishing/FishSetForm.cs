using System.Collections.Generic;
using Barterta.Fishing;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FishSetForm", menuName = "SO/FishSetForm")]
public class FishSetForm : SerializedScriptableObject
{
    public List<FishSet> DistanceToFishSets = new();
    
    public FishSet GetFishSet(Transform trans)
    {
        //TODO: Copy from IslandForm.cs
        for (int i = 0; i < Constant.ChunkAndIsland.IndexToDistance.Count; i++)
        {
            if (Constant.ChunkAndIsland.IndexToDistance[i] > trans.position.magnitude)
            {
                //If the distance is smaller than the last one, return the last one
                if(DistanceToFishSets.Count > i)
                    return DistanceToFishSets[i];
                else
                    return DistanceToFishSets[DistanceToFishSets.Count - 1];
            }
        }
        return DistanceToFishSets[DistanceToFishSets.Count - 1];
    }
}

public class FishSet
{
    public Dictionary<FishFeatureContainer, int> FishWithNumber = new();
}