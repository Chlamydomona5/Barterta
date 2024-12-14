using Barterta.ItemGrid;
using Barterta.UI.UIManage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.ScreenUI
{
    public class ItemWithCountUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countText;

        public void SetItemWithCount(IdWithCount idWithCount, IconInfo iconInfo)
        {
            var imageSprite = iconInfo.GetIcon(idWithCount.id);
            if (imageSprite)
            {
                image.sprite = imageSprite;
                image.color = Color.white;
            }
            else image.color = Color.clear;
            if(idWithCount.count > 1)
                countText.text = "x" + idWithCount.count.ToString();
            else countText.text = "";
        }
    }
}