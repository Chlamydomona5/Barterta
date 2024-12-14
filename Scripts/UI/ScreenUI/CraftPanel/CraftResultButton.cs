using UnityEngine;

public class CraftResultButton : ResultButton
{
    public override void OnClick()
    {
        //log
        Debug.Log("CraftResultButton: " + _id);
        _craftPanelUI.SetCraftTable(_id);
        _craftPanelUI.ChangeCurrent(this);
    }

    public override void Select(bool on)
    {
        selectImage.gameObject.SetActive(on);
    }
}