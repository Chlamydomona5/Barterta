using System.Collections.Generic;
using Barterta.Damage;
using Barterta.ItemGrid;
using Barterta.Sound;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using Barterta.Weapon;
using DamageNumbersPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Barterta.Monster
{
    public class Monster : SerializedMonoBehaviour, IDamagable
    {
        public int xp = 5;
        public int maxHealth = 10;
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                _healthBar.ChangeTo((float)_health / maxHealth);
            }
        }
        private int _health;

        /*public Dictionary<Groundable, float> LootToPossDict = new();
        public int lootCount = 1;*/
        //public bool isStill = false;

        private Rigidbody _rb;
        private Animator _animator;
        private MeleeWeapon _weapon;
        private BehaviorDesigner.Runtime.BehaviorTree _behaviorTree;
        private MonsterController _monsterController;
        private ProgressBarUI _healthBar;
        private DamageNumber _damageNumber;
        
        private XPOrb _xpOrbPrefab;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _weapon = GetComponentInChildren<MeleeWeapon>();
            _behaviorTree = GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
            _damageNumber = Resources.Load<DamageNumber>("UI/World/DamageNumber");

            _healthBar =
                (ProgressBarUI)(WorldUIManager.I.GenerateUI(Resources.Load<ProgressBarUI>("UI/World/MonsterHealth"), transform, -.2f));
            
            _xpOrbPrefab = Resources.Load<XPOrb>("Monster/XPOrb");
            
            Health = maxHealth;

        }

        public void Init(MonsterController monsterController)
        {
            _monsterController = monsterController;
        }

        private void FixedUpdate()
        {
            //Linear friction and angular friction
            _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, 0.1f);
            _rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, Vector3.zero, 0.1f);
        }

        /// <summary>
        /// Will triggered by behavior tree
        /// </summary>
        void IDamagable.TakeDamage(Transform from, MeleeAttribute meleeWeapon)
        {
            //Debug.Log(name + " is Taking Damage By " + from.name);
            _behaviorTree.SendEvent("isTakingDamage");
            //Take damage
            Health -= meleeWeapon.Damage;
            if(Health <= 0) StartToDie();
            //Knock back
            var direction = (transform.position - from.position).normalized;
            _rb.AddForce(direction * meleeWeapon.KnockForce, ForceMode.Impulse);
            //Animation
            _animator.SetTrigger("TakeDamage");
            //Damage Number at a random position near the monster
            Instantiate(_damageNumber).Spawn(transform.position + Random.onUnitSphere * .2f, meleeWeapon.Damage);
            //Sound
            SoundManager.I.PlaySound("Hit_1");
        }

        public void StartToDie()
        {
            _behaviorTree.enabled = false;
            _animator.SetBool("Die", true);
        }
    
        public void Die()
        {
            DropXP();
            Debug.Log(name + " is Dead");
            _monsterController.RemoveMonster(this);
            Destroy(_healthBar.gameObject);
            Destroy(gameObject);
        }

        public virtual void AttackStart()
        {
            _weapon.IsAttacking = true;
        }
    
        public virtual void AttackEnd()
        {
            _weapon.IsAttacking = false;
        }

        public void DropXP()
        {
            for(int i = 0; i < xp; i++)
            {
                var xpOrb = Instantiate(_xpOrbPrefab, transform.position + Random.onUnitSphere * .5f, Quaternion.identity); 
            }
        }
    }
}