using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Barterta.ItemGrid;
using Barterta.StaminaAndHealth;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "KilnValidater", menuName = "SO/KilnValidater")]
public class KilnValidater : ScriptableObject
{
    [ReadOnly] [SerializeField] private List<Burnable> allRecipes;

    [Button("Reload All recipes")]
    private void Load()
    {
        allRecipes = Resources.LoadAll<Burnable>("").ToList();
    }
    
    public Burnable GetRecipeByResult(string id)
    {
        return allRecipes.Find(x => x.res.groundable.ID == id);
    }

    public List<Burnable> GetAllRecipe()
    {
        return allRecipes;
    }
    
    public List<Burnable> GetAllRecipeWithoutSameResult()
    {
        var res = new List<Burnable>();
        foreach (var recipe in allRecipes)
        {
            if (res.Find(x => x.res.groundable.ID == recipe.res.groundable.ID) == null)
            {
                res.Add(recipe);
            }
        }
        return res;
    }
}