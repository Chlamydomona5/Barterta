using System.Collections.Generic;
using Barterta.Map;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class FormerMapTrigger : InputTriggerBase
    {
        [SerializeField] private MapUIController controller;
        [Title("SavedInfo")] public List<MapDot> savedDots;
        [SerializeField] public Camera mapCam;

        [SerializeField] [ReadOnly] private MapSituation currentSituation;
        private bool _onMap;

        /*public void StartReadingMap(MapSituation situation = MapSituation.Default)
        {
            currentSituation = situation;
            StateController.ChangeAllState(this);
            controller.OpenMap(this);
        }

        public void StartSealMap(Seal boat)
        {
            StateController.ChangeAllState(this);
            controller.OpenSealMap(this, boat);
        }

        public void EndReadingMap()
        {
            controller.CloseMap();

            if (currentSituation == MapSituation.Default)
                StateController.ChangeToDefault();
            else if (currentSituation == MapSituation.OnBoat)
                StateController.ChangeAllState(GetComponent<SailTrigger>());
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
            controller.ConfirmKey();
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            EndReadingMap();
        }

        public override void OnLongInteractCancel(InputAction.CallbackContext ctx)
        {
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
            //Tranforming input in to dir input
            var ret = new Vector2Int();
            if (ctx.ReadValue<Vector2>().x > 0.01) ret.x = 1;
            else if (ctx.ReadValue<Vector2>().x < -0.01) ret.x = -1;
            else ret.x = 0;

            if (ctx.ReadValue<Vector2>().y > 0.01) ret.y = 1;
            else if (ctx.ReadValue<Vector2>().y < -0.01) ret.y = -1;
            else ret.y = 0;

            controller.DirInput(ret);
        }

        public override void EmergentExit()
        {
            EndReadingMap();
        }


        private void MapKey(InputAction.CallbackContext ctx)
        {
            if (_onMap)
            {
                _onMap = false;
                EndReadingMap();
            }
            else
            {
                _onMap = true;
                StartReadingMap();
            }
        }*/

        public override void OnInteractPress(InputAction.CallbackContext ctx)
        {
            
        }

        public override void OnShortGrab(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLongGrab(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void OnInteractCancel(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void OnShortInteract(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLongInteract(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void EmergentExit()
        {
            throw new System.NotImplementedException();
        }
    }
}