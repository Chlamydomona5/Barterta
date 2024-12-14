using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.ItemGrid
{
    public class Groundable : IDBase
    {
        [Title("Value")] public int value = 1;
        [Title("UpperPivot")] public Transform upperPivot;

        public bool stackable;
        public bool cantBeGrabbed;
        public bool willCollideWithPlayer = true;
        public bool hasDiffModel;

        [SerializeField] [ShowIf("hasDiffModel")]
        private GameObject groundModel;

        [SerializeField] [ShowIf("hasDiffModel")]
        private GameObject handModel;

        [ReadOnly] public GroundBlock blockUnder;
        [HideInInspector] public BoxCollider humanCollider;
        public string Introduction => Methods.GetLocalText(ID + "_introduction");

        private Vector3 _originScale;

        private void SetCollsion(bool enable)
        {
            if (willCollideWithPlayer)
                humanCollider.enabled = enable;
        }

        public void SwitchModelTo(bool isToHand)
        {
            handModel.SetActive(isToHand);
            groundModel.SetActive(!isToHand);
        }

        public virtual bool CanPutInWater(GrabTrigger trigger)
        {
            return false;
        }
        
        public virtual void OnPutInWater(GrabTrigger trigger)
        {
        }

        #region Transport

        public void SetOn(GroundBlock block, GrabTrigger trigger = null)
        {
            var willEffect = false;
            //if block don't exist
            if (!block)
            {
                Destroy(gameObject);
                return;
            }

            //Detect Effector, effector can exist one and only at [0]
            if (block.groundablesOn.Count > 0 && block.groundablesOn[0].GetComponent<IPutGroundableOn>() != null)
                if (block.groundablesOn[0].GetComponent<IPutGroundableOn>().JudgePut(this))
                {
                    block.groundablesOn[0].GetComponent<IPutGroundableOn>().EffectBeforeSetOn(this);
                    willEffect = true;
                }

            if (block.groundablesOn.Count > 0 && block.groundablesOn[0].GetComponent<IConsumeGroundable>() != null)
            {
                var judge = block.groundablesOn[0].GetComponent<IConsumeGroundable>().JudgeConsume(this, trigger);
                block.groundablesOn[0].GetComponent<IConsumeGroundable>().OnJudgeConsume(judge, this, trigger);
                if (judge)
                {
                    block.groundablesOn[0].GetComponent<IConsumeGroundable>().ConsumeEffect(this, trigger);
                    Destroy(gameObject);
                }
            }

            block.TryRemoveNull();
            //Set position & rotation
            transform.SetParent(block.transform);

            if (block.groundablesOn.Count == 0)
            {
                transform.localPosition = Vector3.up * Constant.ChunkAndIsland.BlockSize / 2;
            }
            else
            {
                if (block.groundablesOn[block.groundablesOn.Count - 1].upperPivot)
                    transform.localPosition =
                        block.groundablesOn[block.groundablesOn.Count - 1].transform.localPosition +
                        block.groundablesOn[block.groundablesOn.Count - 1].upperPivot.localPosition;
                else
                    //iF no upperpivot
                    transform.localPosition =
                        block.groundablesOn[block.groundablesOn.Count - 1].transform.localPosition +
                        block.groundablesOn[block.groundablesOn.Count - 1].transform.localPosition;
            }

            var tran = transform;
            tran.localRotation = Quaternion.identity;
            tran.localScale = _originScale;

            //Add ref to each other
            block.groundablesOn.Add(this);
            blockUnder = block;

            //Enable Collider
            SetCollsion(true);

            //Switch Model
            if (hasDiffModel) SwitchModelTo(block.CompareTag("Player"));

            GetComponent<IBeSettled>()?.OnSettled(block);

            if (willEffect) block.groundablesOn[0].GetComponent<IPutGroundableOn>().EffectAfterSetOn(this, trigger);
        }


        public void BeRemovedFromNowBlock()
        {
            if (blockUnder)
            {
                //Detect error
                if (blockUnder.groundablesOn.Exists(x => x == this))
                    Debug.Assert(true, "Groundables is not on the block but attempt to be carried");
                //Remove self from list
                blockUnder.groundablesOn.Remove(this);

                //Update Animation
                blockUnder.GetComponentInParent<GrabTrigger>()?.CheckAnimChange();

                SetCollsion(false);
            }
        }

        public bool CanStackOn(Groundable groundable)
        {
            //only if block's groundableson not full
            if (groundable.blockUnder && groundable.blockUnder.groundablesOn.Count < Constant.MaxStackCount)
            {
                return stackable && groundable.ID == ID;
            }
            return false;
        }

        #endregion

        #region Unity_Logic

        protected virtual void Awake()
        {
            foreach (var col in GetComponentsInChildren<Collider>())
                //Can set trigger collider as detector
                if (!col.isTrigger)
                    col.enabled = false;

            humanCollider = gameObject.AddComponent<BoxCollider>();
            humanCollider.size = new Vector3(1, 1, 1) * .45f;
            var material = humanCollider.material;
            //Set ALL Friction to 0, let player pass easier
            material.dynamicFriction = 0;
            material.staticFriction = 0;
            if (!willCollideWithPlayer)
                humanCollider.enabled = false;

            _originScale = transform.localScale;
        }

        private void OnDestroy()
        {
            BeRemovedFromNowBlock();
        }

        #endregion
    }
}