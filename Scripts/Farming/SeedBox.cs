using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Farming
{
    public class SeedBox : MonoBehaviour
    {
        public ToolScripts.Rarity rarity;
        public Groundable seed;
        public Transform plantDisplayPos;

        public void Init(Groundable s ,ToolScripts.Rarity r)
        {
            //Assign
            seed = s;
            rarity = r;
            //Model
            var plant = seed.GetComponent<Seed>().resultCrop;
            var go = Instantiate(plant).gameObject;
            Destroy(go.GetComponent<Groundable>());
            go.transform.SetParent(transform);
            go.transform.position = plantDisplayPos.position;
            //Rarity
            Methods.RarityOutline(gameObject, rarity);
        }

        public void PlantSeed(GroundBlock block)
        {
            GetComponent<Groundable>().BeRemovedFromNowBlock();
            //Debug.Log("Planted");
            var seedInstance = Instantiate(seed);
            var seedComponent = seedInstance.GetComponent<Seed>();
            //Set groundables
            seedInstance.SetOn(block);
            //Rarity
            seedComponent.RarityInit(rarity);
            //Set Seed
            seedComponent.BeginToGrow();
            //DestorySelf
            Destroy(gameObject);
        }
    }
}