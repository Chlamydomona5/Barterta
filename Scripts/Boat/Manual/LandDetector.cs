using Barterta.ItemGrid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Boat.Manual
{
    public class LandDetector : MonoBehaviour
    {
        [ReadOnly] public GameObject surroundBlock;

        private void OnTriggerEnter(Collider other)
        {
            if (!surroundBlock && other.GetComponent<GroundBlock>() && other.GetComponent<GroundBlock>().island)
                surroundBlock = other.gameObject;
        }

        private void OnTriggerExit(Collider other)
        {
            if (surroundBlock && other.GetComponent<GroundBlock>() && other.GetComponent<GroundBlock>().island)
                if (surroundBlock == other.gameObject)
                    surroundBlock = null;
        }
    }
}