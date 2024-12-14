using Barterta.ToolScripts;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Monster.BehaviorTree
{
    public class Seek : MonsterAction
    {
        public LayerMask TargetMask = LayerMask.GetMask("Player");
        public LayerMask ObstructionMask = new LayerMask();

        public SharedFloat Speed = 2;
        public SharedFloat StopDistance = 1;

        public SharedTransform Target;

        public override TaskStatus OnUpdate()
        {
            //If close enough, stop, turn and attack
            if (Vector3.Magnitude(transform.position - Target.Value.position) < StopDistance.Value)
            {
                Methods.RotateTowards(transform,
                    Quaternion.LookRotation(Methods.YtoZero(Target.Value.position - transform.position)));
                Animator.SetBool("Seek", false);
                return TaskStatus.Success;
            }

            Animator.SetBool("Seek", true);
            transform.rotation = Quaternion.LookRotation(Methods.YtoZero(Target.Value.position - transform.position));
            Rb.velocity = transform.forward * Speed.Value;
            return TaskStatus.Running;
        }
    }
}