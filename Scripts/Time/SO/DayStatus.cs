using Barterta.InputTrigger;
using Barterta.Mark;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Barterta.Time.SO
{
    [CreateAssetMenu(fileName = "DayStatus", menuName = "SO/DayStatus")]
    public class DayStatus : ScriptableObject
    {
        public int dayCount;
        public float timeOfOneDay = 300;

        [ReadOnly] [ProgressBar(0, "timeOfOneDay")]
        public float nowTime;

        public UnityEvent newDayEvent;
        public UnityEvent<int> dayStageEvent;

        private bool _isSleeping;

        public int NowStage
        {
            get => nowStage;
            set
            {
                nowStage = value;
                dayStageEvent.Invoke(nowStage);
            }
        }
        [SerializeField] [ReadOnly] private int nowStage;

        public void NewDay()
        {
            nowTime = 0;
            NowStage = 0;
            dayCount++;
            newDayEvent.Invoke();
        }
    }
}