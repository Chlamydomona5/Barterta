using System.Collections.Generic;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Island.SO.UpgradeModule
{
    [CreateAssetMenu(fileName = "FloatUpgrade", menuName = "SO/Upgrade/FloatUpgrade")]
    public class FloatUpgrade : ElfUpgrade
    {
        [SerializeField] private Dictionary<Groundable, float> _newFloatDict;

        public override void LoadInEffect(MONO.Island island)
        {
            island.attribute.FloatDict = _newFloatDict;
        }
    }
}