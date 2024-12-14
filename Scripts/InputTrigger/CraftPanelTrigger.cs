using Barterta.InputTrigger;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftPanelTrigger : InputTriggerBase
{
    [SerializeField] private CraftPanelUI craftPanelUI;
    [SerializeField] private float craftPanelUIUIOffset = 700;

    private Tween _panelTween;
    private bool _panelOn;

    protected override void Awake()
    {
        base.Awake();
        craftPanelUI.Init(GetComponent<CraftRecipeContainer>());
    }

    public void OnCraftPanelKey(InputAction.CallbackContext ctx)
    {
        if (_panelTween != null && _panelTween.IsActive()) _panelTween.Complete();

        _panelOn = !_panelOn;

        if (_panelOn)
        {
            //StateController.ChangeInteractState(this);
            _panelTween = craftPanelUI.GetComponent<RectTransform>()
                .DOAnchorPosX(craftPanelUI.GetComponent<RectTransform>().anchoredPosition.x + craftPanelUIUIOffset,
                    0.5f);
            craftPanelUI.Refresh();
        }
        else
        {
            //StateController.ChangeToDefault();    
            _panelTween = craftPanelUI.GetComponent<RectTransform>()
                .DOAnchorPosX(craftPanelUI.GetComponent<RectTransform>().anchoredPosition.x - craftPanelUIUIOffset,
                    0.5f);
        }
    }

    public override void OnInteractPress(InputAction.CallbackContext ctx)
    {
    }

    public override void OnShortGrab(InputAction.CallbackContext ctx)
    {
    }

    public override void OnLongGrab(InputAction.CallbackContext ctx)
    {
    }

    public override void OnInteractCancel(InputAction.CallbackContext ctx)
    {
    }

    public override void OnShortInteract(InputAction.CallbackContext ctx)
    {
    }

    public override void OnLongInteract(InputAction.CallbackContext ctx)
    {
    }

    public override void OnDirection(InputAction.CallbackContext ctx)
    {
    }

    public override void EmergentExit()
    {
    }
}