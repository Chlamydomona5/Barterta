using Barterta.Craft;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.UI.ScreenUI
{
    public class DetailUI : MonoBehaviour
    {
        [SerializeField] private CraftTableUI craftTableUIPrefab;
        [SerializeField] private Transform craftTableUI;
        [SerializeField] private SelectInfo selectInfo;
        private CraftValidater _craftValidater;

        
        public void SetUI(GroundBlock block)
        {
            selectInfo.ChangeBlock(block);
            SetCraftTableUI(block);
        }

        private void SetCraftTableUI(GroundBlock block)
        {
            //if active
            if (craftTableUI.gameObject.activeSelf)
            {
                //Destroy all children
                foreach (Transform child in craftTableUI.transform)
                {
                    Destroy(child.gameObject);
                }
                //Load Table
                if(block.groundablesOn.Count == 0) return;
                var recipes = _craftValidater.GetAllRecipeByMaterial(block.groundablesOn[0].ID);
                if (recipes != null)
                {
                    foreach (var recipe in recipes)
                    {
                        var ui = Instantiate(craftTableUIPrefab, craftTableUI.transform);
                        ui.ChangeToNewTable(recipe);
                    }   
                }
            }
        }
    }
}