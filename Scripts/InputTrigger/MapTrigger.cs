using System.Linq;
using Barterta.Core;
using Barterta.Island.MONO;
using Barterta.Mark;
using Barterta.Player;
using Barterta.ToolScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class MapTrigger : InputTriggerBase
    {
        [SerializeField] private Vector3 camOffset = new(0, 30, -24);
        private Tween _moveCam;

        private MarkContainer _shrineContainer;
        private Mark.Mark _currentIsland;
        public Mark.Mark CurrentIsland
        {
            get => _currentIsland;
            set
            {
                _currentIsland = value;
                _moveCam = _camFollow.SetNewOffsetWithoutAngle(camOffset, value.transform, 1.5f);
            }
        }
        private FollowObject _camFollow;

        private UnityEvent<Mark.Mark> _onConfirm = new();

        private bool _willAutoCancel;
    
        private void Start()
        {
            _shrineContainer = Resources.Load<MarkContainer>("IslandMarkContainer");
            _camFollow = selfCam.GetComponent<FollowObject>();
        }
        public void StartMap(UnityAction<Mark.Mark> confirm, bool autoCancel)
        {
            StateController.ChangeAllState(this);
        
            CurrentIsland = GetComponent<GridDetector>().GetStandBlock().island.islandMark;
            _onConfirm.AddListener(confirm);

            _willAutoCancel = autoCancel;
        }

        private void GetInput(Vector2Int vec)
        {
            if (_moveCam == null || !_moveCam.active)
            {
                //Add Current to supervised, make it appear
                WorldManager.I.RemoveSupervisedChunk(WorldManager.I.FindInPool(WorldManager.PosToCoord(CurrentIsland.transform.position)));
                
                if (vec.x != 0)
                {
                    //if vec.x == 1, then save right side, == -1 save left
                    var sideList =
                        _shrineContainer.markList.FindAll(x => (x.transform.position.x - _currentIsland.transform.position.x) * vec.x > 0 && x != _currentIsland);
                    var ordered = sideList.OrderBy(x => Mathf.Abs(x.transform.position.x - _currentIsland.transform.position.x)).ToList();
                    if(ordered.Count > 0)
                        CurrentIsland = ordered[0].GetComponent<Mark.Mark>();
                }
        
                if (vec.y != 0)
                {
                    var sideList =
                        _shrineContainer.markList.FindAll(x => (x.transform.position.z - _currentIsland.transform.position.z) * vec.y > 0  && x != _currentIsland);
                    var ordered = sideList.OrderBy(x => Mathf.Abs(x.transform.position.z - _currentIsland.transform.position.z)).ToList();
                    if(ordered.Count > 0)
                        CurrentIsland = ordered[0].GetComponent<Mark.Mark>();
                }
                
                WorldManager.I.AddSupervisedChunk(WorldManager.I.FindInPool(WorldManager.PosToCoord(CurrentIsland.transform.position)));
            }
        }

        private void Cancel()
        {
            StateController.ChangeToDefault();
            _onConfirm.RemoveAllListeners();
            //Reset camera
            _camFollow.ResetFollow(.5f);
            WorldManager.I.RemoveSupervisedChunk(WorldManager.I.FindInPool(WorldManager.PosToCoord(CurrentIsland.transform.position)));
        }

        public override void OnInteractPress(InputAction.CallbackContext ctx)
        {
            
        }

        public override void OnShortGrab(InputAction.CallbackContext ctx)
        {
        }

        public override void OnLongGrab(InputAction.CallbackContext ctx)
        {
        }

        public override void OnInteractCancel(InputAction.CallbackContext ctx)
        {
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
            _onConfirm?.Invoke(CurrentIsland);
            _onConfirm?.RemoveAllListeners();

            if(_willAutoCancel)
                Cancel();
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            Cancel();
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
            //Tranforming input in to dir input
            //Debug.Log(ctx.ReadValue<Vector2>());
            var ret = new Vector2Int();
            if (ctx.ReadValue<Vector2>().x > 0.2)
            {
                ret.x = 1;
                GetInput(ret);
                return;
            }
            else if (ctx.ReadValue<Vector2>().x < -0.2)
            {
                ret.x = -1;
                GetInput(ret);
                return;
            }
            else ret.x = 0;

            if (ctx.ReadValue<Vector2>().y > 0.2) ret.y = 1;
            else if (ctx.ReadValue<Vector2>().y < -0.2) ret.y = -1;
            else ret.y = 0;

            //Debug.Log(ret);
            GetInput(ret);
        }

        public override void EmergentExit()
        {
            Cancel();
        }
    }
}