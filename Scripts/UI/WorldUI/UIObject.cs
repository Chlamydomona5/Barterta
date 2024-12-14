using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.WorldUI
{
    public class UIObject : MonoBehaviour
    {
        [ReadOnly] public float originScale;
        public Dictionary<Image, Color> ImageOriginalColorDict = new();
        public Dictionary<TextMeshProUGUI, Color> TextOriginalColorDict = new();


        private void Awake()
        {
            InitObejctOriginalColorDict();
            originScale = transform.localScale.y;
        }

        private void InitObejctOriginalColorDict()
        {
            foreach (var text in GetComponentsInChildren<TextMeshProUGUI>())
                TextOriginalColorDict.Add(text, text.color);

            foreach (var image in GetComponentsInChildren<Image>()) ImageOriginalColorDict.Add(image, image.color);
        }
    }
}