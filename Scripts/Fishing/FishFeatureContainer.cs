using Barterta.ItemGrid;
using Barterta.Rarity;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Fishing
{
    public class FishFeatureContainer : Groundable, IRarity
    {
        public override string LocalizeName => Methods.RarityColorText(base.LocalizeName, Rarity);
        [SerializeField] [ReadOnly] public FishFeature feature;

        private void Start()
        {
            GetComponentInChildren<Animator>().enabled = false;
        }

        [Button("GetFeatureByName")]
        private void GetFeatureByName()
        {
            feature = Resources.Load<FishFeature>("FishFeature/" + GetComponent<Groundable>().ID);
        }

        public ToolScripts.Rarity Rarity => feature.rarity;
    }
}