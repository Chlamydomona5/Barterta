using Barterta.Boat.Components;
using Barterta.PointArrow;
using Barterta.ToolScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Barterta.InputTrigger
{
    public class BoatTrigger : InputTriggerBase
    {
        [SerializeField] private BoatConsole currentConsole;
        public bool IsControlling => currentConsole;
        public bool canHop = true;
        private PointArrowController _arrowController;
    
        public void StartControll(BoatConsole console)
        {
            StateController.ChangeAllState(this);
            currentConsole = console;
            //Enable move
            currentConsole.belongBoat.SetMoveActive(true);
            //Add to transform
            transform.SetParent(currentConsole.belongBoat.transform);
            Rb.isKinematic = true;
            
            detector.detectBlock = false;

            _arrowController.enabled = true;
            
            if(currentConsole.belongBoat.hasViewEnchancer) 
                selfCam.GetComponent<FollowObject>().SetNewOffsetWithoutAngle(new Vector3(0, 20, -16));
            else
                selfCam.GetComponent<FollowObject>().SetNewOffsetWithoutAngle(new Vector3(0, 15, -12));
        }
    
        public void EndControll()
        {
            currentConsole.belongBoat.SetMoveActive(false);
            StateController.ChangeToDefault();
            currentConsole = null;
            Rb.isKinematic = false;

            detector.detectBlock = true;
            
            _arrowController.enabled = false;
            
            selfCam.GetComponent<FollowObject>().ResetFollow();
        }
        
        
        protected override void Awake()
        {
            base.Awake();
            _arrowController = GetComponent<PointArrowController>();
        }

        private void FixedUpdate()
        {
            if (currentConsole)
            {
                currentConsole.Input(StateController.GetDirectionInput(this));
            }
        }

        public override void OnInteractPress(InputAction.CallbackContext ctx)
        {
            EndControll();
        }

        public override void OnShortGrab(InputAction.CallbackContext ctx) {}

        public override void OnLongGrab(InputAction.CallbackContext ctx) {}

        public override void OnInteractCancel(InputAction.CallbackContext ctx) {}

        public override void OnShortInteract(InputAction.CallbackContext ctx) {}

        public override void OnLongInteract(InputAction.CallbackContext ctx) {}

        public override void OnDirection(InputAction.CallbackContext ctx) {}

        public override void EmergentExit() {}
    }
}