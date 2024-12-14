using UnityEngine;

namespace Barterta.ToolScripts
{
    public class CenteredMap<T>
    {
        private T[,] blockMap;
        private int maxSize;

        public CenteredMap(int size)
        {
            maxSize = size;
            blockMap = new T[size, size];
        }

        public T[,] Map => blockMap;

        public T this[int x, int y]
        {
            get => blockMap[x + maxSize / 2, y + maxSize / 2];
            set => blockMap[x + maxSize / 2, y + maxSize / 2] = value;
        }

        public T this[Vector2Int vec]
        {
            get => blockMap[vec.x + maxSize / 2, vec.y + maxSize / 2];
            set => blockMap[vec.x + maxSize / 2, vec.y + maxSize / 2] = value;
        }
    }
}