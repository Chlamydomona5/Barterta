using System.Collections;
using System.Collections.Generic;
using Barterta.Core;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Monster
{
    public class MonsterController : MonoBehaviour
    { 
        [SerializeField,ReadOnly] private List<Monster> monsters = new();
        public bool IsEmpty => monsters.Count == 0;
    
        public Monster InstantiateMonster(Monster monsterPrefab, GroundBlock block)
        {
            var monster = Instantiate(monsterPrefab,
                block.transform.position + Constant.ChunkAndIsland.BlockSize / 2 * Vector3.up, Quaternion.identity,
                transform);
            monster.Init(this);
            monsters.Add(monster);
            return monster;
        }
        
        public Monster InstantiateMonsterOnRandomEdgeBlock(Monster monsterPrefab, Island.MONO.Island island)
        {
            var edges = island.GetAllEdgeBlock();
            var block = edges[Random.Range(0, edges.Count)];
            return InstantiateMonster(monsterPrefab, block);
        }

        public void RemoveMonster(Monster monster)
        {
            monsters.Remove(monster);
        }

        public void StartRaid(List<MonsterWave> waves, int interval)
        {
            StartCoroutine(RaidCoroutine(waves, interval));
        }

        private IEnumerator RaidCoroutine(List<MonsterWave> waves, int interval)
        {
            foreach (var wave in waves)
            {
                foreach (var monsterWithCount in wave.MonsterWithCount)
                {
                    for (var i = 0; i < monsterWithCount.Value; i++)
                    {
                        InstantiateMonsterOnRandomEdgeBlock(monsterWithCount.Key, HomeManager.I.homeIsland);
                    }
                }
                yield return new WaitForSeconds(interval);
            }
        }
    }
}