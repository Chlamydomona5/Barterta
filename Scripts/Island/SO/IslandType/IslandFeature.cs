using System.Collections.Generic;
using Barterta.Facility;
using Barterta.ItemGrid;
using Barterta.NPC.Merchant;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Island.SO.IslandType
{
    public class IslandFeature
    {
        [Title("Innate Settings")] [DictionaryDrawerSettings]
        public Dictionary<Material, float> BlockMaterialPossDict = new();
        public Vector2 IslandDisRange = new(5, 11);
        public Vector2 IslandAngleRange = new(5, 30);
        
        public List<GameObject> StartNPCDict = new();
        public Dictionary<Groundable, int> StartGroundResourceDict = new();
        public List<FacilityRecipe> StartBlueprintInfoList = new();
        public List<MerchantIdentifier> StartMerchantList = new();

        [Title("Refresh Settings")]
        public float MonsterRefreshTime = 30f;
        public float MonsterRefreshTimeNoise = 5f;
        public float ResourceRefreshTime = 30f;
        public float ResourceRefreshTimeNoise = 5f;
        [DictionaryDrawerSettings(KeyLabel = "Fish", ValueLabel = "Weight")]
        public List<Groundable> FishPool = new();

        [Space] [DictionaryDrawerSettings(KeyLabel = "Resource", ValueLabel = "Weight")]
        public Dictionary<Groundable, float> RefreshGroundResourceDict = new();

        [DictionaryDrawerSettings(KeyLabel = "Seed", ValueLabel = "Weight")]
        public Dictionary<Groundable, float> SeedDict = new();
    }
}