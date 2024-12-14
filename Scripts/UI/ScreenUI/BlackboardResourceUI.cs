using System;
using System.Collections.Generic;
using Barterta.Core;
using Barterta.Island;
using Barterta.Island.MONO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace Barterta.UI.ScreenUI
{
    public class BlackboardResourceUI : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<string, BlackboardResourceSlotUI> idToTextDict = new();
        [SerializeField] private BlackboardResourceSlotUI slotPrefab;

        private void Awake()
        {
            //inactive all slots
            foreach (var pair in idToTextDict)
            {
                pair.Value.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            UpdateAllSlots();
        }
        
        public void EnableSlot(string id)
        {
            if (!idToTextDict.ContainsKey(id)) return;
            idToTextDict[id].gameObject.SetActive(true);
        }

        public void UpdateSlot(string id)
        {
            if(!idToTextDict[id].countText || !idToTextDict[id].progressBarUI) return;
            idToTextDict[id].countText.text = HomeManager.I.BlackboardResource.IDToValueDict[id].ToString("F1") + "/" +
                                              HomeManager.I.BlackboardResourceCapacity.IDToValueDict[id].ToString("F1");
            idToTextDict[id].progressBarUI.ChangeTo(HomeManager.I.BlackboardResource.IDToValueDict[id] /
                                                    HomeManager.I.BlackboardResourceCapacity.IDToValueDict[id]);
        }
        
        public void UpdateAllSlots()
        {
            foreach (var id in BlackboardResource.IDList)
            {
                UpdateSlot(id);
            }
        }
    }
}