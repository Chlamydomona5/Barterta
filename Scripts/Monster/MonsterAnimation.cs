using UnityEngine;
using UnityEngine.Events;

namespace Barterta.Monster
{
    public class MonsterAnimation : MonoBehaviour
    {
        public UnityEvent attackStart;
        public UnityEvent attackEnd;
        public UnityEvent die;
    
        public void AttackStart()
        {
            attackStart.Invoke();
        }
    
        public void AttackEnd()
        {
            attackEnd.Invoke();
        }

        public void Die()
        {
            die.Invoke();
        }
    }
}