using Sirenix.OdinInspector;

namespace Barterta.ToolScripts
{
    public class Singleton<T> : SerializedMonoBehaviour where T:SerializedMonoBehaviour//单例模式基类
    {
        private static T _instance;

        public static T I => _instance;

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}