using Barterta.NaturalResouce;
using Barterta.UI.WorldUI;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterDistillaterUI : UIObject
{
    [SerializeField] private ProgressBarUI process;
    [SerializeField] private Transform seaWaterRoot;
    [SerializeField] private GameObject seaWaterPrefab;
    [SerializeField] private Transform pureWaterRoot;
    [SerializeField] private GameObject pureWaterPrefab;

    public void ChangeProcess(float percentage)
    {
        process.ChangeTo(percentage);
    }
    
    public void ChangeWater(int count, WaterType type)
    {
        var root = type == WaterType.SeaWater ? seaWaterRoot : pureWaterRoot;
        var prefab = type == WaterType.SeaWater ? seaWaterPrefab : pureWaterPrefab;
        
        if (count > root.childCount)
        {
            for (int i = 0; i < count - root.childCount; i++)
            {
                Instantiate(prefab, root, false);
            }
        }
        else if (count < root.childCount)
        {
            for (int i = 0; i < root.childCount - count; i++)
            {
                Destroy(root.GetChild(i).gameObject);
            }
        }
    }
}