using System.Collections.Generic;
using Barterta.ToolScripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Barterta.UI.UIManage
{
    public class StartMenuController : MonoBehaviour
    {
        private void Start()
        {
            //Text Fade Loop
            GetComponentInChildren<TextMeshProUGUI>().DOFade(.3f, 4f).SetLoops(-1,LoopType.Yoyo);
        }

        public void StartGame(InputAction.CallbackContext ctx)
        {
            SceneManager.LoadScene(1);
        }
    }
}