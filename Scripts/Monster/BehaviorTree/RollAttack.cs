using Barterta.ToolScripts;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Barterta.Monster.BehaviorTree
{
    public class RollAttack: MonsterAction
    {
        public SharedFloat speed = 4f;
        public SharedFloat time = 1.5f;
        public SharedFloat guideStep = 0f;
        
        public SharedTransform target;

        private float timer;

        public override void OnStart()
        {
            base.OnStart();
            timer = 0;
            //Rotate to player
            transform.rotation = Quaternion.LookRotation(target.Value.position - transform.position);
            //Set Animation
            Animator.SetBool("RollAttack", true);
            //Set Attack Collider
            Monster.AttackStart();
        }

        public override TaskStatus OnUpdate()
        {
            if (timer > time.Value)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            base.OnEnd();
            Animator.SetBool("RollAttack", false);
            Rb.velocity = Vector3.zero;
            Monster.AttackEnd();
        }

        public override void OnFixedUpdate()
        {
            timer += UnityEngine.Time.fixedDeltaTime;
            //Adjust rotation by guideStep
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.Value.position - transform.position), guideStep.Value);
            //Move forward
            Rb.velocity = transform.forward * speed.Value;
        }
        
    }
}