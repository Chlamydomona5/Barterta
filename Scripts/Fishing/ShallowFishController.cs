using System.Collections;
using System.Linq;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Fishing
{
    public class  ShallowFishController : FishControllerBase
    {
        private Island.MONO.Island _island;
        
        public void IslandInit(Island.MONO.Island island)
        {
            _island = island;
            fishTypeList = Resources.LoadAll<FishFeatureContainer>("Fish").Where(x => x.feature.rarity != ToolScripts.Rarity.Legend).ToList();
            Init((int)island.attribute.fishInterval, island.attribute.fishDensity);
        }

        protected override void InsFish(FishFeatureContainer fishFeature = null)
        {
            var fish = Instantiate(fishInWaterPrefab, transform);

            var edgeList = _island.GetAllEdgeBlock();

            var block = edgeList[Random.Range(0, edgeList.Count)];
            var blockPosition = block.transform.position;
            //Extend from vector from island center to block pos
            fish.transform.position =
                blockPosition + Methods.YtoZero(blockPosition - _island.centerPoint).normalized * Random.Range(6f, 10f) -
                Vector3.up * .5f;
            fish.Init(block.transform, GetFish());
            
            fishList.Add(fish);
        }
    }
}