using Barterta.Core;
using Barterta.Facility;
using DG.Tweening;
using UnityEngine;

public class WeatherTower : FacilityEntity
{
    [SerializeField] private Transform rotateTransform;
    public override void Start()
    {
        base.Start();
        HomeManager.I.SetPressureEventUI(true);
        //Cirlce rotate infinitely using DOTween
        rotateTransform.DORotate(new Vector3(-90, 180, 0), 20f).SetLoops(-1);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        HomeManager.I.SetPressureEventUI(false);
    }
}