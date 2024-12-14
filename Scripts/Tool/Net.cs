using System.Collections.Generic;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using DG.Tweening;
using UnityEngine;

namespace Barterta.Tool
{
    public class Net : MonoBehaviour
    {
        [SerializeField] private int maxCount = 5;
        private List<Floatable.Floatable> _catchedFloatables;

        private void Awake()
        {
            _catchedFloatables = new List<Floatable.Floatable>();
        }

        private void Catch(Floatable.Floatable floatable)
        {
            _catchedFloatables.Add(floatable);

            Destroy(floatable.GetComponent<Rigidbody>());
            Destroy(floatable.GetComponent<Collider>());
            floatable.DOKill();

            Transform tran;
            (tran = floatable.transform).SetParent(transform);
            tran.localPosition = Methods.YtoZero(tran.localPosition);
        }

        public bool PopAllFloatablesTo(GroundBlock block, GrabTrigger trigger)
        {
            //Remove all null
            _catchedFloatables.RemoveAll(x => !x);
            bool ret = _catchedFloatables.Count > 0;
            bool entropy = false;
            if (block)
            {
                foreach (var floatable in _catchedFloatables)
                {
                    var happenEntropy = !floatable.TransferToGroundable(
                        block.island.GetRandomSurroundStackableBlock(block.coordinate, floatable.referGroundable));
                    entropy = entropy || happenEntropy;
                }
            }
            else
            {
                foreach (var floatable in _catchedFloatables)
                {
                    Destroy(floatable.gameObject);
                }
            }

            _catchedFloatables.Clear();
            if (entropy) trigger.GetComponent<DialogTrigger>().SelfBark("noenoughspaceforfloatable");
            return ret;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Catch floatables by collision
            if (_catchedFloatables.Count < maxCount)
            {
                if (other.GetComponent<Floatable.Floatable>()) Catch(other.GetComponent<Floatable.Floatable>());
            }
        }
    }
}