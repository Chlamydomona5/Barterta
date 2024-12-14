/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Barterta.Boat.Manual;
using Barterta.Island.MONO;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class SailTrigger_Past : InputTriggerBase
    {
        public bool OnSail => boatOn;
        [SerializeField] private Vector3 cameraHeight;
        /*[SerializeField] private GameObject shrinePointerPrefab;
        [SerializeField] private GameObject islandPointerPrefab;#1#
        [SerializeField] [ReadOnly] private ManualBoat boatOn;

        private Dictionary<Island.MONO.Island, GameObject> _islandToPointerDict;
        private FollowObject _camFollow;
        private int _xpCount;
        private Coroutine _xpCoroutine;

        private Island.MONO.Island _lastIslandWithShrine;

        private static readonly int IsBoating = Animator.StringToHash("isBoating");

        private void Start()
        {
            _camFollow = selfCam.GetComponent<FollowObject>();
        }

        private void FixedUpdate()
        {
            if (boatOn)
                boatOn.Move(StateController.GetDirectionInput(this) * UnityEngine.Time.fixedDeltaTime,
                    skillContainer.GetAttribute("sailSpeed"));
        }

        public void StartBoating(ManualBoat boat)
        {
            if (!_camFollow.isTweening)
            {
                StateController.ChangeAllState(this);
                //Lock
                _camFollow.OnTweenEnd.AddListener(EnterBoatMove);
                _camFollow.SetNewOffsetWithoutAngle(cameraHeight * skillContainer.GetAttribute("cameraHeight"), null, 1f);
                //register the boat, taking over the move system
                boatOn = boat;
                //Debug.Log(GetComponent<MoveTrigger>().GetStandBlock().island);
                //PointerProcess(GetComponent<MoveTrigger>().GetStandBlock().island);
                //Change Character's setting and position
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                Transform tran;
                (tran = transform).SetParent(boatOn.transform);
                tran.localPosition = boatOn.ridePoint.localPosition;
                tran.localRotation = boatOn.ridePoint.localRotation;
                Animator.SetBool(IsBoating, true);
                //Close detect
                detector.detectBlock = false;
            }
        }

        private void EnterBoatMove()
        {
            //Start Boat
            boatOn.GetComponent<Rigidbody>().isKinematic = false;
            //Level
            _xpCoroutine = StartCoroutine(SailXP());
        }

        private IEnumerator SailXP()
        {
            _xpCount = 0;
            while (true)
            {
                _xpCount += 1;
                yield return new WaitForSeconds(2);
            }
        }

        public void EndBoating(Vector3 pos)
        {
            if (!_camFollow.isTweening)
            {
                StateController.ChangeToDefault();

                //Resume Character's setting and position
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                transform.SetParent(null);
                Animator.SetBool(IsBoating, false);

                //Stop boat
                boatOn.GetComponent<Rigidbody>().isKinematic = true;
                boatOn.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //Unregister the event attached to boat
                //PointerProcess(null);
                boatOn = null;

                transform.position = pos;

                _camFollow.ResetFollow();
                
                StopCoroutine(_xpCoroutine);
                skillContainer.AddXPTo("Sailing",_xpCount);

                detector.detectBlock = true;
            }
        }

        /*public void PointerProcess(Island.MONO.Island island)
        {
            if (island)
            {
                _traceCoroutine = StartCoroutine(PointerCoroutine(island));
            }
            else
            {
                StopCoroutine(_traceCoroutine);
                foreach (var pair in _islandToPointerDict)
                {
                    Destroy(pair.Value);
                }
            }
        }

        private IEnumerator PointerCoroutine(Island.MONO.Island island)
        {
            _lastIslandWithShrine = island;
            _islandToPointerDict = new();
            if(_lastIslandWithShrine)
                _islandToPointerDict.Add(_lastIslandWithShrine, Instantiate(shrinePointerPrefab));

            while (true)
            {
                //Adjust pointers dict
                var activeIslandAround = WorldManager.I.GetActiveIslandAround(transform.position);
                foreach (var activeIsland in activeIslandAround)
                {
                    if(_islandToPointerDict.ContainsKey(activeIsland)) continue;
                    else if(activeIsland == _lastIslandWithShrine) continue;
                    //Add
                    else
                    {
                        _islandToPointerDict.Add(activeIsland, Instantiate(islandPointerPrefab));
                    }
                }
                //Delete part don't exist
                var deleteList = _islandToPointerDict.Where(x => (!activeIslandAround.Contains(x.Key) && x.Key != _lastIslandWithShrine)).ToList();
                for (int i = 0; i < deleteList.Count; i++)
                {
                    _islandToPointerDict.Remove(deleteList[i].Key);
                    Destroy(deleteList[i].Value);
                }

                //Adjust pointer pos
                foreach (var pair in _islandToPointerDict)
                {
                    AdjustPointer(pair);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private void AdjustPointer(KeyValuePair<Island.MONO.Island,GameObject> pair)
        {
            var position = transform.position;
            var vec = Methods.YtoZero(pair.Key.transform.position - position).normalized;
            var pos = position + vec * 6f;
            pos = new Vector3(pos.x, 2f, pos.z);
            pair.Value.transform.position = pos;
            pair.Value.transform.rotation = Quaternion.LookRotation(vec);
        }#1#

        private void Land()
        {
            //if boat has surround block
            //TODO: normal height is .5f for some reason
            var surroundBlock = boatOn.GetSurroundBlock();
            if (boatOn && surroundBlock)
            {
                boatOn.Harvest(surroundBlock.GetComponent<GroundBlock>());
                EndBoating(surroundBlock.transform.position + .5f * Vector3.up);
            }
        }

        public override void OnInteractPress(InputAction.CallbackContext ctx)
        {
            
        }

        public override void OnShortGrab(InputAction.CallbackContext ctx)
        {

        }

        public override void OnLongGrab(InputAction.CallbackContext ctx)
        {
            //GetComponent<FormerMapTrigger>().StartReadingMap(MapSituation.OnBoat);
        }

        public override void OnInteractCancel(InputAction.CallbackContext ctx)
        {
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
            if(!_camFollow.isTweening)
                Land();
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
        }

        public override void EmergentExit()
        {
            
        }
    }
}*/