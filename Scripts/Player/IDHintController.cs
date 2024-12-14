using System;
using Barterta.ItemGrid;
using Barterta.Player;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using TMPro;
using UnityEngine;

public class IDHintController : MonoBehaviour
{
    private MouseHandler _mouseHandler;
    private UIObject _idHint;
    private TextMeshProUGUI _idText;
    private bool _onShow;
    private Groundable _currentGroundable;

    private void Start()
    {
        _mouseHandler = GetComponent<MouseHandler>();
        _idHint = WorldUIManager.I.GenerateUI(Resources.Load<UIObject>("UI/World/IDHintUI"), Vector3.zero, 0f);
        _idText = _idHint.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        var block = _mouseHandler.GetBlockOnMouse();
        if (block && block.groundablesOn.Count > 0 && _currentGroundable != block.groundablesOn[0])
        {
            _onShow = true;
            _currentGroundable = block.groundablesOn[0];
            var idToShow = _currentGroundable.LocalizeName;
            WorldUIManager.I.AppealUI(_idHint);
            
            _idText.text = idToShow;
            _idHint.transform.position = block.transform.position + new Vector3(0, 1.5f, 0);
        }
        
        
        if(_onShow && (!block || block.groundablesOn.Count == 0))
        {
            _currentGroundable = null;
            _onShow = false;
            WorldUIManager.I.HideUI(_idHint);
        }
    }
}