/*using System.Collections.Generic;
using System.Linq;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Interactable.Concrete
{
    public class ConcreteDesk : MonoBehaviour, IConsumeGroundable
    {
        [SerializeField] private GameObject plane;
        [SerializeField] private Transform upperPoint;

        [SerializeField] [ReadOnly] private List<string> filleeList;
        [SerializeField] [ReadOnly] private string currentFillee;
        [SerializeField] [ReadOnly] private int currentFillAmount;
        [SerializeField] private int maxFillAmount;
        [SerializeField] private ConcreteBlock concreteBlock;

        private void Start()
        {
            filleeList = concreteBlock.IdToModelDict.Keys.ToList();
        }

        public bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
        {
            if (currentFillAmount == 0) return filleeList.Contains(groundable.id);
            return currentFillee.Equals(groundable.id);
        }

        public void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            if (currentFillAmount == 0) currentFillee = groundable.id;

            currentFillAmount++;

            if (currentFillAmount == maxFillAmount)
            {
                currentFillAmount = 0;
                var block = GetComponent<Groundable>().blockUnder.island
                    .GetRandomSurroundStackableBlock(GetComponent<Groundable>().blockUnder.coordinate);
                var conc = Instantiate(concreteBlock);
                conc.Init(groundable.id);
                conc.GetComponent<Groundable>().SetOn(block);
            }

            ChangePlaneTo((float)currentFillAmount / maxFillAmount);
        }

        public void EffectAfterSetOn(Groundable groundable)
        {
        }

        private void ChangePlaneTo(float poss)
        {
            plane.transform.DOLocalMoveY(upperPoint.localPosition.y * poss, .3f);
        }
    }
}*/