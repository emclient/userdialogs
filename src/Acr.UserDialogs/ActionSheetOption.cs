using System;


namespace Acr.UserDialogs
{

    public class ActionSheetOption
    {

        public string Text { get; set; }
        public Action Action { get; set; }
        public string ItemIcon { get; set; }
        public uint? IconTint { get; set; }
        public object Tag { get; set; }


        public ActionSheetOption(string text, Action action = null, string icon = null, uint? iconTint = null, object tag = null)
        {
            this.Text = text;
            this.Action = action;
            this.ItemIcon = icon;
            this.IconTint = iconTint;
            this.Tag = tag;
        }
    }
}
