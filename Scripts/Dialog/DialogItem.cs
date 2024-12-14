using System.Collections.Generic;
using Barterta.Craft;

namespace Barterta.Dialog
{
    public class DialogItem
    {
        public CraftRecipe CraftRecipe;
        public string Text;

        public DialogItem(string text)
        {
            Text = text;
        }

        public DialogItem(CraftRecipe recipe)
        {
            CraftRecipe = recipe;
        }

        public static List<DialogItem> StringListToDialogItems(List<string> texts)
        {
            var list = new List<DialogItem>();
            foreach (var text in texts) list.Add(new DialogItem(text));

            return list;
        }
    }
}