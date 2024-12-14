using Barterta.InputTrigger;
using Barterta.Interactable;
using Barterta.ItemGrid;
using Barterta.NaturalResouce;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterDistillater : Processor
{
    [SerializeField] public int pureWaterCapacity = 3;
    [SerializeField] public int seaWaterCapacity = 3;
    
    private int _pureWaterCount;
    public int PureWaterCount
    {
        get => _pureWaterCount;
        set
        {
            _pureWaterCount = Mathf.Clamp(value, 0, pureWaterCapacity);
            ((WaterDistillaterUI)_playerStayUI.uiInstance).ChangeWater(_pureWaterCount, WaterType.PureWater);
        }
    }
    
    private int _seaWaterCount;
    public int SeaWaterCount
    {
        get => _seaWaterCount;
        set
        {
            _seaWaterCount = Mathf.Clamp(value, 0, seaWaterCapacity);
            ((WaterDistillaterUI)_playerStayUI.uiInstance).ChangeWater(_seaWaterCount, WaterType.SeaWater);
        }
    }
    
    [SerializeField] private ParticleSystem particle;
    
    protected override bool Judge(Groundable groundable, GrabTrigger trigger = null)
    {
        return false;
    }

    public override void OnJudgeConsume(bool judge, Groundable groundable, GrabTrigger trigger = null)
    {
    }

    protected override Groundable GetResult(Groundable putIn)
    {
        return null;
    }

    protected override void ResultProcess(Groundable resultInstance)
    {

    }

    protected override void OnProcessEnd()
    {
        base.OnProcessEnd();
        SeaWaterCount--;
        PureWaterCount++;
        particle.gameObject.SetActive(false);
        TryProcess();
    }

    public bool TryContainer(WaterContainer container)
    {
        //If container is SeaWater, try to add it to seaWaterCount
        if (container.WaterType == WaterType.SeaWater)
        {
            if (SeaWaterCount < seaWaterCapacity)
            {
                SeaWaterCount += container.WaterCount;
                container.WaterType = WaterType.Empty;
            }
        }
        //If container is empty, try to take out pure water
        else if (container.WaterType == WaterType.Empty)
        {
            if (PureWaterCount >= container.WaterCount)
            {
                PureWaterCount -= container.WaterCount;
                container.WaterType = WaterType.PureWater;
            }
        }
        //Test
        return TryProcess();
    }
    
    private bool TryProcess()
    {
        if (SeaWaterCount > 0 && PureWaterCount < pureWaterCapacity)
        {
            //If there is sea water and pure water is not full, process
            TryStartProcess(null);
            particle.gameObject.SetActive(true);
            return true;
        }
        return false;
    }
}