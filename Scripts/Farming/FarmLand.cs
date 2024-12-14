using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using UnityEngine;

namespace Barterta.Farming
{
    public class FarmLand : MonoBehaviour, IPutGroundableOn
    {
        private GroundBlock _gb;

        public bool JudgePut(Groundable groundable)
        {
            //Debug.Log("Start Judge");
            return groundable.GetComponent<SeedBox>() && _gb.groundablesOn.Count == 1;
        }

        public void OnJudgePut(bool judge, Groundable groundable, GrabTrigger trigger = null)
        {
        }

        public void EffectBeforeSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
        }

        public void EffectAfterSetOn(Groundable groundable, GrabTrigger trigger = null)
        {
            groundable.GetComponent<SeedBox>().PlantSeed(_gb);
            if (trigger) trigger.skillContainer.AddXPTo("Farming", 5);
        }

        public void Init()
        {
            _gb = GetComponent<Groundable>().blockUnder;
            var coord = _gb.coordinate;
            var controller = _gb.island;
            //if surround block has at least one empty
            /*isFertile = controller.GetSurroundBlocksDir8(coord).Count < 8;
            if (!isFertile) Invoke(nameof(DestroySelf), Random.Range(returnTimeRange.x, returnTimeRange.y));*/
        }

        public void DestroySelf()
        {
            _gb.DestoryAll();
        }
    }
}