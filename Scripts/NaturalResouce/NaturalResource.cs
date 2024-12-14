using System.Collections.Generic;
using Barterta.ItemGrid;
using Barterta.Sound;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Barterta.NaturalResouce
{
    public class NaturalResource : SerializedMonoBehaviour
    {
        public string toolBoostType = "Vertical";
        [SerializeField] private int oriHealth = 10;
        [SerializeField] [ReadOnly] private int health;
        
        [SerializeField] protected string damageSound;
        [SerializeField] protected string destroySound;
        
        [SerializeField] protected Groundable afterDestory;
        [SerializeField] private Dictionary<Groundable,Vector2Int> _regularDrops;
        [SerializeField] private Dictionary<Groundable, float> _rareDrops;
        
        [SerializeField] private List<ParticleSystem> damagedEffect;
        private void Start()
        {
            health = oriHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            health -= damage;
            //Sound
            if (damage > 0)
                SoundManager.I.PlaySound(damageSound, Random.Range(0.7f, 1.3f));
            
            foreach (var particle in damagedEffect)
            {
                particle.Play();
            }

            if (health <= 0) DestorySelf();
            else transform.DOShakePosition(.2f, .05f, 100, 90f, false, true);
        }

        protected virtual void DestorySelf()
        {
            if (afterDestory) Instantiate(afterDestory).SetOn(GetComponent<Groundable>().blockUnder);
            SoundManager.I.PlaySound(destroySound);
            DestroyDrop();
            Destroy(gameObject);
        }

        protected virtual void DestroyDrop()
        {
            SoundManager.I.PlaySound(destroySound);

            if (_regularDrops != null)
                foreach (var drop in _regularDrops)
                {
                    for (int i = 0; i < Random.Range(drop.Value.x, drop.Value.y); i++)
                    {
                        DropAtRandomBlock(drop.Key);
                    }
                }

            if (_rareDrops != null)
                foreach (var drop in _rareDrops)
                {
                    if (Random.value < drop.Value)
                    {
                        DropAtRandomBlock(drop.Key);
                    }
                }

            Destroy(gameObject);
        }

        protected void DropAtRandomBlock(Groundable groundable)
        {
            var groundBlock = GetComponent<Groundable>().blockUnder;
            Instantiate(groundable)
                .SetOn(groundBlock.island.GetRandomSurroundStackableBlock(groundBlock.coordinate, groundable));
        }
    }
}