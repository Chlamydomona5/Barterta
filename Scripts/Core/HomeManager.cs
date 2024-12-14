using System;
using System.Collections.Generic;
using Barterta.Facility;
using Barterta.Island;
using Barterta.Island.MONO;
using Barterta.NPC.Guide;
using Barterta.Time.SO;
using Barterta.ToolScripts;
using Barterta.UI.ScreenUI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Barterta.Core
{
    public class HomeManager : Singleton<HomeManager>
    {
        [Title("Blackboard Resource")] public BlackboardResource BlackboardResource = new();
        public BlackboardResource BlackboardResourceCapacity = new();
        public Dictionary<string, bool> BlackboardResourceEnableList = new();
        [SerializeField] private PressureEventPanelUI pressureEventUI;

        [Title("Scene Reference")] public HomeIsland homeIsland;
        public Shrine.Shrine Shrine => homeIsland.shrine;
        [SerializeField] private BlackboardResourceUI ui;

        [Title("Observe")] [SerializeField] private List<FacilityEntity> facilities = new();
        [SerializeField] private List<HomeMember> members = new();

        [Title("Temp")] [SerializeField] private Transform deadTimerUI;
        [SerializeField] TextMeshProUGUI deadTimerText;

        [SerializeField] private float deadTime;

        [FormerlySerializedAs("deadTimer")] [SerializeField, ReadOnly]
        private float erodeTimer;

        private bool _countingToErode = false;
        private Coroutine _erodeCoroutine;
        private bool _isEroding = false;
        

        public bool IsEroding
        {
            get => _isEroding;
            set
            {
                _isEroding = value;
                //Start or interrupt coroutine according to value
                if (_isEroding)
                {
                    _erodeCoroutine = StartCoroutine(homeIsland.ErosionCoroutine());
                }
                else
                {
                    if (_erodeCoroutine != null) StopCoroutine(_erodeCoroutine);
                }
            }
        }

        public override void Awake()
        {
            base.Awake();
            foreach (var id in BlackboardResource.IDList)
            {
                BlackboardResourceEnableList.Add(id, false);
            }
        }

        private void Start()
        {
            foreach (var id in BlackboardResource.IDList)
            {
                //StartBlackboardResource(id);
            }
        }

        public void RegisterMember(HomeMember member)
        {
            members.Add(member);
        }
        
        public void ChangeBlackboardResource(string id, float value)
        {
            if (!BlackboardResourceEnableList.ContainsKey(id)) return;
            
            if(value >= 0)
                Debug.Log(id + value);
            var preValue = BlackboardResource.IDToValueDict[id];
            
            BlackboardResource.IDToValueDict[id] += value;
            BlackboardResource.IDToValueDict[id] = Mathf.Clamp(BlackboardResource.IDToValueDict[id], 0, BlackboardResourceCapacity.IDToValueDict[id]);
            ui.UpdateSlot(id);

            if (BlackboardResource.IDToValueDict[id] < 0.01 && preValue > 0)
                StartDeadTimer();

            if (_countingToErode)
            {
                var endTimer = true;
                foreach (var pair in BlackboardResource.IDToValueDict)
                {
                    if(pair.Value < 0.01)
                        endTimer = false;
                }
                if (endTimer) EndDeadTimer();
            }
        }
        
        public void StartBlackboardResource(string id)
        {
            ui.EnableSlot(id);
            BlackboardResourceEnableList[id] = true;
        }
        
        public void ChangeBlackboardResourceCapacity(string id, float value)
        {
            BlackboardResourceCapacity.IDToValueDict[id] = value;
            ui.UpdateSlot(id);
        }

        private void FixedUpdate()
        {
            ChangeBlackboardResource("food", -30 * UnityEngine.Time.fixedDeltaTime / 900);
            ChangeBlackboardResource("water", -40 * UnityEngine.Time.fixedDeltaTime / 900);
            
            if (_countingToErode && !IsEroding)
            {
                erodeTimer -= UnityEngine.Time.fixedDeltaTime;
                deadTimerText.text = erodeTimer.ToString("F0");
                if (erodeTimer <= 0)
                {
                    IsEroding = true;
                }
            }
        }

        private void StartDeadTimer()
        {
            Debug.Log("StartDeadTimer");
            erodeTimer = deadTime;
            _countingToErode = true;
            deadTimerUI.gameObject.SetActive(true);
        }
        
        private void EndDeadTimer()
        {
            Debug.Log("EndDeadTimer");
            _countingToErode = false;
            IsEroding = false;
            deadTimerUI.gameObject.SetActive(false);
        }
        
        public void RegisterFacility(FacilityEntity facility)
        {
            facilities.Add(facility);
        }
        
        public void UnregisterFacility(FacilityEntity facility)
        {
            if(facilities.Contains(facility))
                facilities.Remove(facility);
        }
        
        public bool IsFacilityExist(string facilityName)
        {
            return facilities.Exists(f => f.ID == facilityName);
        }

        public void SetPressureEventUI(bool active)
        {
            pressureEventUI.gameObject.SetActive(active);
            pressureEventUI.SetToDay(Resources.Load<DayStatus>("DayNight/DayStatus").dayCount);
        }
    }
}