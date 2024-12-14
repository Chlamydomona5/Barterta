using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.RagDoll
{
    public class RagdollContainer : MonoBehaviour
    {
        [SerializeField, ReadOnly] private List<Rigidbody> bonesRb;
        [SerializeField, ReadOnly] private List<Collider> bonesCd;
        public Rigidbody root;

        private void Awake()
        {
            bonesRb = GetComponentsInChildren<Rigidbody>().ToList();
            bonesCd = GetComponentsInChildren<Collider>().ToList();
            DisableRagdoll();
        }

        public void EnableRagdoll()
        {
            foreach (var rb in bonesRb)
            {
                rb.isKinematic = false;
                rb.useGravity = false;
            }

            foreach (var cd in bonesCd)
            {
                cd.enabled = true;
            }
        }

        public void EnableGravity()
        {
            foreach (var rb in bonesRb)
            {
                rb.useGravity = true;
            }
        }
    
        public void DisableRagdoll()
        {
            foreach (var rb in bonesRb)
            {
                rb.isKinematic = true;
            }
        
            foreach (var cd in bonesCd)
            {
                cd.enabled = false;
            }
        }
    }
}