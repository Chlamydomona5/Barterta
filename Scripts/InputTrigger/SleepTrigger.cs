using System.Collections;
using System.Linq;
using Barterta.Core;
using Barterta.Island;
using Barterta.Sound;
using Barterta.StaminaAndHealth;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Barterta.InputTrigger
{
    public class SleepTrigger : InputTriggerBase
    {
        [ReadOnly] public bool isSleeping;

        [SerializeField] private Image blackCover;
        [FormerlySerializedAs("staminaResumedPer5Sec")] [SerializeField] private float staminaResumedPerSec = 5f;
        
        private Vector3 _beforePos;
        private Tween _blackCoverTween;
        [SerializeField] private AttributeContainer staminaContainer;
        private Coroutine _staminaResume;
        private static readonly int Sleep = Animator.StringToHash("Sleep");

        private void Start()
        {
            //Set all false
            blackCover.gameObject.SetActive(false);
        }

        public void StartSleep(Vector3 standPos, Vector3 bedPos)
        {
            isSleeping = true;
            //Pos
            _beforePos = standPos;
            transform.position = bedPos + Vector3.up * Constant.ChunkAndIsland.BlockSize / 2f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            //Anim
            Animator.SetBool( Sleep, true);
            //Input State
            //Emergent exit to prevent some state stay happen
            StateController.EmergentExitCurrent();
            StateController.ChangeAllState(this);
            //Stamina
            _staminaResume = StartCoroutine(StaminaResume());

            //cover
            blackCover.gameObject.SetActive(true);
            blackCover.color = new Color(0, 0, 0, 0f);
            _blackCoverTween = blackCover.DOFade(.8f,  8f);
            //Sound
            SoundManager.I.PlaySound("Bed");
        }

        public void CancelSleep()
        {
            isSleeping = false;
            //Coroutine
            StopAllCoroutines();
            _blackCoverTween.Kill();
            //Pos
            transform.position = _beforePos;
            //Anim
            Animator.SetBool(Sleep, false);
            //Input State
            StateController.ChangeToDefault();
            //UI cover
            if (_blackCoverTween != null && _blackCoverTween.active) _blackCoverTween.Complete();
            _blackCoverTween = blackCover.DOFade(0f, 1f)
                .OnComplete(delegate { blackCover.gameObject.SetActive(false); });
            SoundManager.I.PlaySound("Bed");
        }

        private IEnumerator StaminaResume()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                staminaContainer.Resume(staminaResumedPerSec);
            }
        }

        public void Faint()
        {
            StateController.ChangeAllState(this);
            Animator.SetBool("Faint", true);
            //If is sleeping, cancel sleep and kill coroutine
            if (_blackCoverTween != null && _blackCoverTween.active) _blackCoverTween.Kill();
            
            blackCover.gameObject.SetActive(true);
            blackCover.color = new Color(0, 0, 0, 0f);
            _blackCoverTween = blackCover.DOFade(1f, 5f).OnComplete(OnFaintOver);
        }
        
        private void OnFaintOver()
        {
            //TP to 0,0,0
            transform.position = Vector3.zero + Vector3.up * Constant.ChunkAndIsland.BlockSize / 2f;
            
            StateController.ChangeToDefault();
            Animator.SetBool("Faint", false);
            _blackCoverTween = blackCover.DOFade(0f, 4f);
            //Effect
            //Find all Attribute and Reset to half
            var containers = GetComponents<AttributeContainer>();
            foreach (var container in containers)
            {
                container.NowValue = container.MaxValue / 2f;
            }
            //BlackboardResource Change
            foreach(var blackboard in BlackboardResource.IDList)
            {
                HomeManager.I.ChangeBlackboardResource(blackboard, -Constant.FaintResourceLoss);
            }
        }

        private void OnInteract()
        {
            if(isSleeping)
                CancelSleep();
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
            OnInteract();
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            OnInteract();
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
        }

        public override void EmergentExit()
        {

        }
    }
}