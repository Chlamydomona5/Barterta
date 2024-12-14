using System.Collections;
using System.Collections.Generic;
using Barterta.Core;
using Barterta.Monster;

public class MonsterRaidEvent : PressureEvent
{
    public List<MonsterWave> MonsterWaves = new();
    public int Interval;

    public MonsterRaidEvent()
    {
        Type = PressureEventType.MonsterRaid;
    }

    public override void StartEvent(PressureEventController controller)
    {
        HomeManager.I.homeIsland.monsterController.StartRaid(MonsterWaves, Interval);
    }

    public override void EndEvent(PressureEventController controller)
    {
    }
}

public class MonsterWave
{
    public Dictionary<Monster,int> MonsterWithCount = new();
}
 