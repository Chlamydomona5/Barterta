using Barterta.ToolScripts;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Barterta.Monster.BehaviorTree
{
    public class HangAround: MonsterAction
    {
        public SharedFloat possToMove = .7f;
        public SharedFloat speed = 2f;
        public SharedVector2 timeRange = new Vector2(3, 10f);

        private float _timer = -1f;
        private bool _move;
        private Tween _rotateTween;

        public override TaskStatus OnUpdate()
        {
            if (_timer < 0)
            {
                _timer = Random.Range(timeRange.Value.x, timeRange.Value.y);
                if (Random.value < possToMove.Value)
                {
                    _rotateTween = RandomRotate();
                    _move = true;
                    Animator.SetBool("HangAround", true);
                }
                else
                {
                    _move = false;
                    Animator.SetBool("HangAround", false);
                }
            }
            else
            {
                //if velocity too low, random rotate
                if (_move && Rb.velocity.magnitude < (speed.Value / 8f) && (_rotateTween == null || !_rotateTween.active))
                {
                    _rotateTween = RandomRotate();
                }
                _timer -= UnityEngine.Time.deltaTime;
            }
            
            Rb.velocity = _move ? transform.forward * speed.Value : Vector3.zero;
            return TaskStatus.Running;
        }
        
        private Tween RandomRotate()
        {
            return Methods.RotateTowards(transform, Quaternion.Euler(0, Random.Range(0, 360f), 0));
        }
    }
}