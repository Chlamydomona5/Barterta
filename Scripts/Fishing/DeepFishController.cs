using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Barterta.Fishing;
using Barterta.Island.MONO;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

public class DeepFishController : FishControllerBase
{
    private FishSet _fishSet;
    
    protected override void InsFish(FishFeatureContainer fishFeature = null)
    {
        var fish = Instantiate(fishInWaterPrefab, transform, false);
        fish.Init(transform, fishFeature ? fishFeature : GetFish(), false);
        fish.transform.localPosition = Vector3.zero;
        fishList.Add(fish);
    }

    public override void Init(int fishInterval, int fishDensity)
    {
        var formList = Resources.LoadAll<FishSetForm>("FishSetForm");
        _fishSet = formList[Random.Range(0, formList.Length)].GetFishSet(transform);
        fishTypeList = Resources.LoadAll<FishFeatureContainer>("Fish")
            .Where(x => x.feature.rarity != Rarity.Common).ToList();
        base.Init(fishInterval, fishDensity);
        //Instantiate fish in fish set
        foreach (var fish in _fishSet.FishWithNumber)
        {
            for (int i = 0; i < fish.Value; i++)
            {
                InsFish(fish.Key);
            }
        }
    }
}