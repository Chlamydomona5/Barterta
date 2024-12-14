using UnityEngine;
using UnityEngine.Events;

namespace Barterta.InputTrigger
{
    public class AnimationTrigger : MonoBehaviour
    {
        public UnityEvent toolEffect;
        public UnityEvent toolBoostEnd;
        public UnityEvent boostCanEffect;
        
        public UnityEvent meleeAttackCollisionStart;
        public UnityEvent meleeAttackEnd;

        public void Effect()
        {
            //Debug.Log("Effect Triggered" + Time.time);
            toolEffect.Invoke();
        }

        public void BoostEnd()
        {
            //Debug.Log("BoostEnd Triggered" + Time.time);
            toolBoostEnd.Invoke();
        }

        public void CanEffect()
        {
            boostCanEffect.Invoke();
        }

        public void MeleeAttackCollisionStart()
        {
            meleeAttackCollisionStart.Invoke();
        }
        
        public void MeleeAttackEnd()
        {
            meleeAttackEnd.Invoke();
        }
        
    }
}