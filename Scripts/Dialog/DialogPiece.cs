using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Barterta.Dialog
{
    public class DialogPiece
    {
        public Dictionary<DialogItem, Transform> Dialogs;
        public int Index = 0;
        
        public DialogItem CurrentDialogItem => Dialogs.Keys.ToList()[Index];

        public void MoveOn()
        {
            //if the dialog is not finished
            if (Index < Dialogs.Count - 1)
            {
                Index++;
            }
        }
    }
}