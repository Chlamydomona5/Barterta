using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Barterta.Monster.BehaviorTree
{
    public class CanSeePlayer : MonsterConditional
    {
        public SharedFloat Radius = 10;
        public SharedFloat Angle = 45;
        public SharedFloat IgnoreAngleRadius = 1.5f;
        
        public LayerMask TargetMask = LayerMask.GetMask("Player");
        public LayerMask ObstructionMask = new LayerMask();
        
        public SharedTransform Target;

        public override TaskStatus OnUpdate()
        {
            //Scan sphere
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius.Value, TargetMask);
            
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                //Check angle
                if (Vector3.Angle(transform.forward, directionToTarget) < Angle.Value / 2 || distanceToTarget < IgnoreAngleRadius.Value)
                {
                    //Check obstruction
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask))
                    {
                        Target.Value = target;
                        return TaskStatus.Success;
                    }
                    else
                        return TaskStatus.Failure;
                }
            }
            return TaskStatus.Failure;
        }
    }
}