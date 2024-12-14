using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.UI.UIManage
{
    [CreateAssetMenu(fileName = "IDInfo", menuName = "SO/IDInfo")]
    public class IconInfo : ScriptableObject
    {
        [SerializeField,ReadOnly] private List<Sprite> allIcons;

        [Button]
        private void LoadAll()
        {
            allIcons = Resources.LoadAll<Sprite>("IconExports").ToList();
        }

        public Sprite GetIcon(string id)
        {
            return allIcons.Find(x => x.name == id);
        }
    }
}