/*using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Interactable
{
    public class RequestDesk : MonoBehaviour, IPutGroundableOn, IBeSettled
    {
        [HideInInspector] public GroundBlock blockUnder;
        [HideInInspector] public string nowId;
        [SerializeField] private Transform displayParent;

        private void Start()
        {
            blockUnder = GetComponent<Groundable>().blockUnder;
        }

        public void OnSettled(GroundBlock block)
        {
            if (blockUnder && blockUnder.island) blockUnder.island.requestDesks.Remove(this);
            blockUnder = block;
        }

        public bool JudgePut(Groundable groundable)
        {
            return true;
        }

        public void EffectBeforeSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
        }


        public void EffectAfterSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
            nowId = groundable.id;
            //Switch model
            if (displayParent.childCount > 0) Destroy(displayParent.GetChild(0).gameObject);

            var tsf = groundable.transform;
            tsf.SetParent(displayParent, false);
            tsf.localPosition = Vector3.zero;
            Destroy(groundable);
        }
    }
}*/