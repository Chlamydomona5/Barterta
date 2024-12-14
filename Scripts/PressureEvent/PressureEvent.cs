using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class PressureEvent
{
    [ReadOnly] public PressureEventType Type;
    [MinMaxSlider(0, 30)] public Vector2Int Period;
    public int Duration => Period.y - Period.x;
    
    public abstract void StartEvent(PressureEventController controller);
    public abstract void EndEvent(PressureEventController controller);
}

public enum PressureEventType
{
    Storm,
    MonsterRaid
}
