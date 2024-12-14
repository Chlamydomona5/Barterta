using System.Collections;
using UnityEngine;

namespace Barterta.UI.UIManage
{
    public class UILayerAdjuster : MonoBehaviour
    {
        [SerializeField] private Mark.Mark mark;
        private IEnumerator Start()
        {
            yield return 1;
            var cam = GetComponent<Camera>();
            var newMask = (1 << LayerMask.NameToLayer("Player" + mark.id));
            cam.cullingMask |= newMask;
        }
    }
}