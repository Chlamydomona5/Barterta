using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using UnityEngine;

namespace Barterta.Interactable
{
    public class Bed : MonoBehaviour, IShortInteractOnGroundEffector,ILongInteractOnGroundEffector
    {
        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return true;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            trigger.GetComponent<SleepTrigger>().StartSleep(trigger.transform.position, transform.position);
        }
    }
}