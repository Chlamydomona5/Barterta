using System.Collections.Generic;
using Barterta.Craft;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;

public class CraftRecipeContainer : MonoBehaviour
{
    public List<CraftRecipe> unlockedRecipes = new();
    private CraftValidater _craftValidater;
    private TipController _tipController;

    private void Awake()
    {
        _craftValidater = Resources.Load<CraftValidater>("Crafting/CraftValidater");
        _tipController = GetComponent<TipController>();
        //Test
        unlockedRecipes = _craftValidater.allRecipes;
    }

    public void UnlockRecipeByMaterial(string id)
    {
        //Add all recipes that can be crafted by this material
        var allRecipeByMaterial = _craftValidater.GetAllRecipeByMaterial(id);
        unlockedRecipes.AddRange(allRecipeByMaterial);
        //Show All Unlock Tip
        foreach (var recipe in allRecipeByMaterial)
        {
            _tipController.Tip(Methods.GetLocalText("UnlockRecipe") + recipe.craftResult.groundable.LocalizeName);
        }
    }
}