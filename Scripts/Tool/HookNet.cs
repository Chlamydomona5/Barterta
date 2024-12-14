using System.Collections;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;

namespace Barterta.Tool
{
    public class HookNet : Tool
    {
        public Collider netCollider;
        [HideInInspector] public Net net;
        
        [SerializeField] private Collider fishCollider;
        [SerializeField] private Material wideOpenMat;
        [SerializeField] private Material inWaterMat;
        [SerializeField] private ParticleSystem splashParticle;
        [SerializeField] public ParticleSystem moveParticle;

        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public Animator animator;
        private SkinnedMeshRenderer _renderer;
        private HookNetTrigger _trigger;
        private static readonly int Open = Animator.StringToHash("Open");

        protected override void Awake()
        {
            base.Awake();
            //Assign
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            _renderer = animator.GetComponentInChildren<SkinnedMeshRenderer>();
            net = GetComponentInChildren<Net>();
            //Reset constant
            rb.isKinematic = true;
            fishCollider.enabled = false;
            netCollider.enabled = false;
            
            moveParticle.gameObject.SetActive(false);
        }
        
        public override bool Judge(bool isLong, GrabTrigger trigger)
        {
            return !trigger.detector.targetBlock;
        }

        public override bool Effect(GroundBlock target, ToolTrigger trigger)
        {
            if (GetComponentInParent<HookNetTrigger>())
            {
                _trigger = GetComponentInParent<HookNetTrigger>();
                _trigger.StartHook(this, boostTime);
            }
            return false;
        }

        public void Release(Vector3 velocity)
        {
            //Debug.Log("Net Velocity: " + velocity.magnitude);
            //Transform
            transform.SetParent(null);
            transform.rotation = Quaternion.LookRotation(Methods.YtoZero(velocity));
            //Physics
            rb.isKinematic = false;
            rb.useGravity = true;
            //TODO: (May has problem)clamp velocity magnitude down to 6
            if (velocity.magnitude < 6.5)
                velocity = velocity.normalized * 6.5f;
            rb.velocity = velocity;
            //Visual Elements
            animator.SetTrigger(Open);
            _renderer.material = wideOpenMat;

            StartCoroutine(StopDetect());
        }

        public void ReturnKinematic()
        {
            rb.isKinematic = true;
            netCollider.enabled = false;
            fishCollider.enabled = false;
        }
        

        private IEnumerator StopDetect()
        {
            while (true)
            {
                if (transform.position.y < _trigger.stopHeight)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    break;
                }

                yield return new WaitForFixedUpdate();
            }

            splashParticle.Play();
            moveParticle.gameObject.SetActive(true);
            //After stop
            netCollider.enabled = true;
            fishCollider.enabled = true;
            _renderer.material = inWaterMat;

            _trigger.StartRecollect();
        }

        public override void OnMove(bool isToHand, GrabTrigger trigger, GroundBlock block)
        {
            base.OnMove(isToHand, trigger, block);
            if (isToHand) trigger.selfCam.GetComponent<FollowObject>().SetNewOffsetWithoutAngle(new Vector3(0, 15, -12));
            else trigger.selfCam.GetComponent<FollowObject>().ResetFollow();
        }
    }
}