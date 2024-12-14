using System.Collections.Generic;
using System.Linq;
using Barterta.ItemGrid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Craft
{
    [CreateAssetMenu(fileName = "CraftRecipe", menuName = "SO/CraftRecipe")]
    public class CraftRecipe : SerializedScriptableObject
    {
        public CraftRecipeType type;
        
        public GroundableWithCount craftResult;
        public List<CraftTable> recipeList;

        [Button]
        private void GetItemByName()
        {
            craftResult.groundable = Resources.LoadAll<Groundable>("").ToList().Find(x => x.ID == name);
            craftResult.count = 1;
        }

        public bool Validate(CraftTable table)
        {
            foreach (var testTable in recipeList)
            {
                var correct = true;
                //if not equal, change a recipe
                for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    //Not Related to player's direction
                    //2 - x is a transpose since inspector is displayed as
                    //   0 1 2                        0 1 2
                    // 0            but in world is 2 
                    // 1                            1
                    // 2                            0
                    if (testTable.Table[i, 2 - j].id != table.Table[i, j].id ||
                        testTable.Table[i, 2 - j].count != table.Table[i, j].count)
                        correct = false;
                //if correct, return
                if (correct) return true;
            }

            //all not validated
            return false;
        }
    }
    
    public enum CraftRecipeType
    {
        Tool,
        Item,
        Boat,
        Science
    }
}