using Android.App;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using DI.Shared;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class PolicyDetailsActivity : BaseWebViewActivity
    {
        string webUrl;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.PolicyDetails);
            SetTitleBack();

            webUrl = Intent.GetStringExtra(Constants.WEBURL);

            InitControls();
        }

        void InitControls()
        {
            Url = webUrl;

            base.InitControls();
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
            return AppResources.PolicyDetails.ToUpper();
        }

        #endregion
    }
}
