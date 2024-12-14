using Barterta.Core.KeyInterface;
using Barterta.ItemGrid;
using Barterta.PointArrow;
using UnityEngine;

[RequireComponent(typeof(PointArrowController))]
public class PairBeacon : Groundable, IBeSettled
{
     public GameObject originalMesh;
     public GameObject anotherMesh;
    private PointArrowController _pointArrowController;
    public PairBeacon another;
    protected override void Awake()
    {
        _pointArrowController = GetComponent<PointArrowController>();
        base.Awake();
    }
    
    public void OnSettled(GroundBlock block)
    {
        if (!another)
        {
            var color = new Color(Random.value, Random.value, Random.value);

            var newBlock = block.island.GetRandomSurroundStackableBlock(block.coordinate);

            var partner = Instantiate(gameObject);
            partner.GetComponent<PairBeacon>().another = this;

            partner.GetComponent<Groundable>().SetOn(newBlock);

            another = partner.GetComponent<PairBeacon>();
            another.originalMesh.SetActive(false);
            another.anotherMesh.SetActive(true);
        }
        _pointArrowController.AddPointer(another.transform, "", Color.white, 1.5f, 1f);
    }
}