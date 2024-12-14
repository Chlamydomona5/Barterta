using System;
using System.Collections.Generic;
using UnityEngine;

public class PressureEventPanelUI : MonoBehaviour
{
    [SerializeField] private PressureEventController controller;
    private List<PressureEventSlotUI> _pressureEventSlots = new();
    private void Start()
    {
        //Get all children as PressureEventSlots
        _pressureEventSlots.AddRange(GetComponentsInChildren<PressureEventSlotUI>());
    }

    public void SetToDay(int daycount)
    {
        var events = controller.PressureEvents;
        //Set slots to events
        foreach (var slot in _pressureEventSlots)
        {
            slot.ResetEventIcon();
        }
        //Slot[0] is today, arrange all events which's period is in slots.count range into slots
        foreach (var pressureEvent in events)
        {
            //Align period to window
            int leftEdge = pressureEvent.Period.x - daycount;
            int rightEdge = pressureEvent.Period.y - daycount;
            Vector2Int relativePeriod = new Vector2Int(Mathf.Clamp(leftEdge, 0, 1000),
                Mathf.Clamp(rightEdge, 0, _pressureEventSlots.Count - 1));
            //If period is in window, set event icon
            if (leftEdge < _pressureEventSlots.Count && rightEdge >= 0)
            {
                for (int i = relativePeriod.x; i <= relativePeriod.y; i++)
                {
                    _pressureEventSlots[i].SetEventIcon(pressureEvent.Type);
                }
            }
        }
    }
}