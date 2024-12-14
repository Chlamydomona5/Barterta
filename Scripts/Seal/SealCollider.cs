using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Seal
{
    public class SealCollider : MonoBehaviour
    {
        private Seal _boat;

        private void Start()
        {
            _boat = GetComponentInParent<Seal>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<GroundBlock>() && _boat && _boat.destination &&
                other.GetComponent<GroundBlock>().island == _boat.destination.GetComponent<Groundable>().blockUnder.island) _boat.EndTransport();
        }
    }
}