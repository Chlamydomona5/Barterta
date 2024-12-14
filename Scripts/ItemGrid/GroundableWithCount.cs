using System;

namespace Barterta.ItemGrid
{
    [Serializable]
    public class GroundableWithCount
    {
        public Groundable groundable;
        public int count = 1;
        
        public GroundableWithCount(Groundable groundable, int count)
        {
            this.groundable = groundable;
            this.count = count;
        }

        public GroundableWithCount(Groundable groundable)
        {
            this.groundable = groundable;
        }
    }
}