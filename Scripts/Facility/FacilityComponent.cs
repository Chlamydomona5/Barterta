using Barterta.ItemGrid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Facility
{
    public class FacilityComponent : Groundable
    {
        public Vector2Int relativeCoord;
        [ReadOnly] public FacilityEntity facility;
        
        public virtual void Init(FacilityEntity parent, GroundBlock block)
        {
            SetOn(block);
            facility = parent;
        }
    }
}