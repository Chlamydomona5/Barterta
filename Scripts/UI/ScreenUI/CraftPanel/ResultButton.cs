using Barterta.ItemGrid;
using Barterta.UI.UIManage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ResultButton : MonoBehaviour
{
    protected string _id;
    protected CraftPanelUI _craftPanelUI;
    [SerializeField] protected TextMeshProUGUI countText;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected Image selectImage;

    public virtual void Init(GroundableWithCount res, IconInfo iconInfo, CraftPanelUI craftPanelUI)
    {
        _id = res.groundable.ID;
        //If not 1, show the count, else show nothing
        countText.text = res.count > 1 ? res.count.ToString() : "";
        
        _craftPanelUI = craftPanelUI;
        iconImage.sprite = iconInfo.GetIcon(_id);
    }

    public abstract void OnClick();

    public abstract void Select(bool on);
}