using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Barterta.ItemGrid
{
    [CreateAssetMenu(fileName = "ItemSet", menuName = "SO/ItemSet")]
    public class ItemSet : ScriptableObject
    {
        public List<Groundable> list;

        public Groundable Random()
        {
            return list[UnityEngine.Random.Range(0, list.Count())];
        }
    }
}