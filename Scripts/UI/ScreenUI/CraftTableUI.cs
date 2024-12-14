using System;
using Barterta.Craft;
using Barterta.UI.UIManage;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.ScreenUI
{
    public class CraftTableUI : SerializedMonoBehaviour
    {
        [SerializeField] private ItemWithCountUI[,] imageMatrix = new ItemWithCountUI[3,3];
        [SerializeField] private Image resImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        private IconInfo _iconInfo;

        private void Awake()
        {
            _iconInfo = Resources.Load<IconInfo>("IconInfo");
        }

        public void ChangeToNewTable(CraftRecipe recipe)
        {
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                imageMatrix[i, j].SetItemWithCount(recipe.recipeList[0].Table[i, j], _iconInfo);
            if(resImage)
                resImage.sprite = _iconInfo.GetIcon(recipe.craftResult.groundable.ID);
            if(nameText)
                nameText.text = recipe.craftResult.groundable.LocalizeName;
            if(descriptionText)
                descriptionText.text = recipe.craftResult.groundable.Introduction;
        }

        private void OnEnable()
        {
            nameText.text = "";
            descriptionText.text = "";
        }
    }
}