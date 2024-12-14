using System;
using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.Island.MONO;
using Barterta.ItemGrid;
using Barterta.Sound;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Shrine
{
    public class Shrine : Groundable
    {
        public int level = 0;
        public Island.MONO.Island island;
        protected Dictionary<string, int> CurrentRequirement => requirements[level];
        [SerializeField] protected ParticleSystem chargeParticle;
        //[SerializeField] private List<GameObject> levelToModel;
        private ParticleSystem _particleSystem;
        [SerializeField] private List<Dictionary<string, int>> requirements;
        [SerializeField] private DeadTimer.DeadTimer _deadTimer;

        public void Init(Island.MONO.Island initIsland)
        {
            foreach (var block in initIsland.GetSurroundBlocksDir8(GetComponent<Groundable>().blockUnder.coordinate))
            {
                block.gameObject.tag = "ShrinePlatform";
            }

            _particleSystem = GetComponentInChildren<ParticleSystem>();

            ((HomeIsland)GetComponent<Groundable>().blockUnder.island).AddShrine(this);

            island = initIsland;
        }


        protected Dictionary<string, int> ReadPlatform()
        {
            var dict = new Dictionary<string, int>();
            var coord = blockUnder.coordinate;
            //Each Platform block
            foreach (var vec in Constant.Direction.Dir8)
            {
                //Each groundable
                foreach (var groundable in island.BlockMap[coord.x + vec.x, coord.y + vec.y].groundablesOn)
                {
                    dict.DictAdd(groundable.ID);
                }
            }

            return dict;
        }

        protected void ClearPlatform()
        {
            var coord = blockUnder.coordinate;
            //Each Platform block
            foreach (var vec in Constant.Direction.Dir8)
            {
                island.BlockMap[coord.x + vec.x, coord.y + vec.y].DestoryAll();
            }
        }

        protected List<string> GetRequestText()
        {
            var strList = new List<string>();
            strList.Add(RequestToString());
            foreach (var str in CurrentRequirement.Keys)
            {
                strList.Add(Methods.GetLocalText(str) + ": " + Methods.GetLocalText(str + "_introduction"));
            }

            return strList;
        }

        protected bool ValidatePlatform()
        {
            return Methods.ItemCountDictValueContains(ReadPlatform(), CurrentRequirement);
        }

        #region Dialog

        private List<string> GetUpgradeText()
        {
            var strList = new List<string>();
            //name & intro
            strList.Add(Methods.GetLocalText("shrine_req_0"));
            strList.AddRange(GetRequestText());
            strList.Add(Methods.GetLocalText("shrine_req_1"));
            return strList;
        }

        protected string RequestToString()
        {
            string ret = String.Empty;
            foreach (var pair in CurrentRequirement)
            {
                ret += (Methods.GetLocalText(pair.Key) + "x" + pair.Value + ",");
            }

            return ret;
        }

        #endregion

        #region Interface

        /*public bool OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (ValidatePlatform())
            {
                //Temp level
                level++;
                //levelToModel[level].SetActive(true);
                //Find Timer
                if (!_deadTimer)
                {
                    _deadTimer = FindObjectOfType<DeadTimer.DeadTimer>();
                }

                _deadTimer.AddTimeLimit(level * 100);
                //Particle
                _particleSystem.Play();
                ClearPlatform();
            }
            else
            {
                /*trigger.GetComponent<DialogTrigger>()
                    .StartConversation(Methods.StringToDialogItem(GetUpgradeText()), transform);#1#
                SoundManager.I.PlaySound("Shrine speak");
            }

            return true;
        }

        public bool JudgeConsume(Groundable groundable, GrabTrigger trigger = null)
        {
            return true;
        }

        public void ConsumeEffect(Groundable groundable, GrabTrigger trigger)
        {
            SoundManager.I.PlaySound("ItemToShrine");
            if (!_deadTimer)
            {
                _deadTimer = FindObjectOfType<DeadTimer.DeadTimer>();
            }

            _deadTimer.AddTime(groundable.value * 15);
            if (trigger)
                trigger.skillContainer.AddXPTo("Bartering", groundable.value);
        }*/


        #endregion

        #region Unity_Logic

        protected virtual void Start()
        {
            chargeParticle.Stop();
        }

        #endregion
    }
}