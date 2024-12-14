using System;
using System.Collections.Generic;
using Barterta.Core;
using Barterta.Facility;
using Barterta.Fishing;
using Barterta.ItemGrid;
using Barterta.Mark;
using Barterta.NaturalResouce;
using Barterta.Time.SO;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.NPC.Guide
{
    public class NPCMission
    {
        public string Id;
        public NPCMissionType Type;
        [Title("Info")]        
        public string MissionName => Methods.GetLocalText("mission_name_" + Id);
        public string MissionDescription => Methods.GetLocalText("mission_description_" + Id);
        [Title("Reward")]
        public Dictionary<Groundable,int> Rewards = new Dictionary<Groundable, int>();
        public List<FacilityRecipe> RewardBlueprintInfos;
        [Title("Params")]        
        [ShowIf("@Type == NPCMissionType.HoldItem")]
        public Groundable NeedItem;
        [ShowIf("@Type == NPCMissionType.Island")]
        public int IslandCount = 1;
        [ShowIf("@Type == NPCMissionType.Bowl")]
        public WaterType WaterType;
        [ShowIf("@Type == NPCMissionType.Facility")]
        public string FacilityName;
        [ShowIf("@Type == NPCMissionType.MultipleFacility")]
        public List<string> FacilityNames;

        public NPCMission(string id, NPCMissionType type, Groundable needItem = null)
        {
            Id = id;
            Type = type;
            NeedItem = needItem;
        }

        public bool JudgeFinish(List<Groundable> holdItems, Island.MONO.Island island, DayStatus dayStatus)
        {
            switch (Type)
            {
                case NPCMissionType.HoldItem:
                    return (holdItems.Find(x => x.ID == NeedItem.ID));
                case NPCMissionType.HoldFish:
                    return holdItems.Find(x => x.GetComponent<FishFeatureContainer>());
                case NPCMissionType.Island:
                    var container = Resources.Load<MarkContainer>("IslandMarkContainer");
                    return container.markList.Count >= IslandCount;
                case NPCMissionType.Sleep:
                    return dayStatus.dayCount > 1;
                case NPCMissionType.Bowl:
                    return (holdItems.Find(x => x.GetComponent<WaterContainer>() && x.GetComponent<WaterContainer>().WaterType == WaterType));
                case NPCMissionType.Facility:
                    return HomeManager.I.IsFacilityExist(FacilityName);
                case NPCMissionType.MultipleFacility:
                    foreach (var str in FacilityNames)
                    {
                        if (!HomeManager.I.IsFacilityExist(str)) return false;
                    }
                    return true;
                case NPCMissionType.Nothing:
                    return true;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public enum NPCMissionType
    {
        HoldItem,
        HoldFish,
        Island,
        Sleep,
        Bowl,
        Facility,
        MultipleFacility,
        Nothing
    }
}