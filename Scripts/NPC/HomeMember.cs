using System.Collections.Generic;
using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.Facility;
using Barterta.InputTrigger;
using Barterta.Island;
using Barterta.Player;
using Barterta.Sound;
using Barterta.Time.SO;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.NPC.Guide
{
    public class HomeMember : IDBase, IPressInteractOnGroundEffector
    {
        public BlackboardResourceRatio BlackboardResourceRatio;

        public List<NPCMission> MissonList;
        private DayStatus _dayStatus;

        [Title("Tutorial")] [ReadOnly] public int tutorialProcess;

        private UIObject _pointer;


        [ReadOnly]
        public bool IsFirstAtTutorial
        {
            get => _isFirstAtTutorial;
            set
            {
                _isFirstAtTutorial = value;
                DialogueLua.SetVariable("FirstAtQuest", value);
            }
        }

        private bool _isFirstAtTutorial = true;

        protected void Start()
        {
            _dayStatus = Resources.Load<DayStatus>("DayNight/DayStatus");
            HomeManager.I.RegisterMember(this);

            _pointer = WorldUIManager.I.GenerateUI(Resources.Load<UIObject>("UI/World/Dialog/Pointer"), transform, 0f);
        }

        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return tutorialProcess < MissonList.Count;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            var missionContainer = trigger.GetComponent<MissionContainer>();
            var dialogTrigger = trigger.GetComponent<DialogTrigger>();

            if (missionContainer.CurrentMission != null && missionContainer.CurrentMission != MissonList[tutorialProcess])
            {
                dialogTrigger.StartConversation("cantoverridemission", transform);
                return;
            }

            //Set first varibles
            IsFirstAtTutorial = IsFirstAtTutorial;
            //repeat text next time, no check
            if (IsFirstAtTutorial)
            {
                Dialog();
                IsFirstAtTutorial = false;
                return;
            }
            //if reach needs, no repeat
            else if (MissonList[tutorialProcess].JudgeFinish(trigger.HandBlock.groundablesOn,
                         trigger.GetComponent<GridDetector>().GetStandBlock().island, _dayStatus))
            {
                //Finish Mission
                if (MissonList[tutorialProcess].Type != NPCMissionType.Nothing)
                    missionContainer.FinishMission(MissonList[tutorialProcess]);
                RewardMission(MissonList[tutorialProcess]);
                //Reward
                tutorialProcess++;
                if (tutorialProcess >= MissonList.Count)
                {
                    _pointer.gameObject.SetActive(false);
                }
                //avoid double first
                IsFirstAtTutorial = true;
                //if no, repeat
                SoundManager.I.PlaySound("Mission complete");
                Dialog();
                IsFirstAtTutorial = false;

                return;
            }

            Dialog();
            return;


            void Dialog()
            {
                if (tutorialProcess >= MissonList.Count)
                {
                    return;
                }
                
                //Try to start mission
                if (IsFirstAtTutorial && MissonList[tutorialProcess].Type != NPCMissionType.Nothing)
                    missionContainer.StartMission(MissonList[tutorialProcess]);

                if (tutorialProcess >= MissonList.Count) return;
                //dialog
                dialogTrigger.StartConversation(ID + "_" + MissonList[tutorialProcess].Id, transform);
            }
        }

        private void RewardMission(NPCMission mission)
        {
            //Log
            Debug.Log("Mission " + mission.Id + " Reward");
            var stand = Methods.GetStandBlock(transform);
            //Item Reward
            if (mission.Rewards != null)
                foreach (var pair in mission.Rewards)
                {
                    for (int i = 0; i < pair.Value; i++)
                    {
                        Instantiate(pair.Key)
                            .SetOn(stand.island.GetRandomSurroundStackableBlock(stand.coordinate, pair.Key));
                    }
                }

            //Blueprint reward
            if (mission.RewardBlueprintInfos != null && mission.RewardBlueprintInfos.Count > 0)
            {
                foreach (var blueprintInfo in mission.RewardBlueprintInfos)
                {
                    var prefab = Resources.Load<FacilityBlueprint>("Facility/Blueprint");
                    var blueprint = Instantiate(prefab);
                    blueprint.SetOn(stand.island.GetRandomSurroundStackableBlock(stand.coordinate));
                    blueprint.targetRecipe = blueprintInfo;
                }
            }

            //if is the last mission, teleport to the home island
            if (mission.Equals(MissonList[MissonList.Count - 1]))
            {
                transform.position = new Vector3(0, 1, 0);
            }
        }
    }
}