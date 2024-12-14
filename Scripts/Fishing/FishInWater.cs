using System.Collections;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Fishing
{
    public class FishInWater : MonoBehaviour
    {
        [HideInInspector] public Groundable fishGroundable;
        [SerializeField] [ReadOnly] public FishFeature currentFeature;
        [SerializeField] [ReadOnly] public bool baited;
        [SerializeField] [ReadOnly] public FishAttractedMode mode;
        
        private Animator _animator;
        private Transform _boundPos;
        private Transform _buoy;
        private FishingTrigger _fisher;
        private Coroutine _freeMoveCoroutine;
        private readonly float _leaveTime = 8f;
        private float _nowFreeSpeed;
        private Rigidbody _rb;
        private ParticleSystem _rippleParticle;
        private Vector3 _targetDir;
        
        private bool _willLeave = true;

        public void AdjustRotation(Quaternion angle, float speed = 0f)
        {
            //Move
            Methods.RotateTowards(transform, angle).OnComplete(delegate
            {
                Vector3 velocity;
                velocity = transform.forward * Random.Range(currentFeature.freeMoveSpeedRange.x,
                    currentFeature.freeMoveSpeedRange.y);
                if(speed > 0) 
                    velocity = transform.forward * speed;
                _rb.velocity = velocity;
            });
        }

        #region FreeMode

        public void Init(Transform trans, Groundable fish, bool willLeave = true)
        {
            //Assignment
            _rb = GetComponent<Rigidbody>();
            _rippleParticle = GetComponentInChildren<ParticleSystem>();
            currentFeature = fish.GetComponent<FishFeatureContainer>().feature;
            fishGroundable = fish;
            _boundPos = trans;
            _animator = Instantiate(fish.GetComponentInChildren<Animator>(), transform);
            
            _willLeave = willLeave;
            //Add rarity outline
            //Methods.RarityOutline(_animator.gameObject, fish.GetComponent<FishFeatureContainer>().feature.rarity);
            //Adjust transform
            var tran = _animator.transform;
            tran.localPosition = Vector3.zero;
            tran.rotation = Quaternion.identity;
            //Start process
            _freeMoveCoroutine = StartCoroutine(FreeMove());
            if(_willLeave)
                StartCoroutine(CountToLeave());
        }

        private IEnumerator CountToLeave()
        {
            yield return new WaitForSeconds(Random.Range(currentFeature.leaveTimeRange.x,
                currentFeature.leaveTimeRange.y));
            if (!baited || !_boundPos)
                Leave();
        }

        private IEnumerator FreeMove()
        {
            _rippleParticle.Stop();
            while (true)
            {
                //Move
                AdjustRotation(Quaternion.Euler(0, Random.Range(0, 360f), 0));
                var timer = 0f;
                var maxTime = Random.Range(currentFeature.freeMoveTimeRange.x, currentFeature.freeMoveTimeRange.y);
                //Detect if out range or time out
                while (true)
                {
                    if (_boundPos)
                    {
                        //time out
                        timer += UnityEngine.Time.fixedDeltaTime;
                        if (timer > maxTime) break;
                        //dis not in range, then move toward backward
                        var blockToSelf = Methods.YtoZero(transform.position - _boundPos.transform.position);
                        var dis = blockToSelf.magnitude;
                        //move toward block
                        if (blockToSelf != Vector3.zero)
                        {
                            if (dis > currentFeature.boundBlockDisRange.y)
                                AdjustRotation(Quaternion.LookRotation(-blockToSelf.normalized));
                            else if (dis < currentFeature.boundBlockDisRange.x)
                                AdjustRotation(Quaternion.LookRotation(blockToSelf.normalized));      
                        }
                    }
                    yield return 30;
                }

                //Stay and wait
                _rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(Random.Range(currentFeature.freeStayTimeRange.x,
                    currentFeature.freeStayTimeRange.y));
            }
        }

        public void Leave()
        {
            StopAllCoroutines();
            
            _rb.velocity = Vector3.zero;
            var position = transform.position;
            var dir = Methods.YtoZero(position - _boundPos.transform.position).normalized;
            var endPos = position + dir * 20f;
            Methods.RotateTowards(transform, Quaternion.LookRotation(dir));
            _rb.DOMove(endPos, _leaveTime).OnComplete(delegate { Destroy(gameObject); });
        }

        #endregion

        #region AttractedMode

        public void EnterFishing(Transform buoy, FishingTrigger fisher)
        {
            //Assign
            _buoy = buoy;
            _fisher = fisher;
            baited = true;
            WorldUIManager.ChangeUILayer(_rippleParticle.gameObject, fisher.GetComponent<Mark.Mark>(), false);
            //Switch Process
            StopCoroutine(_freeMoveCoroutine);
            StartCoroutine(ModeSwitch());
            StartCoroutine(AttractedMove());
            StartCoroutine(Ripple());
        }

        private IEnumerator AttractedMove()
        {
            //Reset speed
            _rb.velocity = Vector3.zero;

            while (true)
            {
                var delta = _targetDir;
                switch (mode)
                {
                    case FishAttractedMode.Follow:
                        var diff = _buoy.position - transform.position;
                        diff = new Vector3(diff.x, 0, diff.z);
                        if (diff.magnitude > .1f)
                        {
                            delta = diff.normalized;
                            transform.rotation = Quaternion.LookRotation(delta);
                            delta *= currentFeature.followSpeed;
                        }
                        else
                        {
                            delta = Vector3.zero;
                        }

                        break;
                    case FishAttractedMode.Free:
                        delta *= _nowFreeSpeed;
                        break;
                    default:
                        delta = Vector3.zero;
                        break;
                }

                delta = new Vector3(delta.x, 0, delta.z);
                _rb.velocity = delta;

                //Detect whether fish escape, if not in Buoy or out of sight
                var dis = _buoy.position - transform.position;
                dis = new Vector3(dis.x, 0, dis.z);
                if (!_fisher.IsInView(gameObject)) Debug.Log("Not In View");
                if (dis.magnitude > currentFeature.escapeRange || !_fisher.IsInView(gameObject)) _fisher.EndFishing();

                //ChangeTestCircleColor();

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator Ripple()
        {
            while (true)
            {
                AdjustAndPlayParticle();
                yield return new WaitForSeconds(Random.Range(.7f, 1.5f));
            }
        }

        private void AdjustAndPlayParticle()
        {
            _rippleParticle.transform.localScale = new Vector3(1f, 1f, 1f) * (4f * currentFeature.escapeRange * .85f);

            _rippleParticle.Stop();
            _rippleParticle.Play();
        }

        private IEnumerator ModeSwitch()
        {
            while (true)
                //Switch mode
                if (mode == FishAttractedMode.Free)
                {
                    yield return new WaitForSeconds(Random.Range(currentFeature.stayFreeTimeRange.x,
                        currentFeature.stayFreeTimeRange.y));
                    SwitchTo(FishAttractedMode.Follow);
                }
                else if (mode == FishAttractedMode.Follow)
                {
                    yield return new WaitForSeconds(Random.Range(currentFeature.stayFollowTimeRange.x,
                        currentFeature.stayFollowTimeRange.y));
                    SwitchTo(FishAttractedMode.Free);
                }
        }

        public void SwitchTo(FishAttractedMode to)
        {
            mode = to;
            switch (mode)
            {
                case FishAttractedMode.Follow:
                    //targeting Buoy.
                    _targetDir = Methods.YtoZero(_buoy.position - transform.position).normalized;
                    break;
                case FishAttractedMode.Free:
                    //targeting a random dir
                    _nowFreeSpeed = Random.Range(currentFeature.freeSpeedRange.x, currentFeature.freeSpeedRange.y);
                    _targetDir = new Vector3(Random.value, 0, Random.value).normalized;
                    break;
                default:
                    _targetDir = Vector3.zero;
                    break;
            }

            //Rotate to target dir TEST
            transform.DORotateQuaternion(Quaternion.LookRotation(_targetDir), .3f);
        }

        public Sequence OutWater(GroundBlock block)
        {
            StopAllCoroutines();
            return transform.BounceToGroundBlock(block);
        }

        #endregion
    }
}