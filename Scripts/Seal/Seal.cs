using System.Collections;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Seal
{
    public class Seal : MonoBehaviour
    {
        [ReadOnly] public Mark.Mark destination;
        [SerializeField] private Transform models;
        private Coroutine _coroutine;

        public void StartTransport(Mark.Mark islandMark)
        {
            if (Methods.YtoZero(islandMark.transform.position - transform.position).magnitude > 30f)
            {
                destination = islandMark;
                _coroutine = StartCoroutine(Transport());   
            }
        }

        private IEnumerator Transport()
        {
            transform.DOLookAt(destination.transform.position - transform.position, .8f);
            models.DOMoveY(-2f, 1f);

            while (true)
            {
                var delta = (destination.transform.position - transform.position).normalized *
                            (UnityEngine.Time.fixedDeltaTime * 5f);
                delta = new Vector3(delta.x, 0, delta.z);
                transform.position += delta;
                yield return new WaitForFixedUpdate();
            }
        }

        //Triggered by collider
        public void EndTransport()
        {
            StopCoroutine(_coroutine);
            models.DOMoveY(-.5f, 1f);
        }
    }
}