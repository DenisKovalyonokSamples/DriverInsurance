using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using DI.Droid.Fragments;
using Android.Content;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class ContactsActivity : BaseActivity
    {
        MenuFragment menuFragment;

        TextView textPhonesTitle;

        TextView localCallDescription;
        TextView localCallNumber;
        TextView globalCallDescription;
        TextView globalCallNumber;
        TextView textDescriptionTitle;
        TextView textDIDescriptionOne;
        TextView textDIDescriptionTwo;
        TextView textDIDescriptionThree;

        ImageView imageViewVK;
        ImageView imageViewFB;

        RelativeLayout layoutLocalCall;
        RelativeLayout layoutGlobalCall;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Contacts);
            SetTitleBack();

            textPhonesTitle = FindViewById<TextView>(Resource.Id.textPhonesTitle);

            localCallDescription = FindViewById<TextView>(Resource.Id.localCallDescription);
            localCallNumber = FindViewById<TextView>(Resource.Id.localCallNumber);
            globalCallDescription = FindViewById<TextView>(Resource.Id.globalCallDescription);
            globalCallNumber = FindViewById<TextView>(Resource.Id.globalCallNumber);
            textDescriptionTitle = FindViewById<TextView>(Resource.Id.textDescriptionTitle);
            textDIDescriptionOne = FindViewById<TextView>(Resource.Id.textDIDescriptionOne);
            textDIDescriptionTwo = FindViewById<TextView>(Resource.Id.textDIDescriptionTwo);
            textDIDescriptionThree = FindViewById<TextView>(Resource.Id.textDIDescriptionThree);

            imageViewVK = FindViewById<ImageView>(Resource.Id.imageViewVK);
            imageViewFB = FindViewById<ImageView>(Resource.Id.imageViewFB);

            layoutLocalCall = FindViewById<RelativeLayout>(Resource.Id.layoutLocalCall);
            layoutGlobalCall = FindViewById<RelativeLayout>(Resource.Id.layoutGlobalCall);

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

            textPhonesTitle.Text = AppResources.Phones.ToUpper();
            localCallDescription.Text = AppResources.ForCallsFromRussia;
            localCallNumber.Text = string.Empty;
            globalCallDescription.Text = AppResources.ForCallsFromAnotherCountries;
            globalCallNumber.Text = string.Empty;
            textDescriptionTitle.Text = AppResources.Description.ToUpper();
            textDIDescriptionOne.Text = AppResources.AppDescriptionOne;
            textDIDescriptionTwo.Text = AppResources.AppDescriptionTwo;
            textDIDescriptionThree.Text = AppResources.AppDescriptionThree;

            SetupGestures();
        }

        void SetupGestures()
        {
            layoutLocalCall.Click += delegate {
                var uri = Android.Net.Uri.Parse("tel:+375");
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            };
            layoutGlobalCall.Click += delegate {
                var uri = Android.Net.Uri.Parse("tel:+375");
                var intent = new Intent(Intent.ActionDial, uri);
                StartActivity(intent);
            };

            imageViewVK.Click += delegate
            {
                //TODO: Add your link
            };
            imageViewFB.Click += delegate
            {
                //TODO: Add your link
            };
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
            return AppResources.Contacts;
        }

        #endregion
    }
}
