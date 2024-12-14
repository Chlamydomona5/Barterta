using System.Collections.Generic;
using System.Linq;
using Barterta.ItemGrid;
using Barterta.Time.SO;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.NPC.Merchant
{
    public class MerchantController : MonoBehaviour
    {
        [ReadOnly]public Island.MONO.Island island;
        private Merchant _merchantPrefab;

        [Title("Observe")] [SerializeField] [ReadOnly]
        private List<Merchant> merchantList = new List<Merchant>();
        private List<MerchantIdentifier> _merchantIdentifierList;
        private int RandomDaysToLeave => Random.Range(Constant.MerchantAndShrine.LeaveTimeRange.x, Constant.MerchantAndShrine.LeaveTimeRange.y);

        public void Init(Island.MONO.Island inIsland)
        {
            this.island = inIsland;

            _merchantPrefab = Resources.Load<Merchant>("Merchant/MerchantPrefab");
            //Register to day status
            Resources.Load<DayStatus>("DayNight/DayStatus").newDayEvent.AddListener(DailyRefresh);
            _merchantIdentifierList = Resources.LoadAll<MerchantIdentifier>("Request/General").ToList();
            //Init preset merchants
            var startMerchantList = island.islandForm.GetIslandFeature(island).StartMerchantList;
            if(startMerchantList != null && startMerchantList.Count > 0)
                startMerchantList.ForEach(x => InsMerchant(x, RandomDaysToLeave));
        }

        private void InsMerchant(MerchantIdentifier identifier, int daysToLeave, Groundable merchandise = null)
        {
            var blockList = island.GetAllEdgeBlock();
            var block = blockList[Random.Range(0, blockList.Count)];

            var instance = Instantiate(_merchantPrefab, block.transform.position + Constant.ChunkAndIsland.BlockSize / 2 * Vector3.up,
                Quaternion.identity, block.island.transform);
            instance.Init(identifier, daysToLeave, this, merchandise);

            merchantList.Add(instance);
        }

        private MerchantIdentifier GetIdentifier(string merchandise = null)
        {
            MerchantIdentifier identifier = null;
            //Get identifier with certain merchandise
            if (merchandise != null)
                identifier = _merchantIdentifierList.Find(x => x.sellingItems.Find(y => y.ID == merchandise));
            //if no merchandise or no identifier with this merchandise, get random one
            if (!identifier)
                identifier = _merchantIdentifierList[Random.Range(0, _merchantIdentifierList.Count)];
            return identifier;
        }

        private void DailyRefresh()
        {
            if (merchantList.Count < island.attribute.merchantDensity)
            {
                InsMerchant(GetIdentifier(), RandomDaysToLeave);
            }
        }

        #region Interface

        public void MerchantLeave(Merchant merchant)
        {
            //Destory
            merchantList.Remove(merchant);
            DestroyImmediate(merchant.gameObject);
        }

        #endregion
    }
}