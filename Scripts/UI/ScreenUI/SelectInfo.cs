using Barterta.ItemGrid;
using TMPro;
using UnityEngine;

namespace Barterta.UI.ScreenUI
{
    public class SelectInfo : MonoBehaviour
    {
        private TextMeshProUGUI _text;
    
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
    
        public void ChangeBlock(GroundBlock block)
        {
            //if block's groundablesOn list is not none, show the name and count of the groundable
            if (block && block.groundablesOn.Count > 0)
            {
                bool notTheSame = block.groundablesOn.Count > 1 && block.groundablesOn[0].ID != block.groundablesOn[1].ID;
                bool cantStack = !block.groundablesOn[0].stackable;
                if (notTheSame || cantStack)
                {
                    _text.text = block.groundablesOn[0].LocalizeName;
                }
                else
                {
                    _text.text = block.groundablesOn[0].LocalizeName + " x" +
                                 block.groundablesOn.Count;
                }
                //Load Info
                if(block.groundablesOn[0].Introduction != "")
                    _text.text += ":\n" + block.groundablesOn[0].Introduction;
            }
            else
            {
                _text.text = "";
            }
        }
    }
}