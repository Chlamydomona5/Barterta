using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Barterta.Player
{
    public class Outfit : MonoBehaviour
    {
        private GameObject _hair;
        [SerializeField] private SkinnedMeshRenderer bodyRenderer;
        [SerializeField] private List<Material> bodyMaterials;
    
        public OutfitGroup hairGroup;
        public List<OutfitGroup> OutfitGroups;

        public void Start()
        {
            bodyRenderer.material = bodyMaterials[Random.Range(0, bodyMaterials.Count)];

            foreach (var group in OutfitGroups)
            {
                Instantiate(group.Prefabs[Random.Range(0, group.Prefabs.Count)], group.Parent, false);
            }
            
            _hair = Instantiate(hairGroup.Prefabs[Random.Range(0, hairGroup.Prefabs.Count)], hairGroup.Parent, false);
        }

        public void SetActiveHair(bool active)
        {
            _hair.SetActive(active);   
        }
    }
}