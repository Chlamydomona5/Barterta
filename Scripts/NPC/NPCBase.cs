using System.Collections;
using Barterta.Core;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.NPC
{
    [RequireComponent(typeof(Rigidbody))]
    public class NPCBase : SerializedMonoBehaviour
    {
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

        [Title("Move Parameters")] [Space] [SerializeField]
        private float speed;
        [SerializeField] private bool stay;

        [SerializeField] [MinMaxSlider(0f, 5f)]
        private Vector2 moveTimeRangeOnce = new(1f, 2f);

        [SerializeField] [MinMaxSlider(0f, 10f)]
        private Vector2 stopTimeRangeOnce = new(7f, 8f);

        private Animator _animator;
        private Rigidbody _rb;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            StartCoroutine(Move());
        }

        public void Freeze()
        {
            stay = true;
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
        }

        public void UnFreeze()
        {
            stay = false;
            _rb.isKinematic = false;
        }

        private IEnumerator Move()
        {
            float walkTimer;
            while (true)
            {
                if (!stay)
                {
                    //Random direction
                    Methods.RotateTowards(transform, Quaternion.Euler(0, Random.Range(0, 360f), 0));
                    _animator.SetBool(IsWalking, true);
                    var time = Random.Range(moveTimeRangeOnce.x, moveTimeRangeOnce.y);
                    walkTimer = 0f;
                    while (walkTimer < time)
                    {
                        walkTimer += UnityEngine.Time.fixedDeltaTime;
                        _rb.velocity = transform.forward * speed;
                        yield return new WaitForFixedUpdate();
                    }

                    //Stop and wait
                    _animator.SetBool(IsWalking, false);
                    _rb.velocity = Vector3.zero;
                    yield return new WaitForSeconds(Random.Range(stopTimeRangeOnce.x, stopTimeRangeOnce.y));
                }

                if (stay)
                {
                    _animator.SetBool(IsWalking, false);
                    yield return new WaitForSeconds(1f);
                }
            }
        }
    }
}