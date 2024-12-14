using System;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Time
{
    [Serializable]
    public class DayStagePreset
    {
        public DayStage stage;
        [MaxValue(1f)] public float percentage;
        public Color ambientColor;
        public Color lightColor;
    }
}