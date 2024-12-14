using System.Collections;
using Barterta.Mark;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace Barterta.UI.UIManage
{
    public class CameraViewportAdjuster : MonoBehaviour
    {
        public void Adjust()
        {
            StartCoroutine(WaitAndAdjust());
        }

        private IEnumerator WaitAndAdjust()
        {
            yield return null;
            foreach (var sign in Resources.Load<MarkContainer>("PlayerContainer").markList)
            {
                var cam = sign.GetComponent<PlayerInput>().camera;
                var overlay = cam.GetComponent<UniversalAdditionalCameraData>().cameraStack[0];
                overlay.rect = cam.rect;
            }
        }
    }
}