using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Island.SO.UpgradeModule
{
    [CreateAssetMenu(fileName = "AttributeUpgrade", menuName = "SO/Upgrade/AttributeUpgrade")]
    public class AttributeUpgrade : ElfUpgrade
    {
        [Title("Add")]
        [SerializeField] private int fishDensityChange;
        [SerializeField] private int merchantDensityChange;
    
        [SerializeField] private float naturalAbundanceChange;
        [SerializeField] private float seedAbundanceChange;
        [SerializeField] private float floatIntervalChange;
        [SerializeField] private float fishIntervalChange;
        [SerializeField] private float mutationPossChange;
        [SerializeField] private float shrineDiscountChange;
        [SerializeField] private float merchantDiscountChange;

        public override void LoadInEffect(MONO.Island island)
        {
            var attr = island.attribute;
            attr.fishDensity += fishDensityChange;
            attr.merchantDensity += merchantDensityChange;

            attr.floatInterval *= floatIntervalChange;
            attr.mutationUpPoss *= mutationPossChange;
            attr.seedAbundance *= seedAbundanceChange;
            attr.naturalAbundance *= naturalAbundanceChange;
            attr.fishInterval *= fishIntervalChange;
            attr.shrineDiscount *= shrineDiscountChange;
            attr.merchantDiscount *= merchantDiscountChange;
        }
    }
}