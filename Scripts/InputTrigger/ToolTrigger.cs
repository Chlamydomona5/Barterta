using System.Collections;
using Barterta.ItemGrid;
using Barterta.StaminaAndHealth;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class ToolTrigger : InputTriggerBase
    {
        private static readonly int IsBoosting = Animator.StringToHash("isBoosting");
        private static readonly int Releasing = Animator.StringToHash("Releasing");
        [SerializeField] [ReadOnly] private Tool.Tool nowTool;
        [SerializeField] [ReadOnly] private GroundBlock targetBlock;
        public float stageLimit;
        private Coroutine _boostingCoroutine;

        private float _boostTimer;
        private bool _grabKeyCanceled;
        [SerializeField] private AttributeContainer _staminaContainer;
        private BackpackTrigger _backpackTrigger;

        private string _nowAnimBoolName;

        private void Start()
        {
            _backpackTrigger = GetComponent<BackpackTrigger>();
            var animTrigger = GetComponentInChildren<AnimationTrigger>();
            animTrigger.toolBoostEnd.AddListener(() =>
            {
                BoostEnd();
            });

            //animTrigger.boostCanEffect.AddListener(() => { _willEffect = true; });
        }

        public void UpdateDurabilityUI()
        {
            _backpackTrigger.UpdateUI();
        }

        private IEnumerator ToolBoosting()
        {
            //_hasUseTool = false;
            //Animator
            ChangeAnimator(true);

            _boostTimer = 0;
            while (true)
            {
                _boostTimer += UnityEngine.Time.fixedDeltaTime;
                nowTool.BoostTimer(_boostTimer);
                yield return new WaitForFixedUpdate();
            }
        }

        private void ChangeAnimator(bool toward)
        {
            Animator.SetBool(IsBoosting, toward);
            if (toward)
            {
                //Bool name by tool
                string boolName;
                if (nowTool.GetComponent<Tool.Tool>().animName != string.Empty)
                    boolName = nowTool.GetComponent<Tool.Tool>().animName;
                else
                    boolName = nowTool.GetComponent<Groundable>().ID;
                //Bool name by Natural Resource
                if (targetBlock && targetBlock.groundablesOn.Count > 0 &&
                    targetBlock.groundablesOn[0].GetComponent<NaturalResouce.NaturalResource>())
                    boolName += targetBlock.groundablesOn[0].GetComponent<NaturalResouce.NaturalResource>().toolBoostType;
                _nowAnimBoolName = boolName;
                Animator.SetBool(boolName, true);
            }
            else Animator.SetBool(_nowAnimBoolName, false);
            
            if(!toward) Animator.SetTrigger(Releasing);
        }

        private void BoostEnd()
        {
            if (_boostingCoroutine != null)
            {
                StopCoroutine(_boostingCoroutine);
                _boostingCoroutine = null;

                //Animator
                ChangeAnimator(false);

                nowTool.boostTime = _boostTimer;
                //Consume Stamina
                _staminaContainer.Consume(1);
                //_willEffect = false;
            }
            else
            {
                StateController.ChangeToDefault();
            }
        }

        //Load on the animation trigger by unity event
        public void UseUtensil()
        {
            //_hasUseTool = true;
            
            StateController.ChangeToDefault();

            var ifContinue = nowTool.Effect(targetBlock, this);
            UpdateDurabilityUI();

            //if player still press grab, then continusily use tool.
            if (nowTool.durability > 0 && !_grabKeyCanceled && ifContinue)
                StartBoosting(nowTool, targetBlock);
        }

        public void StartBoosting(Tool.Tool tool, GroundBlock target)
        {
            _grabKeyCanceled = false;
            if (!_staminaContainer.isZero)
            {
                //Face to target
                mouseHandler.TurnToMouseDirection();

                StateController.ChangeAllState(this);
                nowTool = tool;
                targetBlock = target;
                //In case player can't re boost
                if (_boostingCoroutine != null) StopCoroutine(_boostingCoroutine);
                //Start Boost
                _boostingCoroutine = StartCoroutine(ToolBoosting());
            }
            else
            {
                GetComponent<DialogTrigger>().SelfBark("staminaout");
            }
        }

        public override void OnInteractPress(InputAction.CallbackContext ctx)
        {
            
        }

        public override void OnShortGrab(InputAction.CallbackContext ctx)
        {
        }

        public override void OnLongGrab(InputAction.CallbackContext ctx)
        {
        }

        public override void OnInteractCancel(InputAction.CallbackContext ctx)
        {
            //if(!_hasUseTool)
            BoostEnd();
            _grabKeyCanceled = true;
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
            
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
        }

        public override void EmergentExit()
        {
            BoostEnd();
            UseUtensil();
        }
    }
}