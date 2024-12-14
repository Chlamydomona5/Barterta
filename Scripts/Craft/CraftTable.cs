using Barterta.ItemGrid;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Barterta.Craft
{
    //OK, I found out that 2d-array is not serialized by Unity System.
    //So I use [OdinSerialize] instead of [SerializedField] for mGrid (2d-array in AttackRange class).
    //Also I changed the class which contains AttackRange to be inherited from SerializedMonoBehaviour instead of MonoBehaviour and use [OdinSerialize] too.
    //This seems to solve the my problem
    public class CraftTable
    {
        [SerializeField] [TableMatrix(DrawElementMethod = "DrawElement")]
        public IdWithCount[,] Table;

        public CraftTable()
        {
            Table = new IdWithCount[3, 3]
            {
                { new(), new(), new() },
                { new(), new(), new() },
                { new(), new(), new() }
            };
        }

#if UNITY_EDITOR
        private static IdWithCount DrawElement(Rect rect, IdWithCount value)
        {
            value.id = EditorGUI.TextArea(rect.AlignLeft(rect.width * 0.8f), value.id);
            var style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            value.count = EditorGUI.IntField(rect.AlignRight(rect.width * 0.2f), value.count, style);
            //automaticly set item's count = 1
            if (value.id != "" && value.count == 0) value.count = 1;
            if (value.id == "" && value.count != 0) value.count = 0;
            //Debug.Log("value " + value.id + value.count);
            return value;
        }
#endif
    }
}