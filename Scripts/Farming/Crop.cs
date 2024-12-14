using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Farming
{
    public class Crop : Groundable
    {
        public Seed seed;
        [SerializeField, ReadOnly] public ToolScripts.Rarity rarity;

        public void Init(ToolScripts.Rarity r)
        {
            rarity = r;
            Methods.RarityOutline(gameObject, rarity);
        }
    }
}