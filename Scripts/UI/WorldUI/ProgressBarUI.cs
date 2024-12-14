using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.WorldUI
{
    public class ProgressBarUI : UIObject
    {
        [SerializeField] protected Image progressImage;

        public virtual void ChangeTo(float percentage)
        {
            progressImage.fillAmount = percentage;
        }
    
        public virtual Tween ChangeToContinusly(float percentage)
        {
            return progressImage.DOFillAmount(percentage, .5f);
        }
    }
}