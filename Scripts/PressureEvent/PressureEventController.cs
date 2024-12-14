using System;
using System.Collections.Generic;
using Barterta.Mark;
using Barterta.Time.SO;
using Barterta.UI.ScreenUI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class PressureEventController : SerializedMonoBehaviour
{
    public List<PressureEvent> PressureEvents = new();
    [Button]
    public void SortEvents()
    {
        PressureEvents.Sort((a, b) => a.Period.x.CompareTo(b.Period.x));
    }
    
    [SerializeField] private PressureEventPanelUI pressureEventPanelUI;
    [SerializeField] private ClockUI clockUI;
    
    public Volume beforeStormVolume;
    public Volume afterStormVolume;
    public LightningLight lightningLight;
    [HideInInspector] public MarkContainer playerContainer;
    private DayStatus _dayStatus;
    [SerializeField, ReadOnly] private List<PressureEvent> currentEvents = new();

    private void Awake()
    {
        _dayStatus = Resources.Load<DayStatus>("DayNight/DayStatus");
        playerContainer = Resources.Load<MarkContainer>("PlayerContainer");
        
        _dayStatus.newDayEvent.AddListener(NewDayCheck);
    }

    private void NewDayCheck()
    {
        //Start event that period.x == dayCount
        var nowDayCount = _dayStatus.dayCount;
        var eventsToStart = PressureEvents.FindAll(e => e.Period.x == nowDayCount);
        
        foreach (var pressureEvent in eventsToStart)
        {
            Debug.Log($"Start {pressureEvent.Type} Event, The period is " + $"{pressureEvent.Period.x} ~ {pressureEvent.Period.y}");
            pressureEvent.StartEvent(this);
            currentEvents.Add(pressureEvent);
        }
        //End event that period.y == dayCount - 1
        var eventsToEnd = PressureEvents.FindAll(e => e.Period.y == (nowDayCount - 1));
        foreach (var pressureEvent in eventsToEnd)
        {
            Debug.Log($"End {pressureEvent.Type} Event, The period is " + $"{pressureEvent.Period.x} ~ {pressureEvent.Period.y}");
            currentEvents.Remove(pressureEvent);
            pressureEvent.EndEvent(this);
        }
        
        pressureEventPanelUI.SetToDay(nowDayCount);
        clockUI.LoadEvent(currentEvents.Exists(x => x.Type == PressureEventType.Storm));
    }
}