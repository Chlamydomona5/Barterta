using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PressureEventSlotUI : SerializedMonoBehaviour
{
    public Image baseEventIcon;
    
    //Default and storm
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite stormSprite;
    
    //Monster
    [SerializeField] private Image monsterImage;
    
    public void ResetEventIcon()
    {
        baseEventIcon.sprite = defaultSprite;
        monsterImage.gameObject.SetActive(false);
    }
    
    public void SetEventIcon(PressureEventType type)
    {
        switch (type)
        {
            case PressureEventType.Storm:
                baseEventIcon.sprite = stormSprite;
                break;
            case PressureEventType.MonsterRaid:
                monsterImage.gameObject.SetActive(true);
                break;
        }
    }
}