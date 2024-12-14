using Barterta.RagDoll;
using Barterta.ToolScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class FlyTrigger : InputTriggerBase
    {
        private RagdollContainer _ragdoll;
    
        private Rigidbody _rb;
        private Collider[] _cds;
        private Animator _animator;
        private FollowObject _camFollow;


        private bool _isFlying;

        [SerializeField] private Vector3 flyOffset;
        [SerializeField] private float flySpeed = 20f;

        private void Start()
        {
            _ragdoll = GetComponentInChildren<RagdollContainer>();
            _ragdoll.DisableRagdoll();      

            _rb = GetComponent<Rigidbody>();
            _cds = GetComponents<Collider>();
            _animator = GetComponentInChildren<Animator>();
            _camFollow = selfCam.GetComponent<FollowObject>();
        
            if(!Constant.OnTestComponent)
                EnterWorld();
        }

        public void Fly(Mark.Mark dest)
        {
            _isFlying = true;
            Enable();
        
            //Change layer to not collide with sand
            _ragdoll.gameObject.layer = LayerMask.NameToLayer("ThroughSand");
        
            var sq = DOTween.Sequence();
            //Set offset
            _camFollow.followTran = _ragdoll.transform;
            sq.Append(_camFollow.SetNewOffsetWithoutAngle(flyOffset, null, 1f));
            //Jump
            var position = dest.transform.position;
            sq.Append(_ragdoll.root.DOJump(position, 30, 1, (position - transform.position).magnitude / flySpeed));
            sq.OnComplete(delegate
            {
                //Set gravity
                _ragdoll.EnableGravity();
                //Change layer to collide with sand
                _ragdoll.gameObject.layer = LayerMask.NameToLayer("Player");
                Invoke("Disable", 2f);
            });
        }

        private void Enable()
        {
            StateController.ChangeAllState(this);
        
            _ragdoll.EnableRagdoll();
            _rb.isKinematic = true;
            foreach (var cd in _cds)
            {
                cd.enabled = false;
            }
            _animator.enabled = false;
        }

        private void Disable()
        {
            _isFlying = false;
            transform.position = Methods.YtoZero(_ragdoll.root.position) + .5f * Vector3.up;
            //Reset camera
            _camFollow.ResetFollow(.5f);
            StateController.ChangeToDefault();

            _ragdoll.DisableRagdoll();
            _rb.isKinematic = false;
            foreach (var cd in _cds)
            {
                cd.enabled = true;
            }
            _animator.enabled = true;
        }

        public void EnterWorld()
        {
            Enable();
            _ragdoll.EnableGravity();
            var transform1 = _ragdoll.root.transform;
            transform1.position = transform1.position + Vector3.up * 100f;
            _ragdoll.root.AddTorque(new Vector3(Random.value, Random.value, Random.value) * 10f);
            Invoke("Disable", 4f);
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
            
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            if (!_isFlying)
            {
                Disable();
            }
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
        }

        public override void EmergentExit()
        {
        }
    }
}