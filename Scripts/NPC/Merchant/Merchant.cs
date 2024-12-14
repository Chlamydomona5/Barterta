using System.Collections;
using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.Dialog;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Time.SO;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.NPC.Merchant
{
    public class Merchant : NPCBase, IPressInteractOnGroundEffector
    {
        [Title("Identifier")] [ReadOnly] public MerchantIdentifier identifier;

        [Title("Attributes")] [SerializeField] [ReadOnly]
        private BarterRequest barterRequest;

        [SerializeField] [ReadOnly] public bool onlyMerchandise;

        [SerializeField] [ReadOnly] [HideIf("onlyMerchandise")]
        private int daysToLeave;

        [Title("Now Count of requesting items")] [SerializeField] [ReadOnly]
        private int nowCount;

        [HideInInspector] public MerchantController merchantController;

        private NPCBase _npcBase;
        private UIObject _numUIInstance;
        private WorldUIManager _uiManager;

        private void OnDestroy()
        {
            Resources.Load<DayStatus>("DayNight/DayStatus").newDayEvent.RemoveListener(DailyUpdate);
        }
        
        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return true;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (!isLong)
            {
                /*trigger.GetComponent<DialogTrigger>()
                    .StartConversation(GetDialog(), transform);*/
            }
            else
            {
                if (barterRequest != null && trigger.HandBlock.groundablesOn.Count > 0 &&
                    trigger.HandBlock.groundablesOn[0].ID.Equals(barterRequest.requestItem.ID))
                {
                    nowCount += trigger.HandBlock.groundablesOn.Count;
                    trigger.HandBlock.DestoryAll();
                    StartCoroutine(OpenNumUI());
                    if (nowCount >= barterRequest.num) MakeDeal();
                }
            }
        }

        public void Init(MerchantIdentifier gene, int dayTL, MerchantController cont, Groundable merchandise = null)
        {
            //Assign
            identifier = gene;
            daysToLeave = dayTL;
            merchantController = cont;
            _uiManager = WorldUIManager.I;
            _npcBase = GetComponent<NPCBase>();

            //Initialize request
            if (merchandise) barterRequest = identifier.GetRequestWithMerchandise(merchandise);
            else barterRequest = identifier.GetRequest();
            
            //Discount
            barterRequest.num = Mathf.Clamp((int)(barterRequest.num * cont.island.attribute.merchantDiscount), 1, 1000);

            //Register 
            Resources.Load<DayStatus>("DayNight/DayStatus").newDayEvent.AddListener(DailyUpdate);
        }

        private void DailyUpdate()
        {
            if (!onlyMerchandise)
            {
                daysToLeave--;
                if (daysToLeave == 0) merchantController.MerchantLeave(this);

                //Refresh Request
                if (identifier.sellingItems.Count != 1)
                {
                    barterRequest = identifier.GetRequest();
                    //Reset, so merchant won't give new craft table
                    //_haveSoldOnce = false;
                }
            }
            else
            {
                //if no req, destroy self on new day
                if (barterRequest == null)
                    //Destroy Merchant straight
                    Destroy(gameObject);
            }
        }

        public List<DialogItem> GetDialog()
        {
            var temp = new List<DialogItem>();
            //If there's a request
            Addtext(Methods.GetLocalText("merchant_intro") + $"<b>{barterRequest.rewardItem.LocalizeName}</b>");
            //CraftTable
            /*if (_haveSoldOnce)
            {
                Addtext(Methods.GetLocalText("merchant_craft_y"));
                AddRecipe(_validater.GetRecipeByResult(request.rewardItem.id));
            }
            else
            {
                Addtext(Methods.GetLocalText("merchant_craft_n"));
            }*/

            //only display introduction
            Addtext(barterRequest.rewardItem.Introduction);
            Addtext(Methods.GetLocalText("merchant_cost") + $"<b>{barterRequest.num}x{barterRequest.requestItem.LocalizeName}</b>");
            Addtext(barterRequest.requestItem.Introduction);
            Addtext(Methods.GetLocalText("merchant_howtobuy"));


            if (onlyMerchandise)
            {
                Addtext(Methods.GetLocalText("merchant_onlyone"));
            }
            else
            {
                if (identifier.sellingItems.Count == 1)
                    Addtext(Methods.GetLocalText("merchant_only"));
                else
                    Addtext(Methods.GetLocalText("merchant_many") +
                            Methods.GroundableListToString(identifier.sellingItems));
                Addtext(Methods.GetLocalText("merchant_leavetime_f") + $" <b>{daysToLeave}</b>" + Methods.GetLocalText("merchant_leavetime_b"));
            }

            return temp;

            void Addtext(string text)
            {
                temp.Add(new DialogItem(text));
            }
            
        }

        private void MakeDeal()
        {
            //_haveSoldOnce = true;
            for (var i = 0; i < nowCount / barterRequest.num;)
            {
                var rew = Instantiate(barterRequest.rewardItem);
                var stand = Methods.GetStandBlock(_npcBase.transform);
                //Set on surround block
                rew.SetOn(stand.island
                    .GetRandomSurroundStackableBlock(stand.coordinate));
                nowCount -= barterRequest.num;
            }
        }

        private IEnumerator OpenNumUI()
        {
            if (!_numUIInstance)
            {
                _numUIInstance = _uiManager.GenerateUI(_uiManager.fractionUIPrefab, transform.position);
                WorldUIManager.ChangeFractionUI(_numUIInstance, nowCount, barterRequest.num);
                yield return new WaitForSeconds(1f);
                _uiManager.DestroyUI(_numUIInstance);
            }
            else
            {
                WorldUIManager.ChangeFractionUI(_numUIInstance, nowCount, barterRequest.num);
            }
        }
        
        public void OnUIReveal(UIObject uiObject)
        {
            BarterUI ui = (BarterUI)uiObject;
            ui.SetBarterUI(barterRequest);
        }
    }
}