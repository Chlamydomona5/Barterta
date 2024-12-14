using Barterta.Craft;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Player;
using Barterta.Sound;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Tool
{
    public class CraftHammer : Tool
    {
        private GrabTrigger _trigger;
        private CraftValidater _validater;
        private CraftRecipeContainer _craftRecipeContainer;
        
        protected override void Start()
        {
            _validater = Resources.Load<CraftValidater>("Crafting/CraftValidater");
            _craftRecipeContainer = FindObjectOfType<CraftRecipeContainer>();
        }

        public override void OnMove(bool isToHand, GrabTrigger trigger, GroundBlock block)
        {
            //Debug.Log("CraftHammer Move to hand " + isToHand);
            base.OnMove(isToHand, trigger, block);
            _trigger = trigger;
            if (isToHand)
            {
                trigger.GetComponent<GridDetector>().DetectRange = 2;
            }
            else
            {
                trigger.GetComponent<GridDetector>().DetectRange = 1;
            }
        }

        private void OnDestroy()
        {
            if (_trigger) _trigger.GetComponent<GridDetector>().DetectRange = 1;
        }

        public override bool Judge(bool isLong, GrabTrigger trigger)
        {
            return false;
        }

        public override bool Effect(GroundBlock target, ToolTrigger trigger)
        {
            HammerEffect();
            return false;
        }

        private void HammerEffect()
        {
            var block = _trigger.GetComponent<GridDetector>().planeCenter;
            if (block)
            {
                var matrix = new GroundBlock[3, 3];
                foreach (var dir in Constant.Direction.Dir8PlusCenter)
                {
                    var coord = new Vector2Int(block.coordinate.x + dir.x, block.coordinate.y + dir.y);
                    //if null block or out range, don't test
                    if (!block.island.TestBlockInRange(coord)) return;
                    matrix[dir.x + 1, dir.y + 1] =
                        block.island.BlockMap[coord.x, coord.y];
                }

                //Destory material
                var result = _validater.GetRecipeResult(matrix);
                if (result != null)
                {
                    foreach (var block1 in matrix) if(block1) block1.DestoryAll();
                    //Result
                    for (int i = 0; i < result.count; i++)
                    {
                        Instantiate(result.groundable).SetOn(block);
                    }
                    //Sound
                    SoundManager.I.PlaySound("Hammer");
                    //Unlock
                    _craftRecipeContainer.UnlockRecipeByMaterial(result.groundable.ID);
                    //Play Particle
                    _trigger.GetComponent<PlayerParticleController>().PlayParticleAt("CraftSmoke",
                        block.transform.position + Vector3.up * 0.5f, new Vector3(0, 0, 0));
                }
                
            }
        }
    }
}