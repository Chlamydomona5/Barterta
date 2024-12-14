using Barterta.UI.WorldUI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Barterta.DeadTimer
{
    public class DeadTimer : MonoBehaviour
    {
        [ReadOnly] public float deadCounter;
        [SerializeField] private float maxDeadTimeLimit;
        [SerializeField] private float currentDeadTimeLimit;
        public bool isPaused;
        private ColoredBarUI _coloredBarUi;
        private TMP_Text _text;

        private void Start()
        {
            deadCounter = currentDeadTimeLimit;
        
            _coloredBarUi = GetComponentInChildren<ColoredBarUI>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void FixedUpdate()
        {
            if(isPaused) return;
            if (deadCounter >= 0)
            {
                deadCounter -= UnityEngine.Time.fixedDeltaTime;
                _coloredBarUi.ChangeTo(deadCounter / maxDeadTimeLimit);
                _text.text = ((int) deadCounter).ToString();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    
        public void AddTime(float time)
        {
            deadCounter += time;
            if (deadCounter > currentDeadTimeLimit)
            {
                deadCounter = currentDeadTimeLimit;
            }
        }
    
        public void AddTimeLimit(float time)
        {
            currentDeadTimeLimit += time;
        }
    }
}