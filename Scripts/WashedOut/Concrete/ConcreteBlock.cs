/*using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Interactable.Concrete
{
    public class ConcreteBlock : SerializedMonoBehaviour, IShortInteractOnHandEffector
    {
        private Material _material;
        [DictionaryDrawerSettings] public Dictionary<string, GameObject> IdToModelDict;

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (!trigger.detector.targetBlock)
            {
                var island = trigger.GetComponent<GridDetector>().GetStandBlock().island;
                trigger.HandBlock.groundablesOn[0].GetComponent<ConcreteBlock>()
                    .PlaceAt(island, island.PosToCoordinate(trigger.GetTowardPos(.6f)));
                return true;
            }

            return false;
        }

        public void Init(string id)
        {
            _material = IdToModelDict[id].GetComponent<MeshRenderer>().sharedMaterial;
            var md = Instantiate(IdToModelDict[id], transform, false);
            md.transform.localScale = new Vector3(.6f, .6f, .6f);
        }

        public void PlaceAt(Island.MONO.Island island, Vector2Int coordinate)
        {
            island.InsBlockWithSandAt(coordinate).ChangeMaterial(_material);
            Destroy(gameObject);
        }
    }
}*/