/*using System.Collections;
using Barterta.Core;
using Barterta.InputTrigger;
using Barterta.Mark;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Seal
{
    public class WildSeal : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private WildSealMode currentMode = WildSealMode.Wander;

        [Title("Constant")] [SerializeField] private float wanderSpeed;

        [SerializeField] private float followRange;
        [SerializeField] private float followStopRange;
        [SerializeField] private float followSpeed;
        [SerializeField] private float tameRange;
        [SerializeField] private float tameSpeed;
        private Chunk _belongedChunk;
        private Transform _followedPlayer;
        private MarkContainer _playerContainer;
        private Rigidbody _rb;
        private MarkContainer _shrineContainer;
        private Transform _tamedShrine;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _playerContainer = Resources.Load<MarkContainer>("PlayerContainer");
            _shrineContainer = Resources.Load<MarkContainer>("IslandMarkContainer");

            StartCoroutine(Move());
        }

        private void OnTriggerEnter(Collider other)
        {
            //TODO:Seal's Tame logic
            /*if (other.GetComponent<GroundBlock>() && _tamedShrine &&
                other.GetComponent<GroundBlock>().island && other.GetComponent<GroundBlock>().island.mark &&
                other.GetComponent<GroundBlock>().island.shrine.transform == _tamedShrine)
                TurnToTamed();#1#
        }

        public void Init(Chunk chunk)
        {
            _belongedChunk = chunk;
        }

        private IEnumerator Move()
        {
            var selfTransform = transform;
            while (true)
                switch (currentMode)
                {
                    case WildSealMode.Wander:
                        while (currentMode == WildSealMode.Wander)
                        {
                            //First Move, Choose a random direction
                            RotateAndMoveForward(Quaternion.Euler(0, Random.Range(0, 360f), 0), wanderSpeed);
                            //Check if out chunk, if so, change direction
                            while (currentMode == WildSealMode.Wander)
                            {
                                if (_belongedChunk)
                                {
                                    //Check Out of chunk
                                    if (CheckOutOfChunk())
                                    {
                                        var position = selfTransform.position;
                                        var toChunkCenter = Methods.YtoZero(_belongedChunk.transform.position - position);
                                        RotateAndMoveForward(
                                            Quaternion.LookRotation(toChunkCenter + Vector3.up * position.y),
                                            wanderSpeed);
                                    }   
                                }
                                //Start seal, wait for player activate shrine
                                else _rb.velocity = Vector3.zero;

                                //Check Mode Switch
                                CheckWanderToFollow();
                                CheckAllToTame();
                                yield return new WaitForSeconds(1.5f);
                            }
                        }

                        break;

                    case WildSealMode.Follow:
                        while (currentMode == WildSealMode.Follow)
                        {
                            if (_belongedChunk)
                            {
                                //rotate to player
                                var position = selfTransform.position;
                                selfTransform.rotation = Quaternion.LookRotation(
                                    Methods.YtoZero(_followedPlayer.transform.position - position) +
                                    Vector3.down * position.y);
                                //Move if not in stop range
                                if (Methods.YtoZero(transform.position - _followedPlayer.position).magnitude > followStopRange)
                                    _rb.velocity = selfTransform.forward * followSpeed;
                                else
                                    _rb.velocity = Vector3.zero;
                            }
                            CheckFollowToWander();
                            CheckAllToTame();
                            yield return new WaitForFixedUpdate();   
                        }

                        break;

                    case WildSealMode.Tame:
                        while (currentMode == WildSealMode.Tame)
                        {
                            //rotate to shrine
                            var position = selfTransform.position;
                            selfTransform.rotation = Quaternion.LookRotation(
                                Methods.YtoZero(_tamedShrine.transform.position - position) +
                                Vector3.down * position.y);
                            _rb.velocity = selfTransform.forward * tameSpeed;
                            //If no triggered to tamed, turn back to wander
                            yield return new WaitForSeconds(20f);
                            currentMode = WildSealMode.Wander;
                        }
                        break;
                }
        }

        private void RotateAndMoveForward(Quaternion quaternion, float speed)
        {
            Methods.RotateTowards(transform, quaternion).OnComplete(delegate
            {
                _rb.velocity = transform.forward * speed;
            });
        }

        private bool CheckOutOfChunk()
        {
            return WorldManager.PosToCoord(transform.position) != _belongedChunk.coordinate;
        }

        private void CheckWanderToFollow()
        {
            foreach (var player in _playerContainer.markList)
            {
                if (!player.GetComponent<SailTrigger_Past>().OnSail) continue;
                if (Methods.YtoZero(transform.position - player.transform.position).magnitude < followRange)
                {
                    _followedPlayer = player.transform;
                    currentMode = WildSealMode.Follow;
                    break;
                }   
            }
        }


        private void CheckFollowToWander()
        {
            //if not on boat
            if (!_followedPlayer.GetComponent<SailTrigger_Past>().OnSail) currentMode = WildSealMode.Wander;
            //if out range
            if (Methods.YtoZero(transform.position - _followedPlayer.position).magnitude > followRange)
                currentMode = WildSealMode.Wander;
        }

        private void CheckAllToTame()
        {
            foreach (var shrine in _shrineContainer.markList)
                if (Methods.YtoZero(transform.position - shrine.transform.position).magnitude < tameRange)
                {
                    _tamedShrine = shrine.transform;
                    currentMode = WildSealMode.Tame;
                }
        }

        private void TurnToTamed()
        {
            var position = transform.position;
            Instantiate(Resources.Load<Seal>("Seal/Seal"), new Vector3(position.x, -.5f, position.z),
                Quaternion.identity);
            Destroy(gameObject);
        }
    }
}*/