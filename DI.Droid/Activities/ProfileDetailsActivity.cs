using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using Android.Views;
using DI.Localization;
using Android.Content.PM;
using DI.Droid.Fragments;
using DI.Shared.Enums;
using Android.Graphics;
using Android.Support.V4.Content;
using DI.Shared.Managers;
using DI.Shared.DataAccess;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class ProfileDetailsActivity : BaseActivity
    {
        MenuFragment menuFragment;
        DriverInfoFragment driverInfoFragment;
        CarFragment carFragment;
        TelematicsFragment telematicsFragment;

        LinearLayout layoutPolicyTabs;
        Button buttonDriverInfoTab;
        Button buttonCarsTab;
        Button buttonTelematicsTab;
        LinearLayout layoutFullTabSelectors;
        LinearLayout selectorDriverTab;
        LinearLayout selectorCarTab;
        LinearLayout selectorTelematicsTab;

        LinearLayout layoutWithoutPolicyTabs;
        Button buttonWithoutDriverTab;
        Button buttonWithoutCarsTab;
        LinearLayout layoutWithoutTabSelectors;
        LinearLayout selectorWithoutDriverTab;
        LinearLayout selectorWithoutCarTab;

        Color greenColor;
        Color blueColor;

        ProgressBar progressBarLoading;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ProfileDetails);
            SetTitleBack();

            progressBarLoading = FindViewById<ProgressBar>(Resource.Id.progressBarLoading);

            layoutPolicyTabs = FindViewById<LinearLayout>(Resource.Id.layoutPolicyTabs);
            buttonDriverInfoTab = FindViewById<Button>(Resource.Id.buttonDriverTab);
            buttonCarsTab = FindViewById<Button>(Resource.Id.buttonCarsTab);
            buttonTelematicsTab = FindViewById<Button>(Resource.Id.buttonTelematicsTab);

            layoutFullTabSelectors = FindViewById<LinearLayout>(Resource.Id.layoutFullTabSelectors);
            selectorDriverTab = FindViewById<LinearLayout>(Resource.Id.selectorDriverTab);
            selectorCarTab = FindViewById<LinearLayout>(Resource.Id.selectorCarTab);
            selectorTelematicsTab = FindViewById<LinearLayout>(Resource.Id.selectorTelematicsTab);

            layoutWithoutPolicyTabs = FindViewById<LinearLayout>(Resource.Id.layoutWithoutPolicyTabs);
            buttonWithoutDriverTab = FindViewById<Button>(Resource.Id.buttonWithoutDriverTab);
            buttonWithoutCarsTab = FindViewById<Button>(Resource.Id.buttonWithoutCarsTab);

            layoutWithoutTabSelectors = FindViewById<LinearLayout>(Resource.Id.layoutWithoutTabSelectors);
            selectorWithoutDriverTab = FindViewById<LinearLayout>(Resource.Id.selectorWithoutDriverTab);
            selectorWithoutCarTab = FindViewById<LinearLayout>(Resource.Id.selectorWithoutCarTab);

            greenColor = new Color(ContextCompat.GetColor(this, Resource.Color.content_green));
            blueColor = new Color(ContextCompat.GetColor(this, Resource.Color.actionbar_blue));            

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

            buttonDriverInfoTab.Text = AppResources.Driver.ToUpper();
            buttonCarsTab.Text = AppResources.Car.ToUpper();
            buttonTelematicsTab.Text = AppResources.Telematics.ToUpper();

            buttonWithoutDriverTab.Text = AppResources.Driver.ToUpper();
            buttonWithoutCarsTab.Text = AppResources.Car.ToUpper();

            SetupGestures();

            SetupFragments();
        }

        async void SetupFragments()
        {
            progressBarLoading.Visibility = ViewStates.Visible;

            if (SessionManager.СontractorData != null)
            {
                //Check current contract
                var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                if (contract != null)
                {
                    progressBarLoading.Visibility = ViewStates.Gone;
                    layoutPolicyTabs.Visibility = ViewStates.Visible;
                    layoutFullTabSelectors.Visibility = ViewStates.Visible;

                    driverInfoFragment = new DriverInfoFragment();
                    carFragment = new CarFragment();
                    telematicsFragment = new TelematicsFragment();

                    if (!this.IsFinishing)
                    {
                        var partialSetup = SupportFragmentManager.BeginTransaction();
                        partialSetup.Add(Resource.Id.fragmentContainer, telematicsFragment, "TelematicsFragment");
                        partialSetup.Hide(telematicsFragment);
                        partialSetup.Add(Resource.Id.fragmentContainer, carFragment, "CarFragment");
                        partialSetup.Hide(carFragment);
                        partialSetup.Add(Resource.Id.fragmentContainer, driverInfoFragment, "DriverInfoFragment");
                        partialSetup.CommitAllowingStateLoss();
                    }
                }
                else
                {
                    progressBarLoading.Visibility = ViewStates.Gone;
                    layoutWithoutPolicyTabs.Visibility = ViewStates.Visible;
                    layoutWithoutTabSelectors.Visibility = ViewStates.Visible;

                    driverInfoFragment = new DriverInfoFragment();
                    carFragment = new CarFragment();

                    if (!this.IsFinishing)
                    {
                        var partialSetup = SupportFragmentManager.BeginTransaction();
                        partialSetup.Add(Resource.Id.fragmentContainer, carFragment, "CarFragment");
                        partialSetup.Hide(carFragment);
                        partialSetup.Add(Resource.Id.fragmentContainer, driverInfoFragment, "DriverInfoFragment");
                        partialSetup.CommitAllowingStateLoss();
                    }
                }
            }
        }

        void SetupGestures()
        {
            buttonDriverInfoTab.Click += delegate
            {
                selectorDriverTab.Visibility = ViewStates.Visible;
                selectorCarTab.Visibility = ViewStates.Invisible;
                selectorTelematicsTab.Visibility = ViewStates.Invisible;

                ShowFragment(PartialType.DriverInfo);
            };

            buttonCarsTab.Click += delegate
            {
                selectorDriverTab.Visibility = ViewStates.Invisible;
                selectorCarTab.Visibility = ViewStates.Visible;
                selectorTelematicsTab.Visibility = ViewStates.Invisible;

                ShowFragment(PartialType.Cars);
            };

            buttonTelematicsTab.Click += delegate
            {
                selectorDriverTab.Visibility = ViewStates.Invisible;
                selectorCarTab.Visibility = ViewStates.Invisible;
                selectorTelematicsTab.Visibility = ViewStates.Visible;

                ShowFragment(PartialType.Telematics);
            };

            buttonWithoutDriverTab.Click += delegate
            {
                selectorWithoutDriverTab.Visibility = ViewStates.Visible;
                selectorWithoutCarTab.Visibility = ViewStates.Invisible;

                ShowFragment(PartialType.DriverInfo, false);
            };

            buttonWithoutCarsTab.Click += delegate
            {
                selectorWithoutDriverTab.Visibility = ViewStates.Invisible;
                selectorWithoutCarTab.Visibility = ViewStates.Visible;

                ShowFragment(PartialType.Cars, false);
            };
        }

        private void ShowFragment(PartialType type, bool withTelematics = true)
        {
            var partialSetup = SupportFragmentManager.BeginTransaction();

            if (type == PartialType.DriverInfo)
            {
                partialSetup.Hide(carFragment);
                if (withTelematics)
                {
                    partialSetup.Hide(telematicsFragment);
                }
                partialSetup.AddToBackStack(null);

                driverInfoFragment = new DriverInfoFragment();
                partialSetup.Replace(Resource.Id.fragmentContainer, driverInfoFragment, "DriverInfoFragment");
                partialSetup.Show(driverInfoFragment);
            }
            if (type == PartialType.Cars)
            {
                partialSetup.Hide(driverInfoFragment);
                if (withTelematics)
                {
                    partialSetup.Hide(telematicsFragment);
                }
                partialSetup.AddToBackStack(null);

                carFragment = new CarFragment();
                partialSetup.Replace(Resource.Id.fragmentContainer, carFragment, "CarFragment");
                partialSetup.Show(carFragment);
            }
            if (type == PartialType.Telematics)
            {
                partialSetup.Hide(driverInfoFragment);
                partialSetup.Hide(carFragment);
                partialSetup.AddToBackStack(null);

                telematicsFragment = new TelematicsFragment();
                partialSetup.Replace(Resource.Id.fragmentContainer, telematicsFragment, "TelematicsFragment");
                partialSetup.Show(telematicsFragment);
            }

            if (!this.IsFinishing)
            {
                partialSetup.CommitAllowingStateLoss();
            }
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
            return AppResources.MyProfile;
        }

        #endregion
    }
}
