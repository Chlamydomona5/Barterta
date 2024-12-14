using System;
using Barterta.Time.SO;
using TMPro;
using UnityEngine;

public class DaysUI : MonoBehaviour
{
    private DayStatus _dayStatus;
    [SerializeField] private TextMeshProUGUI dayCountText;

    private void Start()
    {
        _dayStatus = Resources.Load<DayStatus>("DayNight/DayStatus");
        _dayStatus.newDayEvent.AddListener(OnNewDay);
    }

    private void OnNewDay()
    {
        dayCountText.text = $"Day {_dayStatus.dayCount}";
    }
}