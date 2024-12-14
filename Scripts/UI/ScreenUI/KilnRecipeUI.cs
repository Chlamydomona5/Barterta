using System;
using Barterta.Craft;
using Barterta.ItemGrid;
using Barterta.StaminaAndHealth;
using Barterta.UI.UIManage;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.ScreenUI
{
    public class KilnRecipeUI : SerializedMonoBehaviour
    {
        [SerializeField] private ItemWithCountUI material;
        [SerializeField] private ItemWithCountUI result;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        private IconInfo _iconInfo;

        private void Awake()
        {
            _iconInfo = Resources.Load<IconInfo>("IconInfo");
        }

        public void ChangeToNewRecipe(Burnable recipe)
        {
            material.SetItemWithCount(new IdWithCount(recipe.GetComponent<Groundable>().ID, 1), _iconInfo);
            result.SetItemWithCount(new IdWithCount(recipe.res.groundable.ID, recipe.res.count), _iconInfo);
            if(nameText)
                nameText.text = recipe.res.groundable.LocalizeName;
            if(descriptionText)
                descriptionText.text = recipe.res.groundable.Introduction;
        }

        private void OnEnable()
        {
            nameText.text = "";
            descriptionText.text = "";
        }
    }
}