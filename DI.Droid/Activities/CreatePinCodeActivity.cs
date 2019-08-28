using Android.App;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using DI.Droid.Fragments;
using Android.Content;
using DI.Shared;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class CreatePinCodeActivity : BaseActivity
    {
        PasscodeFragment passcodeFragment;

        string phone;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CreatePinCode);

            phone = Intent.GetStringExtra(Constants.PHONE);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            passcodeFragment.IsRegistration = true;

            SetupGestures();
        }

        void SetupGestures()
        {
        }

        public void ProceedPINCode(string value)
        {
            var activity = new Intent(this, typeof(BonusAccrualActivity));
            StartActivity(activity);

            Finish();
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
            return AppResources.CreatePINCode.ToUpper();
        }

        #endregion
    }
}
