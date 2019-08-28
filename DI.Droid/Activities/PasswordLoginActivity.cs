using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Content;
using DI.Shared;
using Android.Views;
using DI.Shared.Entities.API;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using Android.Support.Design.Widget;
using System;
using System.Threading;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class PasswordLoginActivity : BaseFormActivity
    {
        Timer timeoutTimer;
        long timeoutDelay = (long)TimeSpan.FromSeconds(1).TotalMilliseconds;

        Android.Graphics.PaintFlags ResetDefaultStyle;

        string phone;

        TextView textSuchPhoneRegisteredTitle;
        TextView passwordVM;

        TextInputLayout editPassword;
        ProgressBar progressBarCancel;

        RelativeLayout buttonCancel;
        TextView textbuttonCancel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.PasswordLogin);
            SetTitleBack();

            phone = Intent.GetStringExtra(Constants.PHONE);

            editPassword = FindViewById<TextInputLayout>(Resource.Id.editPasswordLayout);
            progressBarCancel = FindViewById<ProgressBar>(Resource.Id.progressBarCancel);
            textSuchPhoneRegisteredTitle = FindViewById<TextView>(Resource.Id.textSuchPhoneRegisteredTitle);
            passwordVM = FindViewById<TextView>(Resource.Id.passwordVM);

            buttonCancel = FindViewById<RelativeLayout>(Resource.Id.buttonCancel);
            textbuttonCancel = FindViewById<TextView>(Resource.Id.textbuttonCancel);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            editPassword.Hint = AppResources.EnterPassword;
            textSuchPhoneRegisteredTitle.Text = AppResources.SuchPhoneRegistered.ToUpper();

            GetSubmitButton().Text = AppResources.Enter.ToUpper();
            textbuttonCancel.Text = AppResources.RestorePassword.ToUpper();
            ResetDefaultStyle = textbuttonCancel.PaintFlags;
            textbuttonCancel.PaintFlags = textbuttonCancel.PaintFlags | Android.Graphics.PaintFlags.UnderlineText;

            PasswordRestoreActivity.ResendSMSTimeout += ActivateTimeoutForSMSResend;

            SetupGestures();
        }

        void SetupGestures()
        {
            buttonCancel.Click += async delegate
            {
                ShowProgressBar();

                await OnCancelAsync();

                HideProgressBar();
            };
        }

        int timeout = 0;
        void ActivateTimeoutForSMSResend()
        {
            textbuttonCancel.Text = AppResources.PasswordRecoveryTimeoutMessage + " 01:00";
            textbuttonCancel.PaintFlags = ResetDefaultStyle;

            timeoutTimer = new System.Threading.Timer(new TimerCallback(TimeoutHandler), null, timeoutDelay, Timeout.Infinite);
        }

        public void TimeoutHandler(object o)
        {
            if (textbuttonCancel.Text == AppResources.PasswordRecoveryTimeoutMessage +  " 00:00")
            {

                this.RunOnUiThread(() =>
                {
                    textbuttonCancel.Text = AppResources.RestorePassword.ToUpper();
                    textbuttonCancel.PaintFlags = textbuttonCancel.PaintFlags | Android.Graphics.PaintFlags.UnderlineText;
                });

                timeout = 0;
                timeoutTimer.Dispose();
            }
            else
            {
                timeout++;
                int value = 60 - timeout;

                this.RunOnUiThread(() =>
                {
                    if (value > 9)
                    {
                        textbuttonCancel.Text = AppResources.PasswordRecoveryTimeoutMessage + " 00:" + value.ToString();
                    }
                    else
                    {
                        textbuttonCancel.Text = AppResources.PasswordRecoveryTimeoutMessage + " 00:0" + value.ToString();
                    }
                });
            }

            timeoutTimer.Change(timeoutDelay, Timeout.Infinite);

            return;
        }

        #region abstract

        protected override int GetStatusBarColor()
        {
            return Resource.Color.statusbar_blue;
        }

        protected override int GetActiveBarColor()
        {
            return Resource.Color.actionbar_blue;
        }

        protected override string GetTitle()
        {
            return AppResources.AppName;
        }

        protected override async Task LoadDataAsync()
        {

        }

        protected override Button GetCancelButton()
        {
            return null;
        }

        protected override Button GetSubmitButton()
        {
            return FindViewById<Button>(Resource.Id.buttonSubmit);
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task OnCancelAsync()
        {
            if (textbuttonCancel.Text == AppResources.RestorePassword.ToUpper())
            {
                buttonCancel.Visibility = ViewStates.Gone;
                GetSubmitButton().Visibility = ViewStates.Gone;
                progressBarCancel.Visibility = ViewStates.Visible;
                _progressBar.Visibility = ViewStates.Gone;

                var data = await APIDataManager.SendSMSForPasswordReset(phone.Trim());
                if (data != null && data.Success == true)
                {
                    var activity = new Intent(this, typeof(PasswordRestoreActivity));
                    activity.PutExtra(Constants.PHONE, phone.Trim());
                    StartActivity(activity);
                }
                else
                {
                    Toast.MakeText(this, AppResources.ErrorMessage, ToastLength.Long).Show();
                }

                progressBarCancel.Visibility = ViewStates.Gone;
                buttonCancel.Visibility = ViewStates.Visible;
                GetSubmitButton().Visibility = ViewStates.Visible;
            }
        }

        protected override async Task OnSubmitAsync()
        {
            SessionManager.UserData = await APIDataManager.GetUserByName("user" + phone.Replace("+", "").Trim());
            if (SessionManager.UserData != null && SessionManager.UserData.CompanyId.HasValue)
            {
                SessionManager.СontractorData = await APIDataManager.GetCompany(SessionManager.UserData.CompanyId.Value);
                if (SessionManager.СontractorData != null)
                {
                    SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                }
            }

            var activity = new Intent(this, typeof(MainActivity));
            StartActivity(activity);

            Finish();
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (editPassword.EditText.Text == string.Empty)
            {
                passwordVM.Text = AppResources.PasswordRequired.ToUpper();
                passwordVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                passwordVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            TokenResponseModel token = await APIDataManager.Login("user" + phone.Replace("+", "").Trim(), editPassword.EditText.Text);
            if (token != null)
            {
                passwordVM.Visibility = ViewStates.Invisible;

                sqliteManager.Register(phone, token.Token, "user" + phone.Replace("+", "").Trim());
            }
            else
            {
                passwordVM.Text = AppResources.WrongPassword.ToUpper();
                passwordVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }

            if (hasErrors)
                return false;

            return true;
        }

        #endregion
    }
}
