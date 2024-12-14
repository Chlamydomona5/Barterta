using System.Collections.Generic;
using System.Linq;
using Barterta.Island.SO.UpgradeModule;
using Barterta.ItemGrid;
using Barterta.Sound;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Barterta.Island.SO
{
    [CreateAssetMenu(fileName = "IslandAttribute", menuName = "SO/IslandAttribute")]
    public class ElfAttribute : SerializedScriptableObject
    {
        public ElfType type;
        [Title("Level")] public int level = 1;

        [Title("Refresh")] public float naturalAbundance = .04f;

        [Title("Fish")] public int fishDensity = 3;
        public float fishInterval = 30;


        [Title("Farm")] public float seedAbundance = .5f;
        public float mutationUpPoss = .4f;
        public float mutationDownPoss = .2f;

        [Title("Float")] public float floatInterval = 5;

        [HideInInspector] public Dictionary<Groundable, float> FloatDict = null;

        [Title("Merchant")] 
        public int merchantDensity = 1;
        public float merchantDiscount; 
        public float shrineDiscount;

        [Title("Special")] 
        [OdinSerialize] public SpecialAttribute SpecialAttribute = new();

        [Title("Upgrade")] public List<ElfUpgrade> loadedUpgrades;

        [Title("Base Requirement")] [SerializeField]
        private List<ItemSet> requirementLists;

        [HideInInspector] public AllUpgrades allUpgrades;

        private void OnValidate()
        {
            if (!allUpgrades) allUpgrades = Resources.Load<AllUpgrades>("Island/AllUpgrades");
        }


        public void Upgrade(MONO.Island island, ElfUpgrade upgrade)
        {
            SoundManager.I.PlaySound("Shrine upgrade");

            level++;
            loadedUpgrades.Add(upgrade);
            upgrade.LoadInEffect(island);
        }

        //count can't bigger than 64 (8x8)
        public Dictionary<string, int> GetRequirement(ElfUpgrade upgrade = null)
        {
            var dict = new Dictionary<Groundable, int>();
            int needValue = Constant.MerchantAndShrine.ActiviateValueRequirement;
            var requireList = requirementLists;
        
            if (upgrade)
            {
                //TODO: may need change formula
                //value = rarity + levelCost
                needValue = Constant.MerchantAndShrine.LevelToUpgradeCost[level] +
                            Constant.MerchantAndShrine.UpgradeRarityToExtraValueDict[upgrade.rarity];
                requireList = upgrade.requirements;
            }
        
            //Activiate requirement
            int valueSum = 0;
            while (valueSum < needValue)
            {
                Groundable groundable;
                if (dict.Count == 8)
                {
                    groundable = dict.Keys.ToList()[Random.Range(0, dict.Keys.Count)];
                }
                else
                {
                    groundable = requireList[Random.Range(0, requireList.Count)].Random();
                }

                dict.DictAdd(groundable);
                valueSum += groundable.value;
            }
        
            return new Dictionary<string, int>(dict.Select(t => new KeyValuePair<string, int>(t.Key.ID, t.Value)));

        }
    }
}