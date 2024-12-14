using UnityEngine;

namespace Barterta.Island.SO.UpgradeModule
{
    [CreateAssetMenu(fileName = "SpecialUpgrade", menuName = "SO/Upgrade/SpecialUpgrade")]
    public class SpecialUpgrade : ElfUpgrade
    {
        public string varName;
        public bool setValue;
        public override void LoadInEffect(MONO.Island island)
        {
            typeof(SpecialAttribute).GetProperty(varName)?.SetValue(island.attribute.SpecialAttribute, setValue);
        }
    }
}