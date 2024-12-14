using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.WorldUI
{
    public class ColoredBarUI : ProgressBarUI
    {
        [SerializeField] Image backGroundImage;
        [SerializeField] private float changeThreshold = .25f;
        [SerializeField] private Color healthyColor = Color.green;
        [SerializeField] private Color unhealthyColor = Color.gray;

        private void Start()
        {
            ChangeColor(1);
        }

        public override void ChangeTo(float percentage)
        {
            base.ChangeTo(percentage);
            ChangeColor(percentage);
        }
        public override Tween ChangeToContinusly(float percentage)
        {
            ChangeColor(percentage);
            return base.ChangeToContinusly(percentage);
        }
    
        private void ChangeColor(float percentage)
        {
            if(percentage > changeThreshold)
            {
                backGroundImage.color = healthyColor * .5f;
                progressImage.color = healthyColor;
            }
            else
            {
                backGroundImage.color = unhealthyColor * .5f;
                progressImage.color = unhealthyColor;
            }
        }

    }
}