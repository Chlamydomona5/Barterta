using System.Collections.Generic;
using Barterta.Mark;
using Barterta.Time.SO;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Time.MONO
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] private Light directionalLight;
        [SerializeField] private List<DayStagePreset> presetList;
        [SerializeField, InlineEditor()] private DayStatus status;

        [SerializeField] private float switchProcessTime;
        private void Start()
        {
            //Listen since new day will set to 0, and 9 need to light update
            status.dayStageEvent.AddListener(StageSwitch);

            status.dayCount = 0;
            status.NewDay();
            
            //Set light to 0
            directionalLight.transform.rotation = Quaternion.Euler(new Vector3(0, 170f, 60f));
            directionalLight.transform.DORotate(
                new Vector3(180f, 170f, 60f), status.timeOfOneDay).SetLoops(-1);
        }

        private void FixedUpdate()
        {
            //Update Time
            status.nowTime += UnityEngine.Time.fixedDeltaTime;
            //TODO: Temp no force end
            if (status.nowTime > status.timeOfOneDay) status.NewDay();
            //0~1 float
            LightUpdate(status.nowTime / status.timeOfOneDay);
        }

        private void LightUpdate(float dayPercent)
        {
            //only if not reacg
            if (status.NowStage < presetList.Count - 1)
                if (dayPercent >= presetList[status.NowStage + 1].percentage)
                {
                    status.NowStage++;
                }
        }

        private void StageSwitch(int stageNum)
        {
            var preset = presetList[stageNum];
            //if 0, means newday, then switch instantly
            DOTween.To(() => RenderSettings.ambientLight, x => RenderSettings.ambientLight = x, preset.ambientColor,
                switchProcessTime);
            DOTween.To(() => directionalLight.color, x => directionalLight.color = x, preset.lightColor,
                switchProcessTime);
        }
    }
}