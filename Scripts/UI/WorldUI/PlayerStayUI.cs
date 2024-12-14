using System;
using Barterta.UI.UIManage;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Barterta.UI.WorldUI
{
    public class PlayerStayUI : PlayerStayTriggerCollider
    {
        [HideInInspector] public UIObject uiInstance;
        [SerializeField] private UIObject uiPrefab;

        public UnityEvent<UIObject> onReveal;
        
        private Sequence _hideSquence;

        private WorldUIManager _uiManager;

        private void Awake()
        {
            _uiManager = WorldUIManager.I;
            uiInstance = _uiManager.GenerateUI(uiPrefab, transform);
            _uiManager.HideUI(uiInstance);
        }

        protected override void OnPlayerStay(bool enter)
        {
            base.OnPlayerStay(enter);

            if (enter)
            {
                _hideSquence?.Complete();
                //If on destroy, first compelete it and start appeal
                _uiManager.AppealUI(uiInstance);
                onReveal?.Invoke(uiInstance);
            }
            else
            {
                if (uiInstance)
                    _hideSquence = _uiManager.HideUI(uiInstance);
            }
        }

        public void SetProgressBar(float process)
        {
            //Debug.Log("Set ProgressBar To " + process);
            if (uiInstance)
            {
                var progressBar = uiInstance.GetComponentInChildren<ProgressBarUI>();
                if(progressBar) progressBar.ChangeTo(process);
            }
        }

        private void OnDestroy()
        {
            if (gameObject.scene.isLoaded && uiInstance)
            {
                Destroy(uiInstance.gameObject);
            }
        }
    }
}