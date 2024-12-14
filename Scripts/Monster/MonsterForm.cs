using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Monster
{
    [CreateAssetMenu(fileName = "MonsterForm", menuName = "SO/MonsterForm")]
    public class MonsterForm : ScriptableObject
    {
        [SerializeField] private bool isEvolutionalMonster;
        [ShowIf("isEvolutionalMonster"), SerializeField]
        private List<Monster> rangeToMonsters;
        [HideIf("isEvolutionalMonster"), SerializeField]
        private List<Monster> monsterPool;

        /// <summary>
        /// if isEvolutionalMonster, return monster, if is special monster, return a random in pool
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public Monster GetMonster(int range)
        {
            if (isEvolutionalMonster)
            {
                if (rangeToMonsters != null) return rangeToMonsters[range];
            }
            else if (monsterPool != null && monsterPool.Count > 0) return monsterPool[Random.Range(0, monsterPool.Count)];
        
            Debug.LogError("MonsterForm " + name + " is empty");
            return null;
        }
    }
}