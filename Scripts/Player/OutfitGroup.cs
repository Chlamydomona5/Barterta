using System;
using System.Collections.Generic;
using UnityEngine;

namespace Barterta.Player
{
    [Serializable]
    public class OutfitGroup
    {
        public Transform Parent;
        public List<GameObject> Prefabs;
    }
}