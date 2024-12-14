using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Barterta.Core
{
    public abstract class IDBase : SerializedMonoBehaviour
    {
        [SerializeField] protected string id;

        public virtual string ID
        {
            get => id;
            set { id = value; }
        }

        public virtual string LocalizeName => Methods.GetLocalText(ID);

        private bool _isIDed;

        private void OnValidate()
        {
            // On first OnValidate
            if (!_isIDed)
            {
                ID = gameObject.name;
                _isIDed = true;
            }
        }
    }
}