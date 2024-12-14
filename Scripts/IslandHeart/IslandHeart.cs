using System.Collections.Generic;
using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.Island.MONO;
using Barterta.ItemGrid;
using Barterta.Player;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.IslandHeart
{
    public class IslandHeart : Groundable, IShortInteractOnGroundEffector
    {
        [SerializeField] private Dictionary<IslandType, GameObject> typeToModelDict;
        [SerializeField, ReadOnly] private bool isActivated;
        [SerializeField] private ParticleSystem activateEffect;
        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (!isActivated && blockUnder.island.monsterController.IsEmpty)
            {
                Activiate();
            }
            else if(!isActivated && !blockUnder.island.monsterController.IsEmpty)
            {
                trigger.GetComponent<DialogTrigger>().SelfBark("stillMonster");
            }
            else
            {
                /*var block = trigger.GetComponent<GridDetector>().GetStandBlock();
                if (block && block.island)
                {
                    trigger.GetComponent<MapTrigger>().StartMap(trigger.GetComponent<FlyTrigger>().Fly, false);
                    return true;
                }
                else if (block && block.island)
                {
                    trigger.GetComponent<DialogTrigger>().SelfBark("flydisable");
                }   */
            }
        }

        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return !isActivated;
        }

        public void Activiate()
        {
            var trans = typeToModelDict[blockUnder.island.islandForm.type].transform;
            Sequence sq = DOTween.Sequence();
            sq.Append(trans.DOShakePosition(1f, .1f, 20));
            sq.Append(trans.DOLocalMoveY(-.5f, 2.5f).SetEase(Ease.OutCubic));
            sq.OnComplete(delegate { isActivated = true; });

            activateEffect.Play();
            WorldManager.I.RegisterIsland(blockUnder.island);
        }

        private void Start()
        {
            typeToModelDict[blockUnder.island.islandForm.type].SetActive(true);
        }
    }
}