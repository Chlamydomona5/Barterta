using System.Collections.Generic;
using Barterta.ItemGrid;
using Barterta.UI.ScreenUI;
using Barterta.UI.UIManage;
using UnityEngine;

namespace Barterta.UI.WorldUI
{
    public class RequestUI : UIObject
    {
        [SerializeField] private Transform requestSlotParent;
        [SerializeField] private RequestSlotUI requestSlotPrefab;
        private Dictionary<string, RequestSlotUI> _idToRequestSlotUI = new();
        private IconInfo _iconInfo;

        public void Init(List<IdWithCount> request)
        {
            _iconInfo = Resources.Load<IconInfo>("IconInfo");
            for (int i = 0; i < request.Count; i++)
            {
                var requestSlotUI = Instantiate(requestSlotPrefab, requestSlotParent);
                requestSlotUI.Init(_iconInfo.GetIcon(request[i].id), 0, request[i].count);
                _idToRequestSlotUI.Add(request[i].id, requestSlotUI);
            }
        }
        
        public void SetCurrent(List<IdWithCount> current)
        {
            //Find the RequestSlotUI with the same id as the current item
            foreach (var idWithCount in current)
            {
                if (_idToRequestSlotUI.TryGetValue(idWithCount.id, out var requestSlotUI))
                {
                    requestSlotUI.SetCurrent(idWithCount.count);
                }
            }
        }
    }
}