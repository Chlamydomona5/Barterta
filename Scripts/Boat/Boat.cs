using System.Collections.Generic;
using Barterta.Core;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Boat
{
    public class Boat : BlockSet
    {
        [SerializeField, ReadOnly] private List<BoatComponent> components = new();
        [SerializeField,ReadOnly] private Vector3 forceVector;
        [SerializeField, ReadOnly] private Island.MONO.Island boardIsland;

        [SerializeField, ReadOnly] public bool hasViewEnchancer;
    
        private bool _justUnload;
        private Rigidbody _rb;
        private bool _canMove;

        #region Interface

        public void Init(Island.MONO.Island island)
        {
            boardIsland = island;
        }
    
        /// <param name="island"></param>
        /// <param name="boatCoordinate">hit point's coordinate on boat</param>
        /// <param name="islandCoordinate">hit point's coordinate on island</param>
        public void HitIsland(Island.MONO.Island island, Vector2Int boatCoordinate, Vector2Int islandCoordinate)
        {
            //Log
            Debug.Log("Hit Island, boatCoordinate: " + boatCoordinate + " islandCoordinate: " + islandCoordinate);
            //Load In
            var diff = islandCoordinate - boatCoordinate;
            LoadInIsland(island, diff);
            //Interface
            foreach (var component in components)
            {
                component.OnHitIsland(island);
            }
            //Stop
            SetMoveActive(false);
        }

        public void LoadInIsland(Island.MONO.Island island, Vector2Int diffFactor)
        {
            foreach (var groundBlock in BlockMap.Map)
            {
                var block = (BoatBlock)groundBlock;
                if (block)
                {
                    //Load In island
                    island.AddBlock(block.boatCoordinate + diffFactor, block);
                }
            }
            boardIsland = island;
        }

        public void UnLoadIsland()
        {
            foreach (var block in BlockMap.Map)
            {
                if (block)
                {
                    boardIsland.RemoveBlock(block.coordinate);
                }
            }
            boardIsland = null;
            _justUnload = true;
        }
    
        public BoatBlock GetBlock(int x, int y)
        {
            return (BoatBlock)BlockMap[x, y];
        }
    
        public BoatBlock GetBlock(Vector2Int coordinate)
        {
            return (BoatBlock)BlockMap[coordinate];
        }

        public void AddBlock(Vector2Int coordninate, BoatBlock block)
        {
            if(BlockMap[coordninate]) return;
            BlockMap[coordninate.x, coordninate.y] = block;
            //Set parent
            block.transform.SetParent(transform);
        }
        
        public void RemoveBlock(Vector2Int coordninate)
        {
            if(!BlockMap[coordninate]) return;
            BlockMap[coordninate.x, coordninate.y] = null;
            //If no block left, destroy
            if (GetBlockCount() == 0)
            {
                Destroy(gameObject);
            }
        }
    
        public void AddComponent(BoatComponent component)
        {
            components.Add(component);
            Debug.Log(component.name + "Added To Boat");
        }
    
        public void RemoveComponent(BoatComponent component)
        {
            components.Remove(component);
            Debug.Log(component.name + "Removed from Boat");
        }

        public void SetMoveActive(bool active)
        {
            _rb.isKinematic = !active;
            _canMove = active;
        
            if(active && boardIsland) UnLoadIsland();
        }


        #endregion

        #region Move

        private void CalculateAddedForceVector()
        {
            forceVector = Vector3.zero;
            //Sort by priority from low to high
            components.Sort((a, b) => a.priority.CompareTo(b.priority));
        
            foreach (var boatComponent in components)
            {
                forceVector += boatComponent.ProduceForceVector(forceVector);
            }
        }

        #endregion

        #region UnityLogic

        private void Start()
        {
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            SetMoveActive(false);
        }

        private void FixedUpdate()
        {
            if(!_canMove) return;
        
            CalculateAddedForceVector();
        
            //TODO: May need improve effeciency
            Vector3 expectedVelocity = forceVector / Mathf.Sqrt(Mathf.Sqrt(GetBlockCount())) * Constant.Boat.expectedVelocityConstant;
            //Lerp velocity to expected velocity
            //Step infected by a factor of expected velocity * current difference
            var lerpStep = Mathf.Clamp(expectedVelocity.magnitude * (expectedVelocity - _rb.velocity).magnitude, .1f, 1f);
            _rb.velocity = Vector3.Lerp(_rb.velocity, expectedVelocity, lerpStep * Constant.Boat.accelerationConstant);
        }
    
        private void OnCollisionEnter(Collision other)
        {
            if(_justUnload) return;
            if(!_canMove) return;
            //Debug.Log("Boat Block Collision");
            var hitBlock = other.collider.GetComponent<GroundBlock>();
            if (hitBlock && hitBlock is not BoatBlock)
            {
                //In Contact, this collider is this
                var boatBlock = other.GetContact(0).thisCollider.GetComponent<BoatBlock>();
                var islandCoordinate = hitBlock.island.PosToCoordinate(boatBlock.transform.position);
                //Debug.Log("Boat Block = " + boatBlock);
                HitIsland(hitBlock.island, boatBlock.boatCoordinate, islandCoordinate);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            //Avoid load instantly after unload
            if(!_justUnload) return;
            var hitBlock = other.collider.GetComponent<GroundBlock>();
            if (hitBlock && hitBlock is not BoatBlock)
                _justUnload = false;

        }

        #endregion
    }
}