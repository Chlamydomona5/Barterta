using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.ItemGrid;
using Barterta.Sound;
using Barterta.Tool;
using Barterta.ToolScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class GrabTrigger : InputTriggerBase
    {
        private static readonly int IsHolding = Animator.StringToHash("isHolding");
        public GroundBlock HandBlock => _backpack.CurrentBlock;
        private BackpackTrigger _backpack;
        private KeyHintController _keyHintController;
        private static readonly int Eat = Animator.StringToHash("Eat");
        

        #region Unity_Logic

        protected override void Awake()
        {
            _backpack = GetComponent<BackpackTrigger>();
            _keyHintController = GetComponent<KeyHintController>();
            base.Awake();
        }

        public override void OnInteractPress(InputAction.CallbackContext ctx)
        {
            if (!HandBlock) return;
            var stop = false;
            //Read and process Interface
            if (detector.targetEntity && detector.targetEntity.GetComponentInParent<IPressInteractOnGroundEffector>() != null)
            {
                stop = detector.targetEntity.GetComponentInParent<IPressInteractOnGroundEffector>().Judge(false, this);
                detector.targetEntity.GetComponentInParent<IPressInteractOnGroundEffector>().OnInteract(false, this);
            }


            if (!stop && detector.targetBlock)
                stop = TryInterface<IPressInteractOnGroundEffector>(detector.targetBlock.groundablesOn, false);

            if (!stop)
                stop = TryInterface<IPressInteractOnHandEffector>(HandBlock.groundablesOn, false);
            RefreshUI();
        }

        private void RefreshUI()
        {
            _backpack.UpdateUI();
            _keyHintController.ShowHintTo(detector.targetBlock);
        }

        #endregion

        public override void OnShortGrab(InputAction.CallbackContext ctx)
        {
            if (!HandBlock) return;
            bool stop = false;
            if (!detector.targetBlock && !detector.targetEntity)
                stop = TryPutInWater(HandBlock.groundablesOn);
            if(!stop)
                ItemTransfer(true);
            CheckAnimChange();
            RefreshUI();
        }

        public override void OnLongGrab(InputAction.CallbackContext ctx)
        {
            if (!HandBlock) return;
            ItemTransfer(false);
            CheckAnimChange();
            RefreshUI();

        }

        public override void OnInteractCancel(InputAction.CallbackContext ctx)
        {
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
            if(!HandBlock) return;
            var stop = false;
            //Seal
            if (detector.targetBlock)
                //SpecialProcess
                //If targetblock is boat block
                if (detector.targetBlock.GetComponentInParent<Seal.Seal>())
                {
                    GetComponent<MapTrigger>().StartMap(detector.targetBlock.GetComponentInParent<Seal.Seal>().StartTransport, true);
                    stop = true;
                }
            
            //Read and process Interface
            if (detector.targetEntity && detector.targetEntity.GetComponentInParent<IShortInteractOnGroundEffector>() != null)
            {
                stop = detector.targetEntity.GetComponentInParent<IShortInteractOnGroundEffector>().Judge(false, this);
                detector.targetEntity.GetComponentInParent<IShortInteractOnGroundEffector>().OnInteract(false, this);
            }

            if (!stop && detector.targetBlock)
                stop = TryInterface<IShortInteractOnGroundEffector>(detector.targetBlock.groundablesOn, false);

            if (!stop)
                TryInterface<IShortInteractOnHandEffector>(HandBlock.groundablesOn, false);
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            var stop = false;
            //Read and process Interface
            if (detector.targetEntity && detector.targetEntity.GetComponentInParent<ILongInteractOnGroundEffector>() != null)
            {
                stop = detector.targetEntity.GetComponentInParent<ILongInteractOnGroundEffector>().Judge(true, this);
                detector.targetEntity.GetComponentInParent<ILongInteractOnGroundEffector>().OnInteract(true, this);
            }

            if (!stop && detector.targetBlock)
                stop = TryInterface<ILongInteractOnGroundEffector>(detector.targetBlock.groundablesOn, true);

            if (!stop)
                stop = TryInterface<ILongInteractOnHandEffector>(HandBlock.groundablesOn, true);
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
            
        }

        public override void EmergentExit()
        {
            
        }

        private bool TryPutInWater(List<Groundable> groundables)
        {
            if(groundables.Count == 0) return false;
            
            bool canGo = true;
            if(!groundables[groundables.Count - 1].CanPutInWater(this)) canGo = false;
            groundables[groundables.Count - 1].OnPutInWater(this);
            return canGo;
        }

        private bool TryInterface<T>(List<Groundable> groundables, bool isLongGrab) where T : IInteractBase
        {
            //Only affect one each time
            //From top to down
            var stop = false;
            foreach (var groundable in groundables.FindAll(x => x.GetComponent<T>() != null).FastReverse())
            {
                stop = groundable.GetComponent<T>().Judge(isLongGrab, this);
                groundable.GetComponent<T>().OnInteract(isLongGrab, this);
                if (stop) return true;
            }
            return false;
        }

        public Vector3 GetTowardPos(float dis)
        {
            var trans = transform;
            return trans.position + trans.forward * dis;
        }

        private void ItemTransfer(bool isSlight)
        {
            if (!detector.targetBlock) return;

            var sourceIsNull = HandBlock.groundablesOn.Count == 0;
            var targetGroundablesOn = detector.targetBlock.groundablesOn;
            var targetIsNull = targetGroundablesOn.Count == 0;
            //Debug.Log("Grab");
            //If Target is empty, try to put things on
            if (targetIsNull)
                Move(HandBlock, detector.targetBlock, isSlight);
            //else if source is empty, try to get things carried`
            else if (sourceIsNull)
                Move(detector.targetBlock, HandBlock, isSlight);
            //else, if both has sth on, need to judge
            else
                JudgeWhenBothNotEmpty();

            if (HandBlock.groundablesOn.Count > 0 && HandBlock.groundablesOn[0].GetComponent<NotHold>())
                AdjustHoldPosition();
            
            //disable groundable's collider in hand
            foreach (var groundable in HandBlock.groundablesOn) groundable.humanCollider.enabled = false;
            
            //Refresh UI
            

            void Move(GroundBlock source, GroundBlock target, bool only)
            {
                //Judge IMoveToHand TODO: Only Support can't stack
                var toHand = target.Equals(HandBlock);
                foreach (var groundable in source.groundablesOn)
                    if (groundable.GetComponent<IMoveToHand>() != null)
                        groundable.GetComponent<IMoveToHand>().OnMove(toHand, this, target);
                bool success = false;
                //isonly
                if (!only)
                    success = source.TransferAll(target, this);
                else
                    success = source.TransferTop(target,  this);
                //Sound
                if (success)
                    SoundManager.I.PlaySound("Grab", .05f);
            }

            void JudgeWhenBothNotEmpty()
            {
                var judgeIndex = 1;
                //Can't use move, there are special condition like seed on farmland, only one can be placed then
                while (judgeIndex <= HandBlock.groundablesOn.Count)
                {
                    var judgeObject = HandBlock.groundablesOn[HandBlock.groundablesOn.Count - judgeIndex];

                    var canStackWithTop = targetGroundablesOn.Count > 0 &&
                                          judgeObject.CanStackOn(targetGroundablesOn[targetGroundablesOn.Count - 1]);
                    var canPutOn = targetGroundablesOn.Count == 1 &&
                                   targetGroundablesOn[0].GetComponent<IPutGroundableOn>() != null &&
                                   targetGroundablesOn[0].GetComponent<IPutGroundableOn>().JudgePut(judgeObject);
                    var canConsume = targetGroundablesOn[0].GetComponent<IConsumeGroundable>() != null &&
                                     targetGroundablesOn[0].GetComponent<IConsumeGroundable>()
                                         .JudgeConsume(judgeObject, this);
                    var willTransfer = canStackWithTop || canPutOn || canConsume;

                    if (willTransfer)
                    {
                        //if can be stack, then next is [judgeindex]
                        Move(HandBlock, detector.targetBlock, true);
                        //if is slight, only move one
                        if(isSlight) return;
                    }
                    else
                    {
                        if (!targetGroundablesOn[0].cantBeGrabbed)
                        {
                            //if can't be stack, then switch hand block and target block
                            var temp = new GameObject("temp",typeof(GroundBlock)).GetComponent<GroundBlock>();
                            temp.transform.position = new Vector3(0, -20, 0);
                            Move(detector.targetBlock, temp, false);
                            Move(HandBlock, detector.targetBlock, false);
                            Move(temp, HandBlock, false);   
                        }
                        return;
                        //judgeIndex++;
                    }
                }
            }
        }

        public void AdjustHoldPosition(bool toHand = true)
        {
            if (toHand)
            {
                HandBlock.groundablesOn[0].transform.localPosition =
                    HandBlock.groundablesOn[0].GetComponent<NotHold>().holdPosition;
                HandBlock.groundablesOn[0].transform.localRotation =
                    Quaternion.Euler(HandBlock.groundablesOn[0].GetComponent<NotHold>().holdRotation);   
            }
            else
            {
                HandBlock.groundablesOn[0].transform.localPosition = Vector3.up * Constant.ChunkAndIsland.BlockSize / 2;
                HandBlock.groundablesOn[0].transform.localRotation = Quaternion.identity;
            }
        }

        public void CheckAnimChange()
        {
            Animator.SetBool(IsHolding,
                HandBlock.groundablesOn.Count > 0 && !HandBlock.groundablesOn[0].GetComponent<NotHold>());
        }

        public void HandReturnIdle()
        {
            Animator.SetBool(IsHolding, false);
        }

        public void EatAnimation()
        {
            Animator.SetTrigger(Eat);
            Animator.SetBool(IsHolding, false);
        }
    }
}