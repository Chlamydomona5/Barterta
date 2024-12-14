using UnityEngine;

public class KilnResultButton : ResultButton
{
    public override void OnClick()
    {
        _craftPanelUI.SetKilnRecipe(_id);
        _craftPanelUI.ChangeCurrent(this);
    }

    public override void Select(bool on)
    {
        selectImage.gameObject.SetActive(on);
    }
}