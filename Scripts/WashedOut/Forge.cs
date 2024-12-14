/*using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Interactable
{
    public class Forge : MonoBehaviour, IPutGroundableOn, IConsumeGroundable
    {
        /// <summary>
        ///     nowTool is not actually settled on forge, since material can't be judged if it's set on forge
        /// </summary>
        private Tool.Tool NowTool
        {
            get => nowTool;
            set
            {
                nowTool = value;
                WorldUIManager.ChangeFractionUI(_playerStayUI.uiInstance, nowMaterialCount, MaxMaterialCount);
            }
        }

        [SerializeField, ReadOnly] private Tool.Tool nowTool;

        [SerializeField]
        private int needRopeCountConstant = 30;

        private int MaxMaterialCount
        {
            get
            {
                if (NowTool)
                    return Mathf.Clamp((NowTool.maxDurability - NowTool.durability) / needRopeCountConstant, 1, 100);
                return 0;
            }
        }

        [SerializeField] [ReadOnly] private int nowMaterialCount;


        private PlayerStayUI _playerStayUI;

        private int NowMaterialCount
        {
            get => nowMaterialCount;
            set
            {
                nowMaterialCount = value;
                if (_playerStayUI.uiInstance)
                    WorldUIManager.ChangeFractionUI(_playerStayUI.uiInstance, nowMaterialCount, MaxMaterialCount);
            }
        }

        private void Start()
        {
            _playerStayUI = GetComponentInChildren<PlayerStayUI>();
            //Reset UI
            NowMaterialCount = 0;
        }

        public bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
        {
            if (!NowTool && !groundable.GetComponent<Tool.Tool>())
            {
                if (trigger) trigger.GetComponent<DialogTrigger>().SelfBark("forgenotool");
                return false;
            }

            if (groundable.id != "rope")
            {
                if (trigger) trigger.GetComponent<DialogTrigger>().SelfBark("forgenotrope");
                return false;
            }

            return true;
        }

        public void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            NowMaterialCount++;
            if (NowMaterialCount >= MaxMaterialCount)
            {
                NowMaterialCount = 0;
                NowTool.Repair();
            }
        }

        public bool JudgePut(Groundable groundable)
        {
            return groundable.GetComponent<Tool.Tool>() && !groundable.GetComponent<Tool.Tool>().infiniteDurability;
        }

        public void EffectBeforeSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
            //Set last tool to ground
            if (NowTool && NowTool != groundable.GetComponent<Tool.Tool>())
            {
                var groundable1 = NowTool.GetComponent<Groundable>();
                var block = GetComponent<Groundable>().blockUnder;
                groundable1.SetOn(block.island.GetRandomSurroundStackableBlock(block.coordinate));
            }
        }

        public void EffectAfterSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
            NowTool = groundable.GetComponent<Tool.Tool>();
        }
    }
}*/