using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Content;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using Android.Support.V7.App;
using Firebase.Analytics;
using DI.Shared;
using System.Threading.Tasks;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, Icon = "@mipmap/icon",
    Theme = "@style/StartupTheme", LaunchMode = LaunchMode.SingleInstance,
    ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class AccountActivity : AppCompatActivity
    {
        FirebaseAnalytics firebaseAnalytics;
        SQLDataManager sqliteManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HockeyApp.Android.Metrics.MetricsManager.Register(Application, Constants.HOCKEY_APP_ANDROID);
            HockeyApp.Android.CrashManager.Register(this, Constants.HOCKEY_APP_ANDROID);
            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs excArgs) =>
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("UnobservedTaskException: Message - " + excArgs.Exception.Message + " InnerException - " + excArgs.Exception.InnerException.Message);

                excArgs.SetObserved();
            };

            firebaseAnalytics = FirebaseAnalytics.GetInstance(this);
            sqliteManager = SessionManager.DBConnection;

            InitControls();
        }

        void InitControls()
        {         
            Shared.Entities.SQL.User CurrentUser = sqliteManager.GetUser();
            if (CurrentUser == null)
            {
                var activity = new Intent(this, typeof(RegisterPhoneActivity));
                StartActivity(activity);
                this.Finish();
            }
            else
            {
                ProceedLogin(CurrentUser);
            }
        }

        public async void ProceedLogin(Shared.Entities.SQL.User entity)
        {
            if (!string.IsNullOrEmpty(entity.Token))
            {
                APIDataManager.SetUserToken(entity.Token);

                SessionManager.UserData = await APIDataManager.GetUserByName(entity.Name);
                if (SessionManager.UserData != null && SessionManager.UserData.CompanyId.HasValue)
                {
                    SessionManager.СontractorData = await APIDataManager.GetCompany(SessionManager.UserData.CompanyId.Value);
                    if (SessionManager.СontractorData != null)
                    {
                        SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                    }
                }

                if (SessionManager.UserData != null)
                {
                    var activity = new Intent(this, typeof(MainActivity));
                    StartActivity(activity);
                    this.Finish();
                }
                else
                {
                    var activity = new Intent(this, typeof(ErrorInfoActivity));
                    StartActivity(activity);
                    this.Finish();
                }
            }
        }
    }
}
