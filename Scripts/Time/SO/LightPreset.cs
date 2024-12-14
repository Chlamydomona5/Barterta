using UnityEngine;

namespace Barterta.Time.SO
{
    [CreateAssetMenu(fileName = "LightPreset", menuName = "SO/LightPreset")]
    public class LightPreset : ScriptableObject
    {
        public Gradient ambientColor;
        public Gradient directionalColor;
        public Gradient fogColor;
    }
}