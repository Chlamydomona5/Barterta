using System.Collections.Generic;
using Barterta.ItemGrid;
using Barterta.Rarity;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;

namespace Barterta.Island.SO.UpgradeModule
{
    public abstract class ElfUpgrade : SerializedScriptableObject, IRarity
    {
        public ToolScripts.Rarity rarity;
        public ElfType type;
        public bool canOverlay = true;
        public List<ItemSet> requirements;

        public abstract void LoadInEffect(MONO.Island island);
    
        public ToolScripts.Rarity Rarity => rarity;
    }
}