using Barterta.ToolScripts;
using Barterta.UI.ScreenUI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class InputStateController : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private InputTriggerBase grabState;
        [SerializeField] [ReadOnly] private InputTriggerBase directionState;
        [SerializeField] private InputTriggerBase interactState;
        
        private InputTriggerBase _defaultDirectionState;
        private InputTriggerBase _defaultGrabState;
        private InputTriggerBase _defaultInteractState;
        
        private InputAction _directionAction;
        private InputAction _backpackAction;
        private InputAction _backpackNumAction;

        protected InputActionMap Input;

        private SettingsPanel _settings;
        
        private void Awake()
        {
            _settings = FindObjectOfType<SettingsPanel>(true);
            
            _defaultDirectionState = GetComponent<MoveTrigger>();
            _defaultGrabState = GetComponent<GrabTrigger>();
            _defaultInteractState = GetComponent<GrabTrigger>();

            Input = GetComponent<PlayerInput>().actions.FindActionMap(Constant.Input.ActionMap);

            _directionAction = Input.FindAction(Constant.Input.DirectionInput);
            _backpackAction = Input.FindAction(Constant.Input.Backpack);
            _backpackNumAction = Input.FindAction(Constant.Input.BackpackNum);

            _backpackAction.performed += GetComponent<BackpackTrigger>().OnBackpackKey;
            _backpackNumAction.performed += GetComponent<BackpackTrigger>().OnBackpackNumKey;

            Input.FindAction("Settings").performed += OpenSettings;
            Input.FindAction("Detail").started += GetComponent<CraftPanelTrigger>().OnCraftPanelKey;

            ChangeToDefault();
        }

        private void OpenSettings(InputAction.CallbackContext ctx)
        {
            _settings.gameObject.SetActive(!_settings.gameObject.activeSelf);
        }

        private void OnDestroy()
        {
            _backpackAction.performed -= GetComponent<BackpackTrigger>().OnBackpackKey;
            
            Input.FindAction(Constant.Input.ShortGrab).performed -= grabState.OnShortGrab;
            Input.FindAction(Constant.Input.LongGrab).performed -= grabState.OnLongGrab;
            
            Input.FindAction(Constant.Input.ShortInteract).performed -= interactState.OnShortInteract;
            Input.FindAction(Constant.Input.LongInteract).performed -= interactState.OnLongInteract;
            Input.FindAction(Constant.Input.InteractCancel).performed -= interactState.OnInteractCancel;
            Input.FindAction(Constant.Input.InteractPress).performed -= interactState.OnInteractPress;
            
            Input.FindAction(Constant.Input.DirectionInput).performed -= directionState.OnDirection;
            
            Input.FindAction("Settings").performed -= OpenSettings;
        }

        public void ChangeAllState(InputTriggerBase trigger)
        {
            ChangeGrabState(trigger);
            ChangeDirectionState(trigger);
            ChangeInteractState(trigger);
        }

        public void EmergentExitCurrent()
        {
            grabState.EmergentExit();
            directionState.EmergentExit();
            interactState.EmergentExit();
            ChangeToDefault();
        }

        public void ChangeGrabState(InputTriggerBase trigger)
        {
            if (grabState)
            {
                Input.FindAction(Constant.Input.ShortGrab).performed -= grabState.OnShortGrab;
                Input.FindAction(Constant.Input.LongGrab).performed -= grabState.OnLongGrab;
            }
            Input.FindAction(Constant.Input.ShortGrab).performed += trigger.OnShortGrab;
            Input.FindAction(Constant.Input.LongGrab).performed += trigger.OnLongGrab;
            
            grabState = trigger;
        }

        public void ChangeInteractState(InputTriggerBase trigger)
        {
            if (interactState)
            {
                Input.FindAction(Constant.Input.ShortInteract).performed -= interactState.OnShortInteract;
                Input.FindAction(Constant.Input.LongInteract).performed -= interactState.OnLongInteract;
                Input.FindAction(Constant.Input.InteractCancel).performed -= interactState.OnInteractCancel;
                Input.FindAction(Constant.Input.InteractPress).performed -= interactState.OnInteractPress;
            }
            Input.FindAction(Constant.Input.ShortInteract).performed += trigger.OnShortInteract;
            Input.FindAction(Constant.Input.LongInteract).performed += trigger.OnLongInteract;
            Input.FindAction(Constant.Input.InteractCancel).performed += trigger.OnInteractCancel;
            Input.FindAction(Constant.Input.InteractPress).performed += trigger.OnInteractPress;

            interactState = trigger;
        }

        public void ChangeDirectionState(InputTriggerBase trigger)
        {
            if (directionState)
                Input.FindAction(Constant.Input.DirectionInput).performed -= directionState.OnDirection;
            Input.FindAction(Constant.Input.DirectionInput).performed += trigger.OnDirection;
            directionState = trigger;
        }

        public Vector2 GetDirectionInput(InputTriggerBase trigger)
        {
            if (directionState == trigger) return _directionAction.ReadValue<Vector2>();
            return Vector2.zero;
        }


        public float GetBackpackInput(InputTriggerBase trigger)
        {
            Debug.Log(_backpackAction.ReadValue<float>());
            return _backpackAction.ReadValue<float>();
        }

        public void ChangeToDefault()
        {
            ChangeGrabState(_defaultGrabState);
            ChangeDirectionState(_defaultDirectionState);
            ChangeInteractState(_defaultInteractState);
        }
    }
}