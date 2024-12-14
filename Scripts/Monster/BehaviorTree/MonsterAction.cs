using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Barterta.Monster.BehaviorTree
{
    public class MonsterAction : Action
    {
        protected Rigidbody Rb;
        protected Animator Animator;
        protected Monster Monster;

        public override void OnAwake()
        {
            base.OnAwake();
            Rb = GetComponent<Rigidbody>();
            Animator = gameObject.GetComponentInChildren<Animator>();
            Monster = GetComponent<Monster>();
        }

        protected void ResetAnimator()
        {
            foreach (var parameter in Animator.parameters)
            {
                if(parameter.type == AnimatorControllerParameterType.Bool) Animator.SetBool(parameter.name, false);
            }
        }
    }
}