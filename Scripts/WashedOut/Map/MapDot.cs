using System;
using UnityEngine;

namespace Barterta.Map
{
    [Serializable]
    public class MapDot
    {
        public Sprite image;

        //This pos is world position, but y axis is not accurate
        public Vector3 pos;
        public Color color;
    }
}