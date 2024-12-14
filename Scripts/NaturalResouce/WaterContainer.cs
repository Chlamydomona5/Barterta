using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Player;
using Barterta.Sound;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.NaturalResouce
{
    public class WaterContainer : Groundable, IPressInteractOnHandEffector
    {
        public override string LocalizeName => Methods.GetLocalText(ID + "_" + WaterType.ToString());

        public override string ID
        {
            get => id + "_" + WaterType.ToString();
            set => id = value;
        }

        private WaterType _waterType = WaterType.Empty;
        [SerializeField] private Dictionary<WaterType, GameObject> _waterTypeToModel;

        [SerializeField] private int waterCount = 1;
        public int WaterCount => waterCount;

        public WaterType WaterType
        {
            get => _waterType;
            set
            {
                _waterTypeToModel[_waterType].SetActive(false);
                _waterType = value;
                _waterTypeToModel[_waterType].SetActive(true);
            }
        }
        
        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            var target = trigger.GetComponent<GridDetector>().targetBlock;
            if (!target) return true;

            if (target.groundablesOn.Count == 0) return false;
            var distillater = target.groundablesOn[0] as WaterDistillater;
            if (!distillater) return false;
        
            if (WaterType == WaterType.SeaWater)
            {
                if (distillater.SeaWaterCount < distillater.seaWaterCapacity)
                {
                    return true;
                }
            }
            //If container is empty, try to take out pure water
            else if (WaterType == WaterType.Empty)
            {
                if (distillater.PureWaterCount >= WaterCount)
                {
                    return true;
                }
            }

            return false;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            var targetBlock = trigger.GetComponent<GridDetector>().targetBlock;
            var targetEntity = trigger.GetComponent<GridDetector>().targetEntity;
            if (targetBlock && targetBlock.groundablesOn.Count > 0 && targetBlock.groundablesOn[0] is WaterDistillater)
            {
                ((WaterDistillater) targetBlock.groundablesOn[0]).TryContainer(this);
            }
            
            if(targetBlock || targetEntity) return;
            if (WaterType == WaterType.Empty)
            {
                WaterType = WaterType.SeaWater;
                //Sound
                SoundManager.I.PlaySound("Water");
            }
            else
            {
                WaterType = WaterType.Empty;
                //Sound
                SoundManager.I.PlaySound("Water");
            }
        }
    }

    public enum WaterType
    {
        Empty,
        PureWater,
        SeaWater,
    }
}