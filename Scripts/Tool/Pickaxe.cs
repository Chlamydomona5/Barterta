using Barterta.Farming;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.NaturalResouce;
using Barterta.Sound;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Tool
{
    public class Pickaxe : Tool
    {
        [Title("Pickaxe Part")]
        [SerializeField] private int damageConstant;

        public override bool Judge(bool isLong, GrabTrigger trigger)
        {
            var target = trigger.detector.targetBlock;
            if (!target) return false;
            return target.groundablesOn.Count > 0 && target.groundablesOn[0].GetComponent<NaturalResource>();
        }

        public override bool Effect(GroundBlock target, ToolTrigger trigger)
        {
            if (!target) return false;

            /*if (target.groundablesOn.Count == 0 && boostingStage > 0)
            {
                var farmland = Instantiate(farmLand);
                farmland.SetOn(target);
                SoundManager.I.PlaySound("Farm");
                farmland.GetComponent<FarmLand>().Init();
                BoostCost();h
                return false;
            }
            //Destroy Farm land
            else if (target.groundablesOn.Count > 0 && target.groundablesOn[0].GetComponent<FarmLand>())
            {
                target.DestoryAll();
                return false;
            }
            else*/ if (target.groundablesOn.Count > 0 && target.groundablesOn[0].GetComponent<NaturalResource>())
            {
                bool go = !target.groundablesOn[0].GetComponent<Weed>();
                
                var resource = target.groundablesOn[0].GetComponent<NaturalResource>();
                resource.TakeDamage(damageConstant);
                //XP
                trigger.skillContainer.AddXPTo("Foraging", damageConstant);
                //Should be last
                BoostCost();
               
                trigger.GetComponent<PlayerParticleController>().PlayParticleAt("PickaxeHit", transform.position + trigger.transform.forward * .2f + Vector3.up * .3f, Vector3.zero);
                return go;
            }

            return false;
        }
    }
}