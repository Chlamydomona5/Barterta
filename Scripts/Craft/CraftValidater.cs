using System.Collections.Generic;
using System.Linq;
using Barterta.ItemGrid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Craft
{
    [CreateAssetMenu(fileName = "CraftValidater", menuName = "SO/CraftValidater")]
    public class CraftValidater : ScriptableObject
    {
        [ReadOnly] [SerializeField] public List<CraftRecipe> allRecipes;

        [Button("Reload All recipes")]
        private void Load()
        {
            allRecipes = Resources.LoadAll<CraftRecipe>("Crafting/Recipe").ToList();
            allRecipes.RemoveAll(x => x.craftResult.groundable == null);
        }

        public List<CraftRecipe> GetAllRecipeByType(CraftRecipeType type)
        {
            return allRecipes.FindAll(x => x.type == type);
        }

        public GroundableWithCount GetRecipeResult(GroundBlock[,] blockMatrix)
        {
            var table = TurnIntoCraftTable(blockMatrix);
            foreach (var recipe in allRecipes)
                if (recipe.Validate(table))
                    return recipe.craftResult;
            return null;
        }

        private CraftTable TurnIntoCraftTable(GroundBlock[,] blockMatrix)
        {
            //if size is not legal, return error
            if (blockMatrix.GetLength(0) < 3 || blockMatrix.GetLength(1) < 3)
                Debug.LogAssertion("block Matrix not in good size");
            var table = new CraftTable();
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
            {
                if(!blockMatrix[i,j]) continue;
                var count = blockMatrix[i, j].groundablesOn.Count;
                table.Table[i, j].count = count;
                //if its not empty, then copy bottom item's id.
                if (count > 0) table.Table[i, j].id = blockMatrix[i, j].groundablesOn[0].ID;
            }

            return table;
        }

        public CraftRecipe GetRecipeByResult(string id)
        {
            return allRecipes.Find(x => x.craftResult.groundable.ID == id);
        }

        
        public List<CraftRecipe> GetAllRecipeByMaterial(string id)
        {
            return allRecipes.FindAll(x => 
                //Find a craft table
                x.recipeList.Exists(y => 
                    //with a itemwithcount
                    y.Table.Cast<IdWithCount>().ToList().Exists(z => 
                        //has a id equal
                        z.id == id)));
        }
    }
}