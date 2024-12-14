using Barterta.UI.WorldUI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Barterta.UI.ScreenUI
{
    public class BlackboardResourceSlotUI : MonoBehaviour
    {
        public string id;
        public TextMeshProUGUI countText;
        public ProgressBarUI progressBarUI;
        private void Awake()
        {
            countText = GetComponentInChildren<TextMeshProUGUI>();
            progressBarUI = GetComponentInChildren<ProgressBarUI>();
        }
    }
}