using Barterta.NPC.Guide;
using Sirenix.OdinInspector;
using UnityEngine;

public class MissionContainer : SerializedMonoBehaviour
{
    public NPCMission CurrentMission;
    
    [SerializeField] private MissionPanelUI missionPanelUI;

    public void StartMission(NPCMission mission)
    {
        //Log
        Debug.Log("Start Mission");
        CurrentMission = mission;
        missionPanelUI.StartMission(mission);
    }
    
    public void FinishMission(NPCMission mission)
    {
        if (CurrentMission != mission) return;
        CurrentMission = null;
        missionPanelUI.FinishMission();
    }
}