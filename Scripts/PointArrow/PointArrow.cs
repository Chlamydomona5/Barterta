using Barterta.UI.WorldUI;
using UnityEngine;

namespace Barterta.PointArrow
{
    public class PointArrow : MonoBehaviour
    {
        private PointArrowController _controller;
        public float distance;
        public PointArrowCommentUI commentUI;

        public void Init(PointArrowController controller,float xdistance, PointArrowCommentUI xcommentUI)
        {
            _controller = controller;
            this.distance = xdistance;
            this.commentUI = xcommentUI;
        }

        public void Destroy()
        {
            _controller.RemovePointer(this);
        }
    }
}