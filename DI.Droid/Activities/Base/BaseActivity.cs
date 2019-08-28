using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using Android.Graphics.Drawables;
using DI.Localization;
using Android.Gms.Common;

namespace DI.Droid.Base
{
    public abstract class BaseActivity : AppCompatActivity
    {
        protected VMManager vmManager;
        protected SQLDataManager sqliteManager;

        protected Android.Support.V7.Widget.Toolbar _toolbar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            vmManager = new VMManager();
            sqliteManager = SessionManager.DBConnection; 
        }

        protected virtual void InitControls()
        {
            SetTitle();
        }

        protected virtual void SetTitle()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(new Color(ContextCompat.GetColor(this, GetStatusBarColor())));
            }

            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbar.Title = GetTitle();
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetBackgroundDrawable(new ColorDrawable(new Color(ContextCompat.GetColor(this, GetActiveBarColor()))));
        }

        protected virtual void SetTitleText(string value)
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _toolbar.Title = value;
        }

        protected virtual void SetTitleBack()
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(ShowHomeButton);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public virtual bool ShowHomeButton
        {
            get { return true; }
        }

        protected abstract string GetTitle();

        protected abstract int GetStatusBarColor();

        protected abstract int GetActiveBarColor();

        #region Dialogs 

        public void ShowOperationFailedDialog()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(AppResources.Error);
            alert.SetMessage(AppResources.ErrorMessage);
            alert.SetPositiveButton("OK", (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }

        protected void ShowInstallGooglePlayServicesDialog()
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

        #endregion

        protected bool IsGooglePlayServicesInstalled()
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
    }
}
