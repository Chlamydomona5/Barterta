using Barterta.ItemGrid;
using Barterta.UI.UIManage;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Barterta.UI.WorldUI
{
    public class RequestSlotUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI ratioText;

        public void Init(Sprite image, int current, int max)
        {
            itemImage.sprite = image;
            ratioText.text = current + "/" + max;
        }
        
        public void SetCurrent(int current)
        {
            //Set the text before “/” to current
            ratioText.text = current + "/" + ratioText.text.Split('/')[1];
        }
    }
}