using System.Collections;
using System.Collections.Generic;
using Barterta.Core;
using Barterta.ItemGrid;
using Barterta.Mark;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Floatable
{
    public class FloatableController : SerializedMonoBehaviour
    {
        [SerializeField] private float insInterval;
        [SerializeField] private Vector2 minMaxSpeed;
        [SerializeField] private float destoryDistance;
        private MarkContainer _playerContainer;
        
        [SerializeField] private Dictionary<Groundable, float> _defaultFloatDict;

        private void Start()
        {
            _playerContainer = Resources.Load<MarkContainer>("PlayerContainer");
            StartCoroutine(FloatableInstantiation());
        }

        private IEnumerator FloatableInstantiation()
        {
            while (true)
            {
                yield return new WaitForSeconds(insInterval);
                InsRandomFloatable();
            }
        }

        private void InsRandomFloatable()
        {
            if(_playerContainer.markList.Count == 0) return;
            var player = _playerContainer.markList[Random.Range(0, _playerContainer.markList.Count)];
            var center = Methods.YtoZero(player.transform.position);
            
            Vector3 insPos;
            float x = Random.Range(10f, 20f);
            x = Random.value > .5f ? x : -x;
            float z = Random.Range(10f, 20f);
            z = Random.value > .5f ? z : -z;
            insPos = center + new Vector3(x, 0, z);
            
            var ins = Methods.GetRandomValueInDict(_defaultFloatDict);
            var floatable = new GameObject("Floating" + ins.ID);
            floatable.AddComponent<Floatable>().Init(this, ins);

            floatable.transform.SetParent(transform);
            floatable.transform.position = insPos;

            var rb = floatable.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.angularDrag = .5f;
            //Set original velocity
            //Get Direction
            var dir = (center - insPos).normalized;
            //Weight avarage, 2 toward player : 1 random
            var weightDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized + 9 * dir;
            //Get Magnitude
            var velocity = weightDir.normalized * Random.Range(minMaxSpeed.x, minMaxSpeed.y);
            rb.velocity = velocity;
        }

        public Chunk FindChunkWithPos(Vector3 pos)
        {
            return WorldManager.I.FindInPool(WorldManager.PosToCoord(pos));
        }

        public bool IsAroundPlayer(Vector3 pos)
        {
            foreach (var player in _playerContainer.markList)
                if (Methods.YtoZero(player.transform.position - pos).magnitude < destoryDistance)
                    return true;
            return false;
        }
    }
}