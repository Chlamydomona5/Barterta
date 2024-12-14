using System;
using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.Damage;
using Barterta.InputTrigger;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Barterta.Weapon
{
    public class MeleeWeapon : Durable, IPressInteractOnHandEffector
    {
        public string attackAnimationName = "Slash";

        public MeleeAttribute Attribute;
        public MeleeEnchantment enchantment;
        public List<IDamagable> TargetsInOneAttack = new List<IDamagable>();

        [SerializeField] private Transform holder;

        [SerializeField] private bool isLogging;
        
        public bool IsAttacking
        {
            get => _isAttacking;
            set
            {
                _isAttacking = value;
                if(value)
                    TargetsInOneAttack.Clear();
            }
        }
        private bool _isAttacking;
        
        private ParticleSystem hitParticle;

        protected override void Start()
        {
            base.Start();
            holder = GetComponentInParent<IDamagable>()?.gameObject.transform;
            hitParticle = Resources.Load<ParticleSystem>("VFXPrefab/Hit");
        }

        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return false;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (durability > 0 || infiniteDurability)
            {
                trigger.GetComponent<CombatTrigger>().AttackWith(this);
                holder = trigger.transform;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (IsAttacking)
            {
                IDamagable damagable;
                damagable = other.GetComponentInParent<IDamagable>();
                if (damagable != null && !TargetsInOneAttack.Contains(damagable))
                {
                    TargetsInOneAttack.Add(damagable);
                    DealDamage(damagable);
                    DurabilityChange(-1);
                }
            }
        }
    
        private void DealDamage(IDamagable damagable)
        {
            if(isLogging) Debug.Log(holder.name + "Deal Damage To " + damagable);
            if (damagable != null)
            {
                Instantiate(hitParticle, transform.position, Quaternion.identity);
                damagable.TakeDamage(holder, Attribute);
            }
        }
    }
}