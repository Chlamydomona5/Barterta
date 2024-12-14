using System;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Player;
using UnityEngine;

namespace Barterta.Facility
{
    public class FacilityBlueprint : Groundable, IPressInteractOnHandEffector, IMoveToHand
    {
        public FacilityRecipe targetRecipe;
        public int size => targetRecipe.TargetFacility.Size;

        [SerializeField] private FacilityEntity constructionSitePrefab;
        [SerializeField] private FacilityPlaceholder placeHolderPrefab;
        
        private GrabTrigger _trigger;

        private void Start()
        {
            //Instantiate FacilityEntity's model on self
            var model = Instantiate(targetRecipe.TargetFacility, transform, false).gameObject;
            model.transform.localPosition = Vector3.up * .05f;
            model.transform.localScale = Vector3.one / 6f;
            
            model.layer = LayerMask.NameToLayer("Default");
            foreach (Transform child in model.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            Destroy(model.GetComponent<FacilityEntity>());
        }

        public bool CanPutOn(GroundBlock blockToPut,GrabTrigger trigger)
        {
            if(!blockToPut) return false;
            var island = blockToPut.island;
            if (!island) return false;
            //Test if can put on
            var canGo = true;
            for (int x = -(size / 2); x <= size / 2; x++)
            {
                for (int y = -(size / 2); y <= size / 2; y++)
                {
                    //if block don't exist or has groundable on, return false
                    var tarBlock = island.TestBlockExist(blockToPut.coordinate + new Vector2Int(x, y));
                    //If block not exist
                    if (!tarBlock) canGo = false;
                    //If block has groundable on
                    var islandBlock = island.BlockMap[blockToPut.coordinate.x + x, blockToPut.coordinate.y + y];
                    if (islandBlock && islandBlock.groundablesOn.Count > 0)
                        canGo = false;
                }
            }
            
            if (!canGo)
            {
                return false;
            }
            
            return true;
        }

        private void PutOn(GroundBlock blockToPut)
        {
            var island = blockToPut.island;
            //If can, put on every block
            var site = Instantiate(constructionSitePrefab, blockToPut.island.transform, false);
            site.transform.position = blockToPut.transform.position;
            //Put on
            for (int x = -(size / 2); x <= size / 2; x++)
            {
                for (int y = -(size / 2); y <= size / 2; y++)
                {
                    var tarBlock = island.BlockMap[blockToPut.coordinate.x + x, blockToPut.coordinate.y + y];
                    //Instantiate placeholder
                    var placeHolder = Instantiate(placeHolderPrefab);
                    placeHolder.SetOn(tarBlock);
                    site.placeHolders.Add(placeHolder);
                }
            }
            site.Init(targetRecipe, blockToPut.coordinate);

            //Destroy this
            BeRemovedFromNowBlock();
            Destroy(gameObject);
        }

        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return CanPutOn(trigger.GetComponent<GridDetector>().planeCenter, trigger);
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            if (Judge(isLong, trigger))
            {
                PutOn(trigger.GetComponent<GridDetector>().planeCenter);
            }
            else
                trigger.GetComponent<DialogTrigger>().SelfBark("noenoughemptyblocks");
        }

        public void OnMove(bool isToHand, GrabTrigger trigger, GroundBlock block)
        {
            //Debug.Log("Bluprint OnMove " + isToHand);
            var gridDetector = trigger.GetComponent<GridDetector>();
            if (isToHand)
            {
                gridDetector.DetectWarning(true, this);
                gridDetector.DetectRange = targetRecipe.TargetFacility.Size / 2 + 1;
            }
            else
            {
                gridDetector.DetectWarning(false, this);
                gridDetector.DetectRange = 1;
            }

            if (isToHand) _trigger = trigger;
        }

        private void OnDestroy()
        {
            if (_trigger)
            {
                var gridDetector = _trigger.GetComponent<GridDetector>();
                gridDetector.DetectWarning(false, this);
                gridDetector.DetectRange = 1;
            }
        }
    }
}