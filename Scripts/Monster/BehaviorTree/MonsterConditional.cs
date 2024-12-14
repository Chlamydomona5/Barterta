using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Barterta.Monster.BehaviorTree
{
    public class MonsterConditional : Conditional
    {
        protected Rigidbody Rb;
        protected Animator Animator;
        protected Monster Combat;

        public override void OnAwake()
        {
            base.OnAwake();
            Rb = GetComponent<Rigidbody>();
            Combat = GetComponent<Monster>();
            Animator = gameObject.GetComponentInChildren<Animator>();
        }
        
    }
}