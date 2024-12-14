using System;
using System.Collections.Generic;
using Barterta.Core;
using Barterta.InputTrigger;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.ItemGrid
{
    [Serializable]
    public class GroundBlock : MonoBehaviour
    {
        public virtual BlockSet BlockSet => island;

        public virtual Vector2Int Coordinate => coordinate;
        //Attain block map
        [ReadOnly] public Island.MONO.Island island;
        [ReadOnly] public Vector2Int coordinate;

        public List<Groundable> groundablesOn = new();

        public virtual void BePlaced()
        {
            
        }
        
        public void ClearAll()
        {
            groundablesOn.Clear();
        }
        
        public void TryRemoveNull()
        {
            for (var i = groundablesOn.Count - 1; i > -1; i--)
                if (groundablesOn[i] == null)
                    groundablesOn.RemoveAt(i);
        }

        public void DestoryAll()
        {
            //Since cant use foreach, use index from top to 0 instead.
            for (var i = groundablesOn.Count - 1; i > -1; i--) Destroy(groundablesOn[i].gameObject);

            groundablesOn.Clear();
        }

        public bool TransferAll(GroundBlock block, GrabTrigger trigger = null)
        {
            //Test cant grab
            if (groundablesOn.Count > 0 && groundablesOn[0].cantBeGrabbed) return false;

            if (groundablesOn.Count > 0)
                foreach (var groundable in groundablesOn) groundable.SetOn(block, trigger);
            ClearAll();
            return true;
        }

        public bool TransferTop(GroundBlock block, GrabTrigger trigger = null)
        {
            //Test cant grab
            if (groundablesOn.Count > 0 && groundablesOn[0].cantBeGrabbed) return false;

            if (groundablesOn.Count > 0)
            {
                var groundable = groundablesOn[groundablesOn.Count - 1];
                groundable.BeRemovedFromNowBlock();
                groundable.SetOn(block, trigger);
                return true;
            }

            return false;
        }

        public void ChangeMaterial(Material material)
        {
            GetComponent<MeshRenderer>().material = material;
        }
    }
}