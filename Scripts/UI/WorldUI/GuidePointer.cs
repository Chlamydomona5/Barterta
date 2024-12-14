using DG.Tweening;
using UnityEngine;

namespace Barterta.UI.WorldUI
{
    public class GuidePointer : MonoBehaviour
    {
        private void Start()
        {
            //transform.DORotate(new Vector3(0, 180, 0), 5f).SetLoops(-1).SetEase(Ease.Linear);
            transform.DOLocalMoveY(transform.localPosition.y + .35f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
    }
}