using System.Collections.Generic;
using System.Linq;
using Barterta.Dialog;
using Barterta.NPC;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using DG.Tweening;
using Febucci.UI;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barterta.InputTrigger
{
    public class DialogTrigger : InputTriggerBase
    {
        private Transform _talkTo;
        private DialogObject _selfTalkDialogObject;
        
        public void StartConversation(string id, Transform talkTo)
        {
            _talkTo = talkTo;
            DialogueManager.StartConversation(id, transform, talkTo);
        }
        
        public void OnDialogStart()
        {
            if(_talkTo) _talkTo.GetComponent<NPCBase>()?.Freeze();
            StateController.ChangeAllState(this);
        }

        public void OnDialogEnd()
        {
            //Log
            Debug.Log("Dialog End");
            if (_talkTo)
                _talkTo.GetComponent<NPCBase>()?.UnFreeze();
            StateController.ChangeToDefault();
        }

        public void SelfBark(string id)
        {
            var text = Methods.GetLocalText(id);

            if (!_selfTalkDialogObject)
            {
                _selfTalkDialogObject = (DialogObject)WorldUIManager.I.GenerateUI(WorldUIManager.I.dialogUIPrefab, transform, 1f);
                WorldUIManager.ChangeUILayer(_selfTalkDialogObject.gameObject, GetComponent<Mark.Mark>());
                WorldUIManager.I.AppealUI(_selfTalkDialogObject).AppendInterval(2f)
                    .Append(WorldUIManager.I.DestroyUI(_selfTalkDialogObject));
                _selfTalkDialogObject.ChangeText(text);
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
            //DialogueManager.StopConversation();
        }

        public override void OnDirection(InputAction.CallbackContext ctx)
        {
        }

        public override void EmergentExit()
        {
        }
    }
}