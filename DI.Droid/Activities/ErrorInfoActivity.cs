using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using DI.Droid.Base;
using DI.Localization;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class ErrorInfoActivity : BaseActivity
    {
        TextView ErrorMessage;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ErrorInfo);

            ErrorMessage = FindViewById<TextView>(Resource.Id.textError);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            ErrorMessage.Text = AppResources.NoServerConnectionMessage;
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
            return AppResources.Error;
        }

        #endregion
    }
}
