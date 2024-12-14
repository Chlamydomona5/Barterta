using Barterta.Weapon;
using UnityEngine;

namespace Barterta.Damage
{
    public interface IDamagable
    {
        //Get Monobehavior by interface
        GameObject gameObject { get ; } 
        
        public abstract void TakeDamage(Transform from, MeleeAttribute meleeWeapon);
    }
}