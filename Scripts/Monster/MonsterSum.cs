using System.Collections.Generic;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Monster
{
    [CreateAssetMenu(fileName = "MonsterSum", menuName = "SO/MonsterSum")]
    public class MonsterSum : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<MonsterForm, float> monsterFormToWeight = new();
    
        [Button]
        private void LoadMonsterForm()
        {
            var monsterForms = Resources.LoadAll<MonsterForm>("Monster");
            foreach (var monsterForm in monsterForms)
            {
                monsterFormToWeight.TryAdd(monsterForm, 1);
            }
        }

        public Monster GetOneMonster(int rangeIndex)
        {
            var form = Methods.GetRandomValueInDict(monsterFormToWeight);
            return form.GetMonster(rangeIndex);
        }
    }
}