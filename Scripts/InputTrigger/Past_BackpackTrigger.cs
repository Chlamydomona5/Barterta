using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Barterta.Core.KeyInterface;
using Barterta.Craft;
using Barterta.Inventory;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Barterta.UI.ScreenUI;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class Past_BackpackTrigger : InputTriggerBase
    {
        [Title("Constant")]
        [SerializeField] private float interval = 1f;
        [SerializeField] private float uiCloseTime = 2f;
    
        [SerializeField] private FollowObject backpackRoot;
        [SerializeField] private Transform holdTran;

        [SerializeField, ReadOnly] private List<Past_BackpackBlock> slotBlocks;
        [SerializeField, ReadOnly] private Past_BackpackBlock currentBlock;
        private float _uiCloseTimer;
        private bool _startToCount;
        private Coroutine _backpackUICoroutine;
        private GrabTrigger _grabTrigger;
        
        private Mark.Mark _mark;

        private Sequence _staminaSeq;
        private int CurrIndex => slotBlocks.FindIndex(x => x == CurrentBlock);
        public Past_BackpackBlock CurrentBlock
        {
            get => currentBlock;
            set
            {
                if (currentBlock != value)
                {
                    CheckIMoveToHand(value);
                    Methods.SetAllChildrenLayer(currentBlock.transform, "Player" + _mark.id); 
                    currentBlock = value;
                    Methods.SetAllChildrenLayer(currentBlock.transform, "Default");
                }
                //Expand effect
                MoveCurr(true);
            }
        }
        private void Start()
        {
            _mark = GetComponent<Mark.Mark>();
            _grabTrigger = GetComponent<GrabTrigger>();
            //_uiManager = WorldUIManager.I;
            //Ins UI
            //_uiInstance = (BackpackUI)_uiManager.GenerateUI(_uiManager.backpackPrefab, Vector3.zero);
            //_uiInstance.GetComponent<FollowObject>().followTran = transform;
            //init
            slotBlocks = backpackRoot.GetComponentsInChildren<Past_BackpackBlock>().ToList();
            currentBlock = slotBlocks[0];
            //TODO: Example Version 7.11
            var list = new List<string>()
            {
                "Utensil/StonePickaxe",
                "Utensil/CraftHammer",
                "Utensil/FishingRod",
                "Utensil/HookNet",
                "Weapon/TestSword"
            };
            for (int i = 0; i < list.Count; i++)
            {
                var groundable = Resources.Load<Groundable>(list[i]);
                var instance = Instantiate(groundable);
                instance.SetOn(slotBlocks[i]);

            }
            //close first
            Invoke("CloseBackpack",.1f);
        }

        private void CheckIMoveToHand(GroundBlock newBlock)
        {
            foreach (var g in currentBlock.groundablesOn)
                if (g.GetComponent<IMoveToHand>() != null)
                    g.GetComponent<IMoveToHand>().OnMove(false, this.GetComponent<GrabTrigger>(), newBlock);
            
            foreach (var g in newBlock.groundablesOn)
                if (g.GetComponent<IMoveToHand>() != null)
                    g.GetComponent<IMoveToHand>().OnMove(true, this.GetComponent<GrabTrigger>(), newBlock);
        }
        
        private bool OpenBackpack()
        {
            if (_backpackUICoroutine == null)
            {
                _backpackUICoroutine = StartCoroutine(BackpackUI());
                return true;
            }

            return false;
        }

        /*public void AppealStamina(float from, float to)
        {
            staminaUI.ChangeTo(from);
            _staminaSeq = _uiManager.AppealUI(_uiInstance.staminaUI);
            _staminaSeq.Append(staminaUI.ChangeToContinusly(to));
            _staminaSeq.AppendInterval(.5f);
            _staminaSeq.Append(_uiManager.HideUI(_uiInstance.staminaUI));
        }*/

        private IEnumerator BackpackUI()
        {
            //if stamina only, kill taht seq
            if (_staminaSeq != null && _staminaSeq.active) _staminaSeq.Kill();
            //UI
            //_uiManager.AppealUI(_uiInstance.introductionUI);
            //After UI appeal, open craft table
            /*craftTableUI.gameObject.SetActive(true);*/
            //slots enable
            backpackRoot.gameObject.SetActive(true);
            PlaceCurrentBlock(false);
            while (_uiCloseTimer > 0)
            {
                if (_startToCount)
                    _uiCloseTimer -= UnityEngine.Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            CloseBackpack();
            //yield return new WaitUntil((() => !backpackRoot.gameObject.activeSelf));
            _backpackUICoroutine = null;
        }

        private Sequence MoveCurr(bool open)
        {
            var intro = "";
            if (currentBlock.groundablesOn.Count > 0)
            {
                var groundable = currentBlock.groundablesOn[0];
                //Introduction
                intro = groundable.LocalizeName + ": " + groundable.Introduction;
            }
            //_uiInstance.introText.text = intro;
        
            var sq = DOTween.Sequence();
            for (int i = 0; i < slotBlocks.Count; i++)
            {
                //Layer change
                WorldUIManager.ChangeUILayer(slotBlocks[i].gameObject, _mark, false);
                //make sure current item on the middle
                if (open)
                {
                    var endValue = new Vector3(-CurrIndex * interval + i * interval, 0, 0);
                    if (i == CurrIndex)
                    {
                        endValue.z += .5f;
                        sq.Join(slotBlocks[i].transform.DOScale(new Vector3(1f, 1f, 1f), .2f));
                    }
                    else
                    {
                        sq.Join(slotBlocks[i].transform.DOScale(new Vector3(.8f, .8f, .8f), .2f));
                    }
                    sq.Join(slotBlocks[i].transform.DOLocalMove(endValue, .2f));
                }
                else
                {
                    slotBlocks[i].transform.localPosition = Vector3.zero;
                }
            }

            return sq;
        }

        private void CloseBackpack()
        {
            var seq = MoveCurr(false);
            //check change when switch
            _grabTrigger.CheckAnimChange();
            backpackRoot.gameObject.SetActive(false);
            //craftTableUI.gameObject.SetActive(false);

            PlaceCurrentBlock(true);
            //seq.Join(_uiManager.HideUI(_uiInstance.introductionUI));
        
        }

        private void PlaceCurrentBlock(bool isToHand)
        {
            //transform change
            currentBlock.transform.SetParent(isToHand ? holdTran : backpackRoot.transform, false);
            //cude active
            currentBlock.containerCube.SetActive(!isToHand);
            //model
            if (currentBlock.groundablesOn.Count > 0)
            {
                var groundable = currentBlock.groundablesOn[0];
                if (groundable.hasDiffModel)
                    groundable.SwitchModelTo(isToHand);
                if (groundable.GetComponent<Tool.Tool>())
                    _grabTrigger.AdjustHoldPosition(isToHand);
            }
        }

        public void OnBackpack(InputAction.CallbackContext ctx)
        {
            //If press q/e to join, slots are not loaded yet
            if (slotBlocks.Count > 0)
            {
                //make sure every key reset the close time
                _startToCount = false;
                _uiCloseTimer = uiCloseTime;
                int indexChange = 0;
                //Open Effect
                if (!OpenBackpack())
                {
                    //If already open.
                    if (ctx.ReadValue<float>() > 0) indexChange = -1;
                    if (ctx.ReadValue<float>() < 0) indexChange = 1;
                }
                //set current block new index
                var newIndex = Mathf.Clamp(CurrIndex + indexChange, 0, slotBlocks.Count - 1);
                CurrentBlock = slotBlocks[newIndex];
            }
        }

        public void OnBackpackCancel(InputAction.CallbackContext ctx)
        {
            //Count start from when key canceled
            _startToCount = true;
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

        private void OnDestroy()
        {
            /*if(_uiInstance)
                Destroy(_uiInstance.gameObject);*/
        }
    }
}