using System.Collections;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.UI.WorldUI;
using UnityEngine;

namespace Barterta.Interactable
{
    public abstract class Processor : Groundable, IConsumeGroundable
    {
        [SerializeField] protected float processTime = 10f;
        [SerializeField] protected Vector2Int resCountRange = new Vector2Int(1, 1);
        protected float Timer;
        private bool _isProcessing;
        protected PlayerStayUI _playerStayUI;

        private void Start()
        {
            _playerStayUI = GetComponentInChildren<PlayerStayUI>();
        }

        protected abstract bool Judge(Groundable groundable, GrabTrigger trigger = null);
    
        public virtual bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
        {
            if (_isProcessing) return false;
            return Judge(groundable, trigger);
        }

        public abstract void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null);

        protected abstract Groundable GetResult(Groundable putIn);
    
        protected abstract void ResultProcess(Groundable resultInstance);

        public virtual void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            TryStartProcess(groundable);
        }

        protected void TryStartProcess(Groundable groundable)
        {
            if (!_isProcessing)
                StartCoroutine(ProcessCoroutine(GetResult(groundable)));
        }

        protected virtual void SetUI()
        {
            _playerStayUI.SetProgressBar(Timer / processTime);
        }

        private IEnumerator ProcessCoroutine(Groundable res)
        {
            _isProcessing = true;

            while (Timer < processTime)
            {
                Timer += UnityEngine.Time.fixedDeltaTime;
                SetUI();
                yield return new WaitForFixedUpdate();
            }

            var island = GetComponent<Groundable>().blockUnder.island;
            var coord = GetComponent<Groundable>().blockUnder.coordinate;

            if(res)
                for (int i = 0; i < Random.Range(resCountRange.x, resCountRange.y); i++)
                {
                    var block = island.GetRandomSurroundStackableBlock(coord);
                    var resInstance = Instantiate(res);
                    resInstance.SetOn(block);
                    ResultProcess(resInstance);   
                }


            Timer = 0;
            SetUI();
            _isProcessing = false;
            
            OnProcessEnd();
        }

        protected virtual void OnProcessEnd()
        {
            
        }
    }
}