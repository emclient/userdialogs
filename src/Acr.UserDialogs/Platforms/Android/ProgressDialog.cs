using System;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;


namespace Acr.UserDialogs
{
    public class ProgressDialog : IProgressDialog
    {
        readonly Activity activity;
        readonly ProgressDialogConfig config;


        public ProgressDialog(ProgressDialogConfig config, Activity activity)
        {
            this.config = config;
            this.activity = activity;
        }

        #region IProgressDialog Members

        string title;
        public virtual string Title
        {
            get { return this.title; }
            set
            {
                if (this.title == value)
                    return;

                this.title = value;
                this.Refresh();
            }
        }


        int percentComplete;
        public virtual int PercentComplete
        {
            get { return this.percentComplete; }
            set
            {
                if (this.percentComplete == value)
                    return;

                if (value > 100)
                    this.percentComplete = 100;
                else if (value < 0)
                    this.percentComplete = 0;
                else
                    this.percentComplete = value;

                this.Refresh();
            }
        }


        public virtual bool IsShowing { get; private set; }


        public virtual void Show()
        {
            if (this.IsShowing)
                return;

            this.IsShowing = true;
            this.Refresh();
        }


        public virtual void Hide()
        {
            this.IsShowing = false;
            try
            {
                AndHUD.Shared.Dismiss(activity);
            }
            catch(Exception exc)
            {
                Infrastructure.Log.Error("Dismiss", $"Exception ({exc.GetType().FullName}) occured while dismissing dialog: {exc.Message}");
            }
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            this.Hide();
        }

        #endregion

        #region Internals

        protected virtual void Refresh()
        {
            if (!this.IsShowing)
                return;

            var p = -1;
            var txt = this.Title;
            if (this.config.IsDeterministic)
            {
                p = this.PercentComplete;
                if (!String.IsNullOrWhiteSpace(txt))
                    txt += "\n";

                txt += p + "%\n";
            }

            AndHUD.Shared.Show(
                this.activity,
                txt,
                p,
                this.config.MaskType.ToNative(),
                null,
                this.config.OnCancel is not null ? this.OnCancelClick : null,
                true,
                null,
                this.BeforeShow,
                this.AfterShow
            );
        }

        private void BeforeShow(Dialog dialog)
        {
            if (dialog == null)
                return;
            dialog.Window.AddFlags(WindowManagerFlags.NotFocusable);
            var textViewId = dialog.FindViewById(AndroidHUD.Resource.Id.textViewStatus);
            var layout = (RelativeLayout)textViewId.Parent;
            layout.SetMinimumWidth((int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 150, dialog.Context.Resources.DisplayMetrics));

            if (config.OnCancel is not null)
            {
                var _params = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                _params.AddRule(LayoutRules.Below, AndroidHUD.Resource.Id.textViewStatus);
                _params.AddRule(LayoutRules.CenterHorizontal);

                Button button;
                if (config.CancelButtonAndroidStyleId is int styleId)
                    button = new Button(new ContextThemeWrapper(dialog.Context, styleId));
                else
                    button = new Button(dialog.Context);

                button.Text = config.CancelText;
                button.Click += (s, e) => OnCancelClick();
                layout.AddView(button, _params);
            }
        }

        private void AfterShow(Dialog dialog)
        {
            if (dialog == null)
                return;

            //Maintain Immersive mode
            if (ProgressDialogConfig.UseAndroidImmersiveMode)
            {
                dialog.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                                                        SystemUiFlags.ImmersiveSticky |
                                                        SystemUiFlags.LayoutStable |
                                                        SystemUiFlags.LayoutFullscreen |
                                                        SystemUiFlags.LayoutHideNavigation |
                                                        SystemUiFlags.HideNavigation |
                                                        SystemUiFlags.Fullscreen);
            }

            dialog.Window.ClearFlags(WindowManagerFlags.NotFocusable);
        }

        void OnCancelClick()
        {
            if (this.config.OnCancel == null)
                return;

            this.Hide();
            this.config.OnCancel();
        }

        #endregion
    }
}
