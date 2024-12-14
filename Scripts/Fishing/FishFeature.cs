using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Fishing
{
    [CreateAssetMenu(fileName = "FishFeature", menuName = "SO/FishFeature")]
    public class FishFeature : ScriptableObject
    {
        [Title("Rarity")] public ToolScripts.Rarity rarity = ToolScripts.Rarity.Rare;

        [Title("FreeMoveMode")] public Vector2 freeMoveSpeedRange = new(1f, 2f);

        public Vector2 boundBlockDisRange = new(2f, 8f);
        public Vector2 freeMoveTimeRange = new(1.5f, 5f);
        public Vector2 freeStayTimeRange = new(5f, 15f);
        public Vector2 leaveTimeRange = new(90, 120);

        [Title("AttractedMode")] public Vector2 stayFreeTimeRange = new(1f, 2f);

        public Vector2 stayFollowTimeRange = new(.5f, 1.5f);
        public Vector2 freeSpeedRange = new Vector2(.8f,1.5f);
        public float followSpeed = 1;
    
        //At Least 1.5f
        public float escapeRange = 1.8f;
    }
}