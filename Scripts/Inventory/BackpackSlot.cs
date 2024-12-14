using System.Collections.Generic;
using Barterta.ItemGrid;
using Barterta.Rarity;
using Barterta.Tool;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackpackSlot : SerializedMonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private ProgressBarUI toolDurabilityBar;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image selectionImage;
    [SerializeField] private TextMeshProUGUI listCountText;
    [SerializeField] private Dictionary<Rarity, Image> rarityImages;
    public GroundBlock block;

    private IconInfo _icons;

    public void Init(GroundBlock blockInit, int count)
    {
        block = blockInit;
        _icons = Resources.Load<IconInfo>("IconInfo");
        listCountText.text = count.ToString();
        UpdateUI();
    }

    public void BeSelected(bool isSelected)
    {
        selectionImage.gameObject.SetActive(isSelected);
    }

    public void UpdateUI()
    {
        if (block.groundablesOn.Count > 0)
        {
            countText.text = block.groundablesOn.Count.ToString();
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = _icons.GetIcon(block.groundablesOn[0].ID);
            //Tool durability UI update
            if (block.groundablesOn[0] is Durable)
            {
                var tool = (block.groundablesOn[0] as Durable);
                if (tool && !tool.infiniteDurability)
                {
                    toolDurabilityBar.gameObject.SetActive(true);
                    toolDurabilityBar.ChangeTo((float)tool.durability / tool.maxDurability);
                }
                else toolDurabilityBar.gameObject.SetActive(false);
            }
            else toolDurabilityBar.gameObject.SetActive(false);
            //Item Rarity UI update
            if (block.groundablesOn[0].GetComponent<IRarity>() != null)
            {
                var rarity = block.groundablesOn[0].GetComponent<IRarity>();
                foreach (var rarityImage in rarityImages)
                {
                    rarityImage.Value.gameObject.SetActive(false);
                }
                rarityImages[rarity.Rarity].gameObject.SetActive(true);
            }
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            countText.text = "";
            
            toolDurabilityBar.gameObject.SetActive(false);
            
            foreach (var rarityImage in rarityImages)
            {
                rarityImage.Value.gameObject.SetActive(false);
            }
        }
    }
}