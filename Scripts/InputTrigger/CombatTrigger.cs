using System;
using System.Collections;
using Barterta.Damage;
using Barterta.Sound;
using Barterta.StaminaAndHealth;
using Barterta.Weapon;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class CombatTrigger : InputTriggerBase, IDamagable
    {
        [SerializeField] private AttributeContainer healthContainer;
        [SerializeField, ReadOnly] private MeleeWeapon nowWeapon;
        private PlayerParticleController particleController;

        private void Start()
        {
            particleController = GetComponent<PlayerParticleController>();
        }

        public override void OnInteractPress(InputAction.CallbackContext ctx)
        {
        }

        public override void OnShortGrab(InputAction.CallbackContext ctx)
        {
        }

        public override void OnLongGrab(InputAction.CallbackContext ctx)
        {
        }

        public override void OnInteractCancel(InputAction.CallbackContext ctx)
        {
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
        }

        public override void EmergentExit()
        {
            AttackEnd();
        }

        public void AttackWith(MeleeWeapon weapon)
        {
            //Preparation
            nowWeapon = weapon;
            StateController.ChangeAllState(this);
            //Animation
            Animator.SetTrigger(weapon.attackAnimationName);
        }
    
        public void AttackCollisionStart()
        {
            nowWeapon.IsAttacking = true;
            SoundManager.I.PlaySound("Slash");
            particleController.PlayParticle("Slash");
        }
    
        public void AttackEnd()
        {
            StateController.ChangeToDefault();
            nowWeapon.IsAttacking = false;
        }
        
        public void TakeDamage(Transform from, MeleeAttribute meleeWeapon)
        {
            StateController.ChangeToDefault();
            //StartCoroutine(KnockedBack());
            //Take damage
            healthContainer.Consume(meleeWeapon.Damage);
            //Knock back
            var direction = (transform.position - from.position).normalized;
            Rb.AddForce(direction * meleeWeapon.KnockForce, ForceMode.Impulse);
            //Sound
            SoundManager.I.PlaySound("Hit");
        }
    }
}