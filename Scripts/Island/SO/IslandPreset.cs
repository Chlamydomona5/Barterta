using Barterta.Island.SO.IslandType;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Island.SO
{
    [CreateAssetMenu(fileName = "PresetIsland", menuName = "SO/IslandPreset")]
    public class IslandPreset : ScriptableObject
    {
        public string Id => Methods.GetLocalText(name);
        public IslandForm islandForm;
        public Vector2Int coordinate;
    }
}