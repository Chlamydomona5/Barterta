using System;
using System.Collections;
using Barterta.Player;
using Barterta.Sound;
using Barterta.Tool;
using Barterta.ToolScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class HookNetTrigger : InputTriggerBase
    {
        [SerializeField] private float releaseSpeedConstant = 10f;
        [SerializeField] private float recollectSpeed;
        [SerializeField] public float stopHeight;
        private Transform _lastParent;
        private HookNet _nowHookNet;
        private BackpackTrigger _backpackTrigger;
        private static readonly int Close = Animator.StringToHash("Close");

        private void Start()
        {
            _backpackTrigger = GetComponent<BackpackTrigger>();
        }

        private void ThrowHook(float time)
        {
            _lastParent = _nowHookNet.transform.parent;
            //Throw
            _nowHookNet.Release(transform.forward * (time * releaseSpeedConstant) + Vector3.up * 5f);
            //Sound
            SoundManager.I.PlaySound("Throw hooknet");
        }

        public void StartHook(HookNet net, float time)
        {
            _backpackTrigger.canSwitch = false;
            _nowHookNet = net;
            StateController.ChangeAllState(this);
            ThrowHook(time);
        }

        public void StartRecollect()
        {
            StartCoroutine(Recollect());
        }

        private IEnumerator Recollect()
        {
            _nowHookNet.rb.velocity =
                Methods.YtoZero(transform.position - _nowHookNet.transform.position).normalized * recollectSpeed;
            var timer = 0f;
            while (true)
            {
                //Detect if its arrived
                if (Methods.YtoZero(transform.position - _nowHookNet.transform.position).magnitude < 1.25f || timer > 5f)
                {
                    bool pop = false;
                    //Get Loot
                    var standBlock = GetComponent<GridDetector>().GetStandBlock();
                    if (standBlock)
                    {
                        pop = _nowHookNet.net.PopAllFloatablesTo(standBlock, GetComponent<GrabTrigger>());
                    }
                    
                    _nowHookNet.moveParticle.gameObject.SetActive(false);

                    //Reset transform
                    _nowHookNet.transform.SetParent(_lastParent);
                    _nowHookNet.ReturnKinematic();
                    GetComponent<GrabTrigger>().AdjustHoldPosition();
                    //Animator
                    _nowHookNet.animator.SetTrigger(Close);
                    //Input
                    StateController.ChangeToDefault();
                    _backpackTrigger.canSwitch = true;
                    //Add xp
                    if(pop)
                        skillContainer.AddXPTo("HookNet", 5);
                    
                    break;
                }

                timer += UnityEngine.Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
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
            
        }
    }
}