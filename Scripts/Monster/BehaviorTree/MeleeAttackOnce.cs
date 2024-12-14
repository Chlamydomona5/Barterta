using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Barterta.Monster.BehaviorTree
{
    public class MeleeAttackOnce : MonsterAction
    {
        public override TaskStatus OnUpdate()
        {
            Rb.velocity = Vector3.zero;
            Animator.SetTrigger("MeleeAttack");
            return TaskStatus.Success;
        }
        
    }
}