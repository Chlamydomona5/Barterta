using System;
using Barterta.UI.WorldUI;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TipUI : UIObject
{
    [SerializeField] private float moveDistance = 100;
    [SerializeField] private RectTransform transformToMove;
    [SerializeField] private TextMeshProUGUI unlockText;

    public void Init(string str)
    {
        unlockText.text = "解锁合成: " + str;
        transformToMove.anchoredPosition = new Vector2(transformToMove.anchoredPosition.x - moveDistance,
            transformToMove.anchoredPosition.y);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transformToMove.DOAnchorPosX(transformToMove.anchoredPosition.x + moveDistance, .5f)
            .SetEase(Ease.OutBack));
        sequence.AppendInterval(2f);
        sequence.Append(transformToMove.DOAnchorPosX(transformToMove.anchoredPosition.x - moveDistance, .2f)
            .SetEase(Ease.InBack));
    }
}