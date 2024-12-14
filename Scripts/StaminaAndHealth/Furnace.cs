using Barterta.InputTrigger;
using Barterta.Interactable;
using Barterta.ItemGrid;
using Barterta.UI.WorldUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.StaminaAndHealth
{
    public class Furnace : Processor
    {
        [SerializeField] private GameObject effect;
        [SerializeField, ReadOnly] private int fuelCount;

        protected override void Awake()
        {
            base.Awake();
            effect.SetActive(false);
        }

        protected override bool Judge(Groundable groundable, GrabTrigger trigger = null)
        {
            return (groundable.GetComponent<Burnable>() && fuelCount > 0) || groundable.GetComponent<Fuel>();
        }

        public override void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null)
        {
            if(groundable.GetComponent<Burnable>() && fuelCount == 0)
                if (trigger)
                    trigger.GetComponent<DialogTrigger>().SelfBark("fuelshort");
        }

        protected override Groundable GetResult(Groundable putIn)
        {
            var burnable = putIn.GetComponent<Burnable>();
            processTime = burnable.burnTime;
            resCountRange = new Vector2Int(burnable.res.count, burnable.res.count);
            return burnable.res.groundable;
        }

        protected override void ResultProcess(Groundable resultInstance)
        {

        }

        protected override void SetUI()
        {
            ((FurnaceUI)_playerStayUI.uiInstance).ChangeProcess(Timer / processTime);
        }

        public override void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            Fuel fuel;
            // ReSharper disable once AssignmentInConditionalExpression
            if (fuel = groundable.GetComponent<Fuel>())
            {
                fuelCount += fuel.fuelCount;
                ((FurnaceUI)_playerStayUI.uiInstance).ChangeFuel(true, fuel.fuelCount);
            }
            else if (groundable.GetComponent<Burnable>() && fuelCount > 0)
            {
                base.ConsumeEffect(groundable, trigger);
                fuelCount--;
                ((FurnaceUI)_playerStayUI.uiInstance).ChangeFuel(false, 1);
                
                effect.SetActive(true);
            }
        }

        protected override void OnProcessEnd()
        {
            base.OnProcessEnd();
            effect.SetActive(false);
        }
    }
}