using Barterta.Farming;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.NaturalResouce
{
    public class Weed : NaturalResource
    {
        [SerializeField] private Groundable boxPrefab;

        protected void DropAt(GroundBlock block, Groundable groundable = null)
        {
            //Seed Abundance
            if (Random.value < block.island.attribute.seedAbundance)
            {
                var box = Instantiate(boxPrefab);
                box.GetComponent<SeedBox>().Init(Methods.GetRandomValueInDict(block.island.islandForm.GetIslandFeature(block.island).SeedDict), ToolScripts.Rarity.Common);
                box.SetOn(block);   
            }
        }

        protected override void DestroyDrop()
        {
            var groundBlock = GetComponent<Groundable>().blockUnder;
            DropAt(groundBlock.island.GetRandomSurroundStackableBlock(groundBlock.coordinate));
            base.DestroyDrop();
        }
    }
}