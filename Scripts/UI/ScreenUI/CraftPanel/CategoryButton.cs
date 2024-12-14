using Barterta.Craft;
using UnityEngine;
using UnityEngine.Serialization;

public class CategoryButton : MonoBehaviour
{
    public RecipeType recipeType;
    public CraftRecipeType craftType;
    private CraftPanelUI _craftPanelUI;

    public void Init(CraftPanelUI craftPanelUI)
    {
        _craftPanelUI = craftPanelUI;
    }
    
    public void OnClick()
    {
        //log
        Debug.Log("CategoryButton: " + craftType);
        _craftPanelUI.ChooseCategory(recipeType, craftType);
    }
}