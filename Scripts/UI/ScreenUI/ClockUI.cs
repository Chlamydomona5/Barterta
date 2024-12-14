using Barterta.Time.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.ScreenUI
{
    public class ClockUI : MonoBehaviour
    {
        private DayStatus _dayStatus;
        [SerializeField] private GameObject needle;
        [SerializeField] private Image backgroundProcess;
        [SerializeField] private Image background;
        
        [SerializeField] private Sprite stormBackground;
        [SerializeField] private Sprite normalBackground;

        private void Start()
        {
            _dayStatus = Resources.Load<DayStatus>("DayNight/DayStatus");
        }

        private void FixedUpdate()
        {
            needle.transform.localRotation = Quaternion.Euler(0,0,- 360 * _dayStatus.nowTime / _dayStatus.timeOfOneDay);
            backgroundProcess.fillAmount = _dayStatus.nowTime / _dayStatus.timeOfOneDay;
        }
        
        public void LoadEvent(bool isStorm)
        {
            background.sprite = isStorm ? stormBackground : normalBackground;
            backgroundProcess.sprite = isStorm ? stormBackground : normalBackground;
        }
    }
}