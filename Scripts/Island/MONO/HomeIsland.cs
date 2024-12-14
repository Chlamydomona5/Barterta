using System.Collections;
using Barterta.Core;
using Barterta.Island.SO;
using Barterta.Island.SO.IslandType;
using Barterta.ItemGrid;
using Barterta.Mark;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Barterta.Island.MONO
{
    public class HomeIsland : Island
    {
        [Title("Shrine")] [SerializeField] [ReadOnly]
        public Shrine.Shrine shrine;
        public bool IsShrined => shrine;
        private MarkContainer _playerContainer;

        private void Awake()
        {
            islandForm = Resources.Load<IslandForm>(Constant.OnTestComponent ? "Island/IslandType/Home_Test" : "Island/IslandType/Home");
            _playerContainer = Resources.Load<MarkContainer>("PlayerContainer");
        }

        public override void Init(IslandForm form, ElfAttribute attr, Vector3 center, Chunk ck)
        {
            transform.SetParent(ck.transform);
            base.Init(form, attr, center, ck);
            InsShrine();
            if (Constant.OnTestComponent) TestGroundables();
            //Refresh after StartGroundables
            Refresh(120);
        
            //TODO:Merchant
            /*var merchantControl = new GameObject("MerchantController");
            merchantControl.transform.SetParent(transform);
            merchantControl.AddComponent<MerchantController>().Init(this);*/
        
            WorldManager.I.RegisterIsland(this);
        }

        public void AddShrine(Shrine.Shrine shrineToAdd)
        {
            shrine = shrineToAdd;
            //Resources.Load<DayStatus>("DayNight/DayStatus").newDayEvent.AddListener(DailyRefresh);
        }
    
        public void InsShrine()
        {
            if (!IsShrined)
            {
                var temp = Instantiate(Resources.Load<Groundable>("Shrine/Shrine"));
                temp.SetOn(BlockMap[0, 0]);
                temp.GetComponent<Shrine.Shrine>().Init(this);
                shrine = temp.GetComponent<Shrine.Shrine>();
            }
        }

        public void TestGroundables()
        {
            var edgeBlocks = GetAllEdgeBlock();
            foreach (var str in Constant.ChunkAndIsland.TestItemPath)
            {
                var groundables = Resources.LoadAll<Groundable>(str);
                foreach (var groundable in groundables)
                {
                    Instantiate(groundable)
                        .SetOn(
                            GetRandomSurroundStackableBlock(edgeBlocks[Random.Range(0, edgeBlocks.Count)].coordinate));
                }
            }
        }

        public override void Erode()
        {
            base.Erode();
            foreach (var mark in _playerContainer.markList)
            {
                mark.GetComponent<TipController>().Tip(Methods.GetLocalText("ErodeTip"));
            }
        }

        public IEnumerator ErosionCoroutine()
        {
            yield return new WaitForSeconds(5f);
            while (true)
            {
                Erode();
                yield return new WaitForSeconds(Constant.ChunkAndIsland.ErosionInterval);
            }
        }
    }
}