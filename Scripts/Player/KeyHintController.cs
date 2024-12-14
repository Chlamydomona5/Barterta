using System;
using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class KeyHintController : SerializedMonoBehaviour
{
    private GrabTrigger _grabTrigger;
    private Image _hintImage;
    private HintType _currentHintType;
    [SerializeField] private Dictionary<HintType, Sprite> _hintTypeToSprite;

    private bool _hintGround;

    private bool HintGround
    {
        get => _hintGround;
        set
        {
            _hintGround = value;
            if(!IsHinting) HideHintUI();
        }
    }
    private bool _hintHand;
    private bool HintHand
    {
        get => _hintHand;
        set
        {
            _hintHand = value;
            if(!IsHinting) HideHintUI();
        }
    }
    private bool IsHinting => _hintGround || _hintHand;

    private void Awake()
    {
        _grabTrigger = GetComponent<GrabTrigger>();
        _hintImage = WorldUIManager.I.GenerateUI(Resources.Load<UIObject>("UI/World/KeyHintUI"), transform).GetComponent<Image>();
        _hintImage.GetComponent<FollowObject>().offset = new Vector3(2.5f, .5f, 0);
    }

    public void ShowHintTo(GroundBlock block)
    {
        if (!block || block.groundablesOn.Count == 0)
        {
            HintGround = false;
            return;
        }
        var target = block.groundablesOn[0].gameObject;
        HintGround = TryShowHintToTarget(target);
    }

    public void ShowHintTo(GameObject entity)
    {
        HintGround = TryShowHintToTarget(entity);
    }

    public void ShowHintTo(List<Groundable> holdingItem)
    {
        if(holdingItem.Count == 0)
        {
            HintHand = false;
            return;
        }
        HintHand = TryShowHintToHand(holdingItem[0]);
    }

    private bool TryShowHintToTarget(GameObject target)
    {
        //Judge interactable
        var interact = target.GetComponent<IInteractOnGround>();
        if (interact != null)
        {
            if ((interact.Judge(true, _grabTrigger) || interact.Judge(false, _grabTrigger)))
            {
                ShowHintUI(HintType.Interact);
                return true;
            }
        }
        //Judge put on
        var putGroundableOn = target.GetComponent<IPutGroundableOn>();
        if (putGroundableOn != null) 
        {
            if ((_grabTrigger.HandBlock.groundablesOn.Count > 0 &&
                 putGroundableOn.JudgePut(_grabTrigger.HandBlock.groundablesOn[0])))
            {
                ShowHintUI(HintType.Grab);
                return true;
            }
        }
        //Judge consume groundable
        var consumeGroundable = target.GetComponent<IConsumeGroundable>();
        if (consumeGroundable != null)
        {
            if (_grabTrigger.HandBlock.groundablesOn.Count > 0 && consumeGroundable.JudgeConsume(_grabTrigger.HandBlock.groundablesOn[0]))
            {
                ShowHintUI(HintType.Grab);
                return true;
            }
        }
        return false;
    }
    
    private bool TryShowHintToHand(Groundable target)
    {
        //Judge interactable
        var interact = target.GetComponent<IInteractOnHand>();
        if (interact != null)
        {
            if ((interact.Judge(true, _grabTrigger) || interact.Judge(false, _grabTrigger)))
            {
                ShowHintUI(HintType.Interact);
                return true;
            }
        }
        //Judge PutInWater
        var putInWater = target;
        if (putInWater != null)
        {
            if (putInWater.CanPutInWater(_grabTrigger))
            {
                ShowHintUI(HintType.Grab);
                return true;
            }
        }

        return false;
    }

    private void ShowHintUI(HintType hintType)
    {
        _currentHintType = hintType;
        _hintImage.gameObject.SetActive(true);
        _hintImage.color = Color.clear;
        _hintImage.DOColor(Color.white, .2f);
        _hintImage.sprite = _hintTypeToSprite[hintType];
    }

    private void HideHintUI(HintType hintType)
    {
        if (_currentHintType == hintType)
            _hintImage.gameObject.SetActive(false);
    }
    
    private void HideHintUI()
    {
        _hintImage.DOColor(Color.clear, .2f);
    }
}

public enum HintType
{
    Interact,
    Grab,
}