using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.ToolScripts
{
    public class RandomModel : MonoBehaviour
    {
        public List<GameObject> childModels;
        public bool scaleChange = true;

        [Range(0, 1)] [ShowIf("scaleChange")] public float scaleChangeRange = .1f;

        private void Awake()
        {
            foreach (var model in childModels) model.SetActive(false);

            var chose = childModels[Random.Range(0, childModels.Count)];
            chose.SetActive(true);

            if (scaleChange)
            {
                var scale = chose.transform.localScale;
                chose.transform.localScale = scale * Random.Range(1 - scaleChangeRange, 1 + scaleChangeRange);
            }
        }
    }
}