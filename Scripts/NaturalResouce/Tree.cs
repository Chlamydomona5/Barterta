using System.Collections.Generic;
using Barterta.ItemGrid;
using Barterta.Sound;
using DG.Tweening;
using UnityEngine;

namespace Barterta.NaturalResouce
{
    public class Tree : NaturalResource
    {
        protected override void DestorySelf()
        {
            SoundManager.I.PlaySound(destroySound);
            
            var groundable = GetComponent<Groundable>();
            groundable.BeRemovedFromNowBlock();

            if (afterDestory) Instantiate(afterDestory).SetOn(GetComponent<Groundable>().blockUnder);
            
            var model = GetComponentInChildren<MeshRenderer>();
            model.transform.DORotate(new Vector3(Random.value > .5f ? 0f : 180f, 0, 0), 2f).SetEase(Ease.InExpo)
                .OnComplete(
                    delegate { DestroyDrop(); });
        }
    }
}