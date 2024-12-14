using UnityEngine;

namespace Barterta.Weapon
{
    [CreateAssetMenu(fileName = "MeleeEnchantment", menuName = "Enchant/MeleeEnchantment")]
    public class MeleeEnchantment : ScriptableObject
    {
        public string id;
        public ToolScripts.Rarity rarity;
        public MeleeAttribute AttributeBonus;
    }
}