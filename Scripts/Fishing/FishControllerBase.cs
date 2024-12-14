using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Barterta.Fishing;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class FishControllerBase : MonoBehaviour
{
    [SerializeField] protected List<FishFeatureContainer> fishTypeList;
    [SerializeField] [ReadOnly] protected List<FishInWater> fishList = new();
    [SerializeField] protected FishInWater fishInWaterPrefab;
    protected int _fishInterval;
    protected int _fishDensity;

    public virtual void Init(int fishInterval, int fishDensity)
    {
        _fishInterval = fishInterval;
        _fishDensity = fishDensity;
        fishInWaterPrefab = Resources.Load<FishInWater>("Fishing/FishInWaterPrefab");
        StartCoroutine(FishControl());
    }
    
    protected IEnumerator FishControl()
    {
        //TODO: Not Stop when not active, May cost much
        InsFish();
        while (true)
        {
            yield return new WaitForSeconds(_fishInterval);
            fishList.RemoveAll(x => x == null);
            if (fishList.Count < _fishDensity) InsFish();
        }
    }

    protected abstract void InsFish(FishFeatureContainer fishFeature = null);
    
    protected Groundable GetFish()
    {
        Rarity rarity;
        List<FishFeatureContainer> fishes;
        //TODO: May cause infinite loop
        do
        {
            rarity = Methods.GetRandomValueInDict(Constant.FishRarityDict);
            fishes = fishTypeList.Where(x => x.GetComponent<FishFeatureContainer>().feature.rarity == rarity).ToList();
        } while (fishes.Count == 0);        
        return fishes[Random.Range(0, fishes.Count)];
    }
}