using Barterta.InputTrigger;
using Barterta.Interactable;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Farming
{
    public class BreedingBox : Processor
    {
        [SerializeField] private SeedBox seedBoxPrefab;
        private Seed _seed;
        private ToolScripts.Rarity _currentRarity;
        protected override bool Judge(Groundable groundable, GrabTrigger trigger = null)
        {
            return groundable.GetComponent<Crop>();
        }

        public override void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null)
        {
        }

        protected override Groundable GetResult(Groundable putIn)
        {
            var crop = putIn.GetComponent<Crop>();
            _currentRarity = crop.rarity;
            _seed = crop.seed;
            return seedBoxPrefab.GetComponent<Groundable>();
        }

        protected override void ResultProcess(Groundable resultInstance)
        {
            resultInstance.GetComponent<SeedBox>().Init(_seed.GetComponent<Groundable>(), _currentRarity);
        }
    }
}