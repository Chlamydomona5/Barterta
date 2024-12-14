using Barterta.ItemGrid;
using Barterta.ToolScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Barterta.InputTrigger
{
    public class MoveTrigger : InputTriggerBase
    {
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        [SerializeField] private float moveAcc;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float friction = 0.2f;

        private void Move(Vector2 vector)
        {
            transform.LookAt(transform.position + new Vector3(vector.x, 0, vector.y));
            //if not face edge
            if (!EdgeAvoidance())
            {
                //Move
                if (vector.magnitude > .1f)
                {
                    if(Rb.velocity.magnitude <= moveSpeed)
                        Rb.AddForce(moveAcc * transform.forward);
                }   
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
        }

        public override void OnInteractCancel(InputAction.CallbackContext ctx)
        {
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
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

        private void CheckAnimationChange()
        {
            Animator.SetBool(IsRunning, Rb.velocity.magnitude > 0.5f);

        }

        private void Friction()
        {
            //Linear friction and angular friction
            Rb.velocity = Vector3.Lerp(Rb.velocity, Vector3.zero, friction);
        }

        #region Unity_Logic

        private void FixedUpdate()
        {
            Move(StateController.GetDirectionInput(this));
            CheckAnimationChange();
            Friction();
        }

        private bool EdgeAvoidance()
        {
            //Raycast if the velocity direction has a block
            RaycastHit hit;
            var detectStep = transform.forward * (UnityEngine.Time.fixedDeltaTime * 10f);
            //Debug.Log("DetectStep: " + detectStep);
            Physics.Raycast(transform.position + detectStep + Vector3.up * 2f, Vector3.down, out hit, 10f,
                LayerMask.GetMask("Ground", "Boat"));
            //Draw a line to show the raycast
            //Debug.DrawRay(transform.position + detectStep, Vector3.down * 5f, Color.red, UnityEngine.Time.fixedDeltaTime);
            if (!hit.collider)
            {
                Rb.velocity = Vector3.zero;
                return true;
            }
            return false;
        }

        #endregion
    }
}