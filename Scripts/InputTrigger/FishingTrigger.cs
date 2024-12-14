using System.Collections;
using Barterta.Fishing;
using Barterta.Player;
using Barterta.Sound;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class FishingTrigger : InputTriggerBase
    {
        private static readonly int FishingRod = Animator.StringToHash("FishingRod");

        [Title("Constant", titleAlignment: TitleAlignments.Centered)] 
        [SerializeField] private float fishPlaneYCoordinate;
        [SerializeField] private float buoyDistance = 7f;
        [SerializeField] private float buoyForce = 2f;
        [SerializeField] private float dragConstant = .8f;
        [SerializeField] private float dragSpeedChangeConstant = .2f;
        [SerializeField] private float catchRange;
        [SerializeField] private float maxSpeed = 2;

        [Title("Reference", titleAlignment: TitleAlignments.Centered)] 
        [SerializeField] private GameObject buoyPrefab;
        [SerializeField] private GameObject buoySignPrefab;

        private GameObject _buoySignInstance;
        private GameObject _buoyInstace;
        private Rigidbody _buoyRb;
        private Coroutine _fishingCoroutine;
        private FishInWater _fishInstance;
        private bool _isDragging;
        private static readonly int Pulling = Animator.StringToHash("Pulling");
        
        private float _astable;
        private float _pullConstant;

        public void StartFishing(float time, float astable, float pullConstant)
        {
            _astable = astable;
            _pullConstant = pullConstant;
            
            StateController.ChangeAllState(this);

            //Set distance and Buoy instance
            SetBuoy(time);

            _fishingCoroutine = StartCoroutine(FishingUpdate());

            //Set Animator
            Animator.SetBool(FishingRod, true);

            SetCamera(true);
        }

        public void EndFishing(bool success = false)
        {
            StateController.ChangeToDefault();
            //if has instance
            if (_fishInstance && _fishInstance.baited)
                //if not success
                if (!success)
                    _fishInstance.Leave();
            Destroy(_buoyInstace);
            StopCoroutine(_fishingCoroutine);

            //Set animator
            Animator.SetBool(FishingRod, false);

            SetCamera(false);

            _isDragging = false;
        }

        public float GetDistance()
        {
            var vec = _fishInstance.transform.position - transform.position;
            var y0Vec = new Vector3(vec.x, 0, vec.z);
            return y0Vec.magnitude;
        }

        private void SetCamera(bool start)
        {
            var cam = selfCam;
            if (start)
            {
                //Set camera pos
                var tran = transform;
                var topdown = Vector3.down;
                var lookAt = Quaternion.AngleAxis(25f,-tran.right) * topdown;
                cam.GetComponent<FollowObject>().SetNewOffsetWithAngle(Vector3.up * 8f - tran.forward * 2f,
                    Quaternion.LookRotation(lookAt).eulerAngles, null, .5f);
            }
            else
            {
                cam.GetComponent<FollowObject>().ResetFollow(.5f);
            }
        }

        public void Drag()
        {
            if (!_fishInstance || !_fishInstance.baited)
            {
                EndFishing();
                return;
            }

            Animator.SetTrigger(Pulling);

            if (!_isDragging)
            {
                _isDragging = true;
                var currentV = _buoyRb.velocity;
                _buoyRb.DOMove(_buoyRb.transform.position - currentV.normalized * dragConstant, .25f / _pullConstant)
                    .OnComplete(
                        delegate
                        {
                            _isDragging = false;
                            _buoyRb.velocity = currentV * dragSpeedChangeConstant;
                        }).SetEase(Ease.OutCubic);
            }
        }

        private void Move(Vector2 vec)
        {
            if (!_isDragging)
            {
                var tran = transform;
                var transVec = vec.y * tran.forward + vec.x * tran.right;
                if (_buoyRb.velocity.magnitude > maxSpeed)
                {
                    var projection = Vector3.Project(transVec, _buoyRb.velocity);
                    //If the projection is in the same direction of velocity
                    if(_buoyRb.velocity.x / projection.x > 0 || _buoyRb.velocity.z / projection.z > 0)
                        transVec -= projection;
                }
                transVec = transVec.normalized;
                _buoyRb.AddForce(transVec * buoyForce * _astable);
                //Log velocity
                Debug.Log("buoy velocity = " + _buoyRb.velocity + " buoy force = " + transVec);
            }
        }

        public void SetBuoy(float time)
        {
            Destroy(_buoySignInstance);
            _buoyInstace = Instantiate(buoyPrefab);
            _buoyInstace.GetComponent<Buoy>().Init(this);
            _buoyRb = _buoyInstace.GetComponent<Rigidbody>();
            //Set first position
            _buoyInstace.transform.position = TimeToBuoyPos(time);
            //Sound
            SoundManager.I.PlaySound("Set buoy");
        }

        private Vector3 TimeToBuoyPos(float time)
        {
            Vector3 position;
            var tran = transform;
            position = tran.forward * (time * skillContainer.GetAttribute("buoyDistance") * buoyDistance) + tran.position;
            position = new Vector3(position.x, fishPlaneYCoordinate, position.z);
            return position;
        }

        private IEnumerator FishingUpdate()
        {
            yield return new WaitForSeconds(1);
            //Wait for fish attracted by buoy
            while (true)
            {
                //if in range && attracted
                if (_fishInstance &&
                    Methods.YtoZero(_fishInstance.transform.position - _buoyInstace.transform.position).magnitude <
                    _fishInstance.currentFeature.escapeRange)
                    break;
                yield return new WaitForSeconds(1);
            }

            _fishInstance.EnterFishing(_buoyInstace.transform, this);

            var timer = 0f;
            while (true)
            {
                //Debug.Log(GetDistance());
                Move(StateController.GetDirectionInput(this));
                //If stay green for more than 1 sec, loot auto
                if (GetDistance() < catchRange)
                {
                    timer += UnityEngine.Time.deltaTime;
                    if (timer > .5f) Success();
                }
                else
                {
                    timer = 0f;
                }

                yield return new WaitForFixedUpdate();
            }
        }

        public void BuoyAttract(FishInWater fish)
        {
            _fishInstance = fish;
        }

        private void Success()
        {
            var fish = _fishInstance.fishGroundable;
            //Skill
            skillContainer.AddXPTo("Fishing", fish.value);
            //Loot
            var block = GetComponent<GridDetector>().GetStandBlock(false).BlockSet
                .GetRandomSurroundStackableBlock(GetComponent<GridDetector>().GetStandBlock(false).Coordinate);
            if(block)
                _fishInstance.OutWater(block).OnComplete(delegate
                {
                    Instantiate(fish).SetOn(block);
                    fish.value += (int)skillContainer.GetAttribute("fishValueBonus");
                    Destroy(_fishInstance.gameObject);
                });
            else
            {
                GetComponent<DialogTrigger>().SelfBark("noenoughspaceforfish");
                Destroy(_fishInstance.gameObject);
            }
            
            EndFishing(true);
        }

        public bool IsInView(GameObject tar)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(selfCam);
            foreach (var plane in planes)
                if (plane.GetDistanceToPoint(tar.transform.position) < 0)
                    return false;
            return true;
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
            Drag();
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            EndFishing();
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
        }

        public override void EmergentExit()
        {
            EndFishing();
        }

        public void SetBuoySign(float boostTime)
        {
            if (!_buoySignInstance) _buoySignInstance = Instantiate(buoySignPrefab);
            _buoySignInstance.transform.position = TimeToBuoyPos(boostTime);
        }
    }
}