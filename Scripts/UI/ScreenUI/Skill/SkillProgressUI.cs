using Barterta.Sound;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.ScreenUI.Skill
{
    public class SkillProgressUI : MonoBehaviour
    {
        public string skillName;
        [SerializeField] private Image progress;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI levelText;

        private int _lastLevel = 1;

        public void UpdateSlots(int level, int nowXP, int maxXP)
        {
            var seq = DOTween.Sequence();
            while (_lastLevel < level)
            {
                seq.Append(progress.DOFillAmount(1f, 1f).SetEase(Ease.OutExpo).OnComplete(delegate
                {
                    levelText.text = (int.Parse(levelText.text) + 1).ToString();
                    progress.fillAmount = 0;
                    SoundManager.I.PlaySound("Upgrade");
                }));
                seq.Append(background.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .15f));
                seq.Append(background.transform.DOScale(new Vector3(1f, 1f, 1f), .15f));
                _lastLevel++;
            }
            var percentage = (float)nowXP / maxXP;
            seq.Append(progress.DOFillAmount(percentage, 1f).SetEase(Ease.OutExpo));
        }
    }
}