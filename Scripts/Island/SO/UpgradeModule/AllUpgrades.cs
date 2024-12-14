using System.Collections.Generic;
using System.Linq;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Island.SO.UpgradeModule
{
    [CreateAssetMenu(fileName = "AllUpgrades", menuName = "SO/AllUpgrades")]
    public class AllUpgrades : ScriptableObject
    {
        public List<ElfUpgrade> upgrades;

        [Button]
        private void Load()
        {
            upgrades = Resources.LoadAll<ElfUpgrade>("").ToList();
        }

        public ElfUpgrade GetRandomUpgradeWithWeight(ElfType type)
        {
            //select type
            List<ElfUpgrade> list;
            if (Random.value < Constant.MerchantAndShrine.SameTypePoss)
            {
                list = upgrades.Where(x => x.type == type).ToList();
            }
            else
            {
                list = upgrades.Where(x => x.type != type).ToList();
            }
        
            //select rarity
            List<ElfUpgrade> rarityList;
            while ((rarityList = list.Where(x => x.rarity == Methods.RandomRarity()).ToList()).Count == 0) ;
            var ret = rarityList[Random.Range(0, rarityList.Count)];
            return ret;
        }

    }
}