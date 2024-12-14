using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Fishing
{
    public class FishVisionCollider : MonoBehaviour
    {
        private FishInWater _fish;
    
        private void Start()
        {
            _fish = GetComponentInParent<FishInWater>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Buoy"))
            {
                var buoy = other.GetComponentInParent<Buoy>();
                if (!buoy.triggered)
                {
                    buoy.Attract(_fish);
                    //move toward buoy
                    var position = transform.position;
                    var vec = Methods.YtoZero(buoy.transform.position - position);
                    _fish.AdjustRotation(Quaternion.LookRotation(vec.normalized), .8f);
                }
            }
            else if (other.CompareTag("HookNet"))
            {
                _fish.Leave();
            }
        }
    }
}