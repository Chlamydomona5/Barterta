using Barterta.Player;
using Barterta.Skill;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public abstract class InputTriggerBase : MonoBehaviour
    {
        [HideInInspector] public GridDetector detector;
        [HideInInspector] public MouseHandler mouseHandler;
        protected Animator Animator;
        protected Transform Root;
        [HideInInspector] public Camera selfCam;
        protected InputStateController StateController;
        [HideInInspector] public SkillContainer skillContainer;
        protected Rigidbody Rb;

        protected virtual void Awake()
        {
            StateController = GetComponent<InputStateController>();
            Animator = GetComponentInChildren<Animator>();
            selfCam = transform.parent.GetComponentInChildren<Camera>();
            Root = GetComponentsInParent<Transform>()[1];
            detector = GetComponent<GridDetector>();
            skillContainer = GetComponent<SkillContainer>();
            Rb = GetComponent<Rigidbody>();
            mouseHandler = GetComponent<MouseHandler>();
        }

        public abstract void OnInteractPress(InputAction.CallbackContext ctx);

        public abstract void OnShortGrab(InputAction.CallbackContext ctx);

        public abstract void OnLongGrab(InputAction.CallbackContext ctx);

        public abstract void OnInteractCancel(InputAction.CallbackContext ctx);
        
        public abstract void OnShortInteract(InputAction.CallbackContext ctx);

        public abstract void OnLongInteract(InputAction.CallbackContext ctx);

        public abstract void OnDirection(InputAction.CallbackContext ctx);
        
        public abstract void EmergentExit();
    }
}