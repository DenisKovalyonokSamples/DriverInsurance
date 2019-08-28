using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using DI.Droid.Fragments;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class AboutActivity : BaseActivity
    {
        MenuFragment menuFragment;

        TextView textAboutTitle;
        TextView textDescriptionTitle;
        TextView textDescription;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.About);
            SetTitleBack();

            textAboutTitle = FindViewById<TextView>(Resource.Id.textAboutTitle);
            textDescriptionTitle = FindViewById<TextView>(Resource.Id.textDescriptionTitle);
            textDescription = FindViewById<TextView>(Resource.Id.textDescription);

            menuFragment = new MenuFragment();

            if (!this.IsFinishing)
            {
                var partialMenuSetup = SupportFragmentManager.BeginTransaction();
                partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
                partialMenuSetup.CommitAllowingStateLoss();
            }

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            textAboutTitle.Text = AppResources.AppName.ToUpper();
            textDescriptionTitle.Text = AppResources.Description.ToUpper();
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
            return AppResources.About.ToUpper();
        }

        #endregion
    }
}
