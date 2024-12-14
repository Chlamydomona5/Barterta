using System.Collections.Generic;
using System.Linq;
using Barterta.ItemGrid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Facility.Construction
{
    public class ConstructionSite : FacilityEntity
    {
        public FacilityRecipe Recipe;
        [SerializeField, ReadOnly] public List<IdWithCount> currentItems = new();
        public override int Size
        {
            get => Recipe.TargetFacility.Size;
        }
        
        public override void Init(FacilityRecipe recipe, Vector2Int coord)
        {
            Recipe = recipe;
            base.Init(recipe, coord);
            //Set currentItems equal to requirement while count = 0
            foreach (var item in recipe.RequiredItems)
            {
                currentItems.Add(new IdWithCount(item.id, 0));
            }
        }

        public bool CanAddItem(string id)
        {
            //If item is not required, return false
            if (Recipe.RequiredItems.All(i => i.id != id)) return false;
            var index = currentItems.FindIndex(i => i.id == id);
            
            if (index == -1) Debug.Log("Construction site Item not found and not return false");
            //if is already full, return false
            else if (currentItems[index].count >= Recipe.RequiredItems[index].count) return false;
            
            return true;
        }

        public void AddItem(string id)
        {
            var index = currentItems.FindIndex(i => i.id == id);
            currentItems[index].count++;
            //If is full, Construct
            if (currentItems.All(i => i.count >= Recipe.RequiredItems.First(r => r.id == i.id).count))
            {
                Construct();
            }
        }

        private void Construct()
        {
            //Instantiate target facility
            var facility = Instantiate(Recipe.TargetFacility, transform.parent, false);
            facility.transform.position = transform.position;
            //Destroy all components
            foreach (var component in components)
            {
                DestroyImmediate(component.gameObject);
            }
            //Transfer placeholder to facility placeholders
            facility.placeHolders = placeHolders;
            facility.Init(Recipe, IslandCoordinate);
            //Destroy this
            Destroy(gameObject);
        }
    }
}