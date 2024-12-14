using Barterta.Craft;
using Barterta.ToolScripts;
using Barterta.UI.ScreenUI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Barterta.UI.WorldUI
{
    public class DialogObject : UIObject
    {
        [SerializeField] [ReadOnly] private DialogMode currentMode;
        [SerializeField] private TextMeshProUGUI textObject;
        [SerializeField] private CraftTableUI craftTableObject;

        public DialogMode CurrentMode
        {
            get => currentMode;
            set
            {
                currentMode = value;
                if (currentMode == DialogMode.Text)
                {
                    textObject.gameObject.SetActive(true);
                    craftTableObject.gameObject.SetActive(false);
                }
                else
                {
                    textObject.gameObject.SetActive(false);
                    craftTableObject.gameObject.SetActive(true);
                }
            }
        }

        public void ChangeText(string text)
        {
            textObject.text = text;
        }

        public void ChangeCraftTable(CraftRecipe recipe)
        {
            craftTableObject.ChangeToNewTable(recipe);
        }
    }
}