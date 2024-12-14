using Barterta.UI.WorldUI;
using DamageNumbersPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Barterta.StaminaAndHealth
{
    public class AttributeContainer : MonoBehaviour
    {
        public string attributeName;
        [SerializeField] public bool changeVisible;
        [ReadOnly] public bool isZero;
        [SerializeField] private float maxValue;

        private float _originalLength;
        private float _originalmaxValue;

        public float MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                //Change Bar's Length
                var rectTransform = barUI.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _originalLength * value / _originalmaxValue);
            }
        }
        
        private float _nowValue;
        public float NowValue
        {
            get => _nowValue;
            set
            {
                _nowValue = Mathf.Clamp(value, 0, MaxValue);
                
                if (value <= 0 && !isZero)
                {
                    _nowValue = 0;
                    isZero = true;
                    onZero.Invoke();
                }

                if (value > 0)
                {
                    isZero = false;
                }
                
                SetUI();
            }
        }
        
        [SerializeField] private ProgressBarUI barUI;
        private Tween _nowTween;
        private DamageNumber _damageNumber;

        public UnityEvent onZero;
        
        private void Start()
        {
            _damageNumber = Resources.Load<DamageNumber>("UI/World/DamageNumber");
            //Set stamina to max
            NowValue = MaxValue;

            _originalLength = barUI.GetComponent<RectTransform>().rect.height;
            _originalmaxValue = MaxValue;
        }

        public void Consume(float num)
        {
            NowValue -= num;
            SetUI();
            //DamageNumber
            if(changeVisible)
                Instantiate(_damageNumber).Spawn(transform.position + Random.onUnitSphere * .2f,num);
        }

        private void SetUI()
        {
            if (_nowTween != null && _nowTween.active) _nowTween.Kill();
            _nowTween = barUI.ChangeToContinusly(NowValue / MaxValue);
        }

        public void Resume(float num)
        {
            NowValue = NowValue + num;
            SetUI();
        }

        public void ResumeAll()
        {
            Resume(MaxValue);
        }
        
        public void GainMaxValue(float num)
        {
            MaxValue += num;
            NowValue += num;
            SetUI();
        }
    }
}