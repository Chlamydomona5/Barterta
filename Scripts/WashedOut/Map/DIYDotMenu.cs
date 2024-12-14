using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.Map
{
    public class DIYDotMenu : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private int selectedSpriteIndex;
        [SerializeField] [ReadOnly] private int selectedColorIndex;

        //0=>sprite,1=>color
        [SerializeField] [ReadOnly] private int selector;

        [Title("UI Reference")] [SerializeField]
        private Image iconImage;

        [SerializeField] private Image colorImage;
        private List<Color> _colors;
        private Vector3 _posRecord;

        [Title("Constant")] private List<Sprite> _sprites;

        /*public void Init(Transform worldTransform, List<Sprite> sprites, List<Color> colors, MapUIController controller)
        {
            var position = worldTransform.position;
            _posRecord = position;
            _sprites = sprites;
            _colors = colors;
            GetComponent<RectTransform>().anchoredPosition3D = controller.TransferToAnchoredPos(position);
            //reposition basic sprite and color
            iconImage.sprite = _sprites[selectedSpriteIndex];
            colorImage.color = _colors[selectedColorIndex];
        }

        public MapDot Confirm()
        {
            var dot = new MapDot();
            dot.image = _sprites[selectedSpriteIndex];
            dot.color = _colors[selectedColorIndex];
            dot.pos = _posRecord;
            return dot;
        }

        public void MoveSelection(Vector2Int dir)
        {
            if (dir.y != 0) selector = (selector + 1) % 2;
            if (selector == 0)
            {
                selectedSpriteIndex = (selectedSpriteIndex + dir.x + _sprites.Count) % _sprites.Count;
                iconImage.sprite = _sprites[selectedSpriteIndex];
            }
            else
            {
                selectedColorIndex = (selectedColorIndex + dir.x + _colors.Count) % _colors.Count;
                colorImage.color = _colors[selectedColorIndex];
            }
        }*/
    }
}