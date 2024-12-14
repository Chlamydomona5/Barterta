using Barterta.Tool;
using EPOOutline;
using Sirenix.OdinInspector;
using UnityEngine;

public class Durable : NotHold
{
    public bool infiniteDurability;
    [HideIf("infiniteDurability")]
    [ReadOnly] public int durability;
    [HideIf("infiniteDurability")]
    public int maxDurability;
    private Outlinable _mask;


    protected override void Awake()
    {        
        durability = maxDurability;
        base.Awake();
    }
    
    protected virtual void Start()
    {
        _mask = gameObject.AddComponent<Outlinable>();
        _mask.RenderStyle = RenderStyle.FrontBack;
        _mask.BackParameters.Enabled = false;
        _mask.FrontParameters.Color = Color.clear;
        _mask.FrontParameters.FillPass.Shader =
            Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
        _mask.FrontParameters.FillPass.SetColor("_PublicColor", new Color(1, 0, 0, .0f));
        _mask.AddAllChildRenderersToRenderingList();
    }
    
    protected void DurabilityChange(int num)
    {
        if(infiniteDurability) return;
        durability = Mathf.Clamp(durability + num, 0, maxDurability);
        if (durability == 0)
        {
            BeRemovedFromNowBlock();
            Destroy(gameObject);
        }
        //change color accord to 3 stages
        var remain = (float)durability / maxDurability;
        var transparency = 0f;
        //Increase
        if (remain > .8f) transparency = 0f;
        else if (remain > .5f) transparency = .1f;
        else if (remain > .15f) transparency = .3f;
        else transparency = .15f;

        _mask.FrontParameters.FillPass.SetColor("_PublicColor", new Color(1, 0, 0, transparency));
    }

    public void Repair()
    {
        //Big number
        DurabilityChange(500);
    }
}