using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Hat
{
    public class Hat : Groundable, IShortInteractOnHandEffector
    {
        [SerializeField] private Transform modelTransform;
        [SerializeField] private Vector3 groundPosition;
        [SerializeField] private Vector3 hatPostion;

        private void Start()
        {
            groundPosition = transform.localPosition;
            if(!modelTransform) modelTransform = transform.GetChild(0);
        }

        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return true;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            trigger.GetComponent<HatContainer>().SetHat(this, trigger);
        }

        public void SetModelPosition(bool isToHead)
        {
            modelTransform.localPosition = isToHead ? hatPostion : groundPosition;
        }
    }
}