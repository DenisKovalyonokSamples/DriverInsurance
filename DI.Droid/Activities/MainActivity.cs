using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Droid;
using DI.Localization;
using Android.Content.PM;
using DI.Droid.Fragments;
using DI.Shared.Enums;
using Android.Content;
using DI.Droid.Helpers;
using DI.Shared.Managers;
using System.Threading.Tasks;
using DI.Shared;
using DI.Droid.Services;
using DI.Shared.Helpers;
using DI.Shared.DataAccess;
using DI.Shared.Entities.SQL;
using Android.Gms.Common.Apis;
using static Android.Support.V4.App.ActivityCompat;
using Android;
using Android.Gms.Location;
using Android.Gms.Common;
using Android.Views;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class MainActivity : BaseActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, IResultCallback, IOnRequestPermissionsResultCallback
    {
        MenuFragment menuFragment;
        TrackDataFragment trackDataFragment;
        PoliciesFragment policiesFragment;
        BonusesFragment bonusesFragment;
        ProfileFragment profileFragment;
        UsefullFragment usefullFragment;
        SystemMessagesFragment systemMessagesFragment;

        public bool isInit = true;
        public PartialType ActiveTab = PartialType.CurrentRating;
        

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if (ActiveTab == PartialType.Usefull)
            {
                if (IsTouchMovesLocked)
                {
                    if (e.Action == MotionEventActions.Move)
                        return false;
                }
            }
            
            return base.DispatchTouchEvent(e);
        }

        public bool IsTouchMovesLocked = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(DI.Droid.Resource.Layout.Main);

            menuFragment = new MenuFragment();

            if (!this.IsFinishing)
            {
                var partialMenuSetup = SupportFragmentManager.BeginTransaction();
                partialMenuSetup.Add(DI.Droid.Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
                partialMenuSetup.CommitAllowingStateLoss();
            }

            if (!this.IsFinishing)
            {
                trackDataFragment = new TrackDataFragment();
                policiesFragment = new PoliciesFragment();
                bonusesFragment = new BonusesFragment();
                profileFragment = new ProfileFragment();
                usefullFragment = new UsefullFragment();
                systemMessagesFragment = new SystemMessagesFragment();

                var partialSetup = SupportFragmentManager.BeginTransaction();
                partialSetup.Add(DI.Droid.Resource.Id.fragmentContainer, systemMessagesFragment, "SystemMessagesFragment");
                partialSetup.Hide(systemMessagesFragment);
                partialSetup.Add(DI.Droid.Resource.Id.fragmentContainer, usefullFragment, "UsefullFragment");
                partialSetup.Hide(usefullFragment);
                partialSetup.Add(DI.Droid.Resource.Id.fragmentContainer, profileFragment, "ProfileFragment");
                partialSetup.Hide(profileFragment);
                partialSetup.Add(DI.Droid.Resource.Id.fragmentContainer, bonusesFragment, "BonusesFragment");
                partialSetup.Hide(bonusesFragment);
                partialSetup.Add(DI.Droid.Resource.Id.fragmentContainer, policiesFragment, "PoliciesFragment");
                partialSetup.Hide(policiesFragment);
                partialSetup.Add(DI.Droid.Resource.Id.fragmentContainer, trackDataFragment, "TrackDataFragment");

                partialSetup.CommitAllowingStateLoss();
            }
            isInit = true;

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            StartSyncService();

            if (SessionManager.СontractData == null)
            {
                ConnectToGooglePlayServices();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (SessionManager.ShowPartialOnMain != PartialType.Unknown)
            {
                menuFragment.SwitchSelectedItem(SessionManager.ShowPartialOnMain);
                SessionManager.ShowPartialOnMain = PartialType.Unknown;
            }

            if (!IsGooglePlayServicesInstalled())
            {
                ShowInstallGooglePlayServicesDialog();
            }
        }

        async Task StartSyncService()
        {
            await SmoochHepler.SyncSmooch(sqliteManager);

            if (AppWrapper.ServiceIntent == null)
            {
                AppWrapper.ServiceIntent = new Intent(this, typeof(SynchronizationService));
                StartService(AppWrapper.ServiceIntent);
            }
        }

        #region Menu

        public void SetMenuSelection(PartialType type)
        {
            if (menuFragment != null)
            {
                menuFragment.SwitchSelectedItem(type, false);
            }
        }

        public void SetupFragment(PartialType type)
        {
            var partialSetup = SupportFragmentManager.BeginTransaction();

            if (type != PartialType.Support)
            {
                HideMenuFragments(partialSetup, type);
                partialSetup.AddToBackStack(null);
            }

            switch (type)
            {
                case (PartialType.SystemMessages):
                    if (!isInit)
                        systemMessagesFragment = new SystemMessagesFragment();
                    partialSetup.Replace(DI.Droid.Resource.Id.fragmentContainer, systemMessagesFragment, "SystemMessagesFragment");
                    partialSetup.Show(systemMessagesFragment);
                    SetTitleText(AppResources.Main);
                    break;
                case (PartialType.CurrentRating):
                    if (!isInit)
                        trackDataFragment = new TrackDataFragment();
                    partialSetup.Replace(DI.Droid.Resource.Id.fragmentContainer, trackDataFragment, "TrackDataFragment");
                    partialSetup.Show(trackDataFragment);
                    SetTitleText(AppResources.Main);
                    break;
                case (PartialType.Policies):
                    if (!isInit)
                        policiesFragment = new PoliciesFragment();
                    partialSetup.Replace(DI.Droid.Resource.Id.fragmentContainer, policiesFragment, "PoliciesFragment");
                    partialSetup.Show(policiesFragment);
                    SetTitleText(AppResources.Policies);
                    break;
                case (PartialType.Bonuses):
                    if (!isInit)
                        bonusesFragment = new BonusesFragment();
                    partialSetup.Replace(DI.Droid.Resource.Id.fragmentContainer, bonusesFragment, "BonusesFragment");
                    partialSetup.Show(bonusesFragment);
                    SetTitleText(AppResources.Bonuses);
                    break;
                case (PartialType.Usefull):
                    if (!isInit)
                        usefullFragment = new UsefullFragment();
                    partialSetup.Replace(DI.Droid.Resource.Id.fragmentContainer, usefullFragment, "UsefullFragment");
                    partialSetup.Show(usefullFragment);
                    SetTitleText(AppResources.Usefull);
                    break;
                case (PartialType.Profile):
                    if (!isInit)
                        profileFragment = new ProfileFragment();
                    partialSetup.Replace(DI.Droid.Resource.Id.fragmentContainer, profileFragment, "ProfileFragment");
                    partialSetup.Show(profileFragment);
                    SetTitleText(AppResources.Profile);
                    break;
            }

            if (type != PartialType.Support)
            {
                if (!this.IsFinishing)
                {
                    partialSetup.CommitAllowingStateLoss();
                }
            }
        }

        void HideMenuFragments(Android.Support.V4.App.FragmentTransaction partialSetup, PartialType activeFragment)
        {
            if (PartialType.SystemMessages != activeFragment)
                partialSetup.Hide(systemMessagesFragment);
            if (PartialType.CurrentRating != activeFragment)
            {
                partialSetup.Hide(trackDataFragment);
            }
            if (PartialType.Policies != activeFragment)
                partialSetup.Hide(policiesFragment);
            if (PartialType.Bonuses != activeFragment)
                partialSetup.Hide(bonusesFragment);
            if (PartialType.Profile != activeFragment)
                partialSetup.Hide(profileFragment);
        }

        #endregion

        #region Location Request

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            if (_apiClient != null)
            {
                if (CheckCallingOrSelfPermission(Manifest.Permission.AccessFineLocation) == Permission.Granted &&
                    CheckCallingOrSelfPermission(Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
                {
                    LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder().AddLocationRequest(_locRequest);
                    var result = LocationServices.SettingsApi.CheckLocationSettings(_apiClient, builder.Build());
                    result.SetResultCallback(this);
                }
            }
        }

        LocationRequest _locRequest;
        GoogleApiClient _apiClient;

        public void ConnectToGooglePlayServices()
        {
            if (IsGooglePlayServicesInstalled())
            {
                if (_apiClient == null)
                    _apiClient = new GoogleApiClient.Builder(this)
                        .AddApi(LocationServices.API)
                        .AddConnectionCallbacks(this)
                        .AddOnConnectionFailedListener(this)
                        .Build();

                if (!_apiClient.IsConnected)
                    _apiClient.Connect();
            }
            else
            {
                ShowInstallGooglePlayServicesDialog();
            }
        }

        void BuildLocationRequest()
        {
            _locRequest = new LocationRequest();
            _locRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            _locRequest.SetFastestInterval(1 * 1000);
            _locRequest.SetInterval(2 * 1000);

            int sdk = (int)Android.OS.Build.VERSION.SdkInt;
            if (sdk < 23 ||
                (this.CheckCallingOrSelfPermission(Android.Manifest.Permission.AccessFineLocation) == Permission.Granted &&
                    this.CheckCallingOrSelfPermission(Android.Manifest.Permission.AccessCoarseLocation) == Permission.Granted))
            {
                LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder().AddLocationRequest(_locRequest);
                builder.SetAlwaysShow(true);

                var result = LocationServices.SettingsApi.CheckLocationSettings(_apiClient, builder.Build());
                result.SetResultCallback(this);
            }
            else
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new System.String[] { Android.Manifest.Permission.AccessFineLocation, Android.Manifest.Permission.AccessCoarseLocation }, 1);
            }
        }

        void ShowInstallGooglePlayServicesDialog()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Google Play Services");
            alert.SetMessage(AppResources.GooglePlayServicesNotInstalledMessage);
            alert.SetPositiveButton("OK", (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }

        bool IsGooglePlayServicesInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
            }
            return false;
        }

        public void OnConnected(Bundle bundle)
        {
            BuildLocationRequest();
        }

        public void OnDisconnected()
        {
        }

        public void OnConnectionFailed(ConnectionResult bundle)
        {
        }

        public void OnConnectionSuspended(int i)
        {
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == requestCheckSettings)
            {
                if (resultCode == Result.Ok)
                {
                    //RequestLocationUpdates();
                }
                else
                {
                    Toast.MakeText(this, AppResources.EnableGPS, ToastLength.Long).Show();
                }
            }
        }

        const int requestCheckSettings = 2002;
        public void OnResult(Java.Lang.Object result)
        {
            var locationSettingsResult = result as LocationSettingsResult;

            Statuses status = locationSettingsResult.Status;
            switch (status.StatusCode)
            {
                case CommonStatusCodes.Success:
                    break;
                case CommonStatusCodes.ResolutionRequired:
                    try
                    {
                        status.StartResolutionForResult(this, requestCheckSettings);
                    }
                    catch (IntentSender.SendIntentException)
                    {
                    }
                    break;
                case LocationSettingsStatusCodes.SettingsChangeUnavailable:
                    break;
            }
        }

        #endregion

        #region abstract

        protected override int GetStatusBarColor()
        {
            return DI.Droid.Resource.Color.statusbar_blue;
        }

        protected override int GetActiveBarColor()
        {
            return DI.Droid.Resource.Color.actionbar_blue;
        }

        protected override string GetTitle()
        {
            return AppResources.Main.ToUpper();
        }

        #endregion
    }
}

