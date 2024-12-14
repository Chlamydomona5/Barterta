using System;

namespace Barterta.ItemGrid
{
    [Serializable]
    public class IdWithCount
    {
        public string id = "";
        public int count;

        public IdWithCount(string id, int count)
        {
            this.id = id;
            this.count = count;
        }

        public IdWithCount()
        {
        }
    }
}