using System.Collections;
using System.Linq;
using Barterta.InputTrigger;
using Barterta.Island.MONO;
using Barterta.Mark;
using Barterta.PointArrow;
using UnityEngine;

[RequireComponent(typeof(PointArrowController))]
public class BoatRadar : NothingComponent
{
    [SerializeField] private RadarType type;
    private PointArrowController _pointArrowController;
    private Mark currentMark;

    private void Start()
    {
        _pointArrowController = GetComponent<PointArrowController>();
        StartCoroutine(Refresh());
    }

    public override Vector3 ProduceForceVector(Vector3 nowForceVector)
    {
        return Vector3.zero;
    }

    public override void OnHitIsland(Island island)
    {
    }

    protected override bool CanInteractOnBoat(bool isLong, GrabTrigger trigger)
    {
        return false;
    }

    protected override void OnInteractOnBoat(GrabTrigger trigger)
    {
    }
    
    IEnumerator Refresh()
    {
        while (true)
        {
            TryLoad();
            yield return new WaitForSeconds(10f);
        }
    }

    private void TryLoad()
    {
        var container = Resources.Load<MarkContainer>(type == RadarType.Island ? "IslandMarkContainer" : "FishPointContainer");
        //Find the closest mark in marklist by distance
        var mark = container.markList.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
        if (currentMark != mark)
        {
            currentMark = mark;
            _pointArrowController.CleanAllPointers();
            _pointArrowController.AddPointer(mark.transform, "", Color.white,1.5f,1f);   
        }
    }
    
    public enum RadarType
    {
        Island,
        FishPoint
    }
}