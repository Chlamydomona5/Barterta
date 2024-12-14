using System;
using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Tool;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BackpackTrigger : SerializedMonoBehaviour
{
    [SerializeField] private int originSlotCount = 3;

    private int _currentIndex;
    private BackpackSlot CurrentSlot => slots[_currentIndex];
    public GroundBlock CurrentBlock => CurrentSlot.block;
    public bool canSwitch = true;
    
    [SerializeField, ReadOnly] private List<BackpackSlot> slots = new();

    [SerializeField] private Transform backpackBlockRoot;
    [SerializeField] private BackpackSlot slotPrefab;
    [SerializeField] private Transform backpackUIRoot;
    [SerializeField] private Transform holdRoot;

    private GrabTrigger _grabTrigger;

    private void Awake()
    {
        _grabTrigger = GetComponent<GrabTrigger>();
        
        //Add origin slot
        for (int i = 0; i < originSlotCount; i++) AddNewSlot();

        var list = new List<string>();
        if (Constant.OnTestComponent == false)
        { 
            list = new List<string>()
            { 
                "",
                "Utensil/CraftHammer",
                "Utensil/HookNet",
                "NaturalResource/WaterBowl"
            };
        }
        else
        {
            list = new List<string>()
            {
                "Utensil/CraftHammer",
                "Utensil/FishingRod_Wood",
                "Weapon/IronSword",
                "Utensil/StonePickaxe"
            };
        }

        
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i] == "") continue;
            var groundable = Resources.Load<Groundable>(list[i]);
            var instance = Instantiate(groundable);
            instance.SetOn(slots[i].block);
            UpdateUI();
        }
    }

    private void Start()
    {
        ChangeCurrent(0);
    }

    private void AddNewSlot()
    {
        //Ins block
        var block = new GameObject("BackpackBlock", typeof(GroundBlock));
        block.transform.SetParent(backpackBlockRoot);
        block.transform.localPosition = Vector3.zero;

        block.tag = "Player";
        //Ins Slot
        var slot = Instantiate(slotPrefab, backpackUIRoot);
        slots.Add(slot);
        slot.Init(block.GetComponent<GroundBlock>(), slots.IndexOf(slot) + 1);

    }
    
    private void ChangeCurrent(int newIndex)
    {
        if (newIndex >= slots.Count) return;
        CurrentSlot.BeSelected(false);
        //transform change
        CurrentBlock.transform.SetParent(backpackUIRoot.transform, false);
        //model
        if (CurrentBlock.groundablesOn.Count > 0)
        {
            var groundable = CurrentBlock.groundablesOn[0];
            if (groundable.hasDiffModel)
                groundable.SwitchModelTo(false);
            if(groundable.GetComponent<IMoveToHand>() != null) 
                groundable.GetComponent<IMoveToHand>().OnMove(false, _grabTrigger, null);
        }

        _currentIndex = newIndex;
        
        CurrentSlot.BeSelected(true);
        //transform change
        CurrentBlock.transform.SetParent(holdRoot, false);
        //model
        if (CurrentBlock.groundablesOn.Count > 0)
        {
            var groundable = CurrentBlock.groundablesOn[0];
            if (groundable.hasDiffModel)
                groundable.SwitchModelTo(true);
            if (groundable.GetComponent<NotHold>())
                _grabTrigger.AdjustHoldPosition();
            if(groundable.GetComponent<IMoveToHand>() != null) 
                groundable.GetComponent<IMoveToHand>().OnMove(true, _grabTrigger, null);
        }
        
        _grabTrigger.CheckAnimChange();
    }
    
    public void OnBackpackKey(InputAction.CallbackContext ctx)
    {
        if (canSwitch)
        {
            int indexChange;
            indexChange = (int)Mathf.Sign(ctx.ReadValue<float>());
            ChangeCurrent((_currentIndex + indexChange + slots.Count) % slots.Count);   
        }
    }
    
    public void OnBackpackNumKey(InputAction.CallbackContext ctx)
    {
        if (canSwitch)
        {
            int indexChange;
            indexChange = (int)ctx.ReadValue<float>();
            ChangeCurrent(indexChange - 1);   
        }
    }

    public void UpdateUI()
    {
        foreach (var slot in slots)
        {
            slot.UpdateUI();
        }
    }
    
    
}