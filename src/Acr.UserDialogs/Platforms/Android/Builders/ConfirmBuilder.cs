using System;
using Android.App;
using AlertDialog = Android.App.AlertDialog;
using AndroidX.AppCompat.App;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;


namespace Acr.UserDialogs.Builders
{
    public class ConfirmBuilder : IAlertDialogBuilder<ConfirmConfig>
    {
        public Dialog Build(Activity activity, ConfirmConfig config)
        {
            var builder = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false));
            if (!string.IsNullOrWhiteSpace(config.NeutralText))
                builder.SetNeutralButton(config.NeutralText, (s, a) => config.OnAction(null));


            return builder.Create();
        }


        public Dialog Build(AppCompatActivity activity, ConfirmConfig config)
        {
            var builder = new AppCompatAlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false));
            if (!string.IsNullOrWhiteSpace(config.NeutralText))
                builder.SetNeutralButton(config.NeutralText, (s, a) => config.OnAction(null));

            return builder.Create();
        }
    }
}
