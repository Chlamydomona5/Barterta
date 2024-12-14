using System;
using Barterta.NPC.Guide;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MissionPanelUI : MonoBehaviour
{
    [SerializeField] private float panelWidth;
    
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI missionDescriptionText;
    [SerializeField] private Image missionFinishImage;
    
    private RectTransform _rectTransform;
    Sequence _tween;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.anchoredPosition = new Vector2(panelWidth, 0) + _rectTransform.anchoredPosition;
        
    }

    public void StartMission(NPCMission mission)
    {
        if (_tween != null && !_tween.IsComplete())
        {
            _tween.AppendInterval(2f);
            _tween.onComplete += () => SetMission(mission);
        }
        else
        {
            SetMission(mission);
        }
    }

    private void SetMission(NPCMission mission)
    {
        missionFinishImage.transform.localScale = Vector3.zero;
        missionNameText.text = mission.MissionName;
        missionDescriptionText.text = mission.MissionDescription;
        _rectTransform.DOAnchorPosX(-panelWidth + _rectTransform.anchoredPosition.x, 2f).SetEase(Ease.OutBack);
    }

    public void FinishMission()
    {
        Sequence sequence = DOTween.Sequence();
        _tween = sequence;
        sequence.Append(missionFinishImage.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InElastic));
        sequence.Append(_rectTransform.DOAnchorPosX(panelWidth + _rectTransform.anchoredPosition.x, 1f)
            .SetEase(Ease.InBack));

    }
}