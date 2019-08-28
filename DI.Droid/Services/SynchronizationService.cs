using Android.App;
using Android.Content;
using Android.OS;
using DI.Shared;
using DI.Shared.DataAccess;
using DI.Shared.Entities.Smooch;
using System;
using System.Linq;
using System.Threading;
using static Android.OS.PowerManager;
using Android.Media;
using Android.Support.V7.App;

namespace DI.Droid.Services
{
    public delegate void SyncSmoochMessagesHandler();
    public delegate void SyncSmoochNoMessagesHandler();

    [Service(Icon = "@mipmap/icon", Label = "DI")]
    public class SynchronizationService : Service
    {
        private const int NotificationId = 1;

        WakeLock wl;
        Timer dataSyncTimer;
        long dataSyncDelay = (long)TimeSpan.FromSeconds(Constants.SMOOCH_SYNC_PERIOD).TotalMilliseconds;

        SQLDataManager sqlManager;

        public static event SyncSmoochMessagesHandler SyncSmoochMessages;
        public static event SyncSmoochNoMessagesHandler SyncSmoochNoMessages;
        public MessagesResponse Messages;

        public override void OnCreate()
        {
            base.OnCreate();
            AppWrapper.Service = this;

            Init();
        }

        public void SetPeriodForChatUpdating(int period)
        {
            dataSyncDelay = (long)TimeSpan.FromSeconds(period).TotalMilliseconds;
        }

        void Init()
        {
            sqlManager = new SQLDataManager();

            dataSyncTimer = new System.Threading.Timer(new TimerCallback(DataSyncHandler), null, (long)TimeSpan.FromSeconds(1).TotalMilliseconds, Timeout.Infinite);

            StartForeground();

            HockeyApp.Android.Metrics.MetricsManager.Register(Application, Constants.HOCKEY_APP_ANDROID);
            HockeyApp.Android.CrashManager.Register(this, Constants.HOCKEY_APP_ANDROID);
        }

        public async void DataSyncHandler(object o)
        {
            try
            {
                var userData = sqlManager.GetSmoochUserData();
                if (userData != null)
                {
                    var data = await SmoochManager.GetMessages(userData.UserId, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
                    if (data != null && data.Messages != null && data.Messages.Count > 0)
                    {
                        if (Messages != null && Messages.Messages.Count > 0)
                        {
                            var maxServerTime = data.Messages.Max(e => e.Received);
                            var maxLocalTime = Messages.Messages.Max(e => e.Received);

                            if (maxServerTime > maxLocalTime)
                            {
                                Messages = data;

                                ActivityManager am = (ActivityManager)this.GetSystemService(Context.ActivityService);
                                var taskInfo = am.GetRunningTasks(1);
                                string currentActivityName = taskInfo[0].TopActivity.ClassName;
                                if (currentActivityName.Contains("SupportActivity"))
                                {
                                    SyncSmoochMessages?.Invoke();
                                }
                                else
                                {
                                    var lastMessage = data.Messages.Where(e => e.Received == maxServerTime).FirstOrDefault();
                                    if (lastMessage != null && lastMessage.Role == "appMaker")
                                    {
                                        SendMessageNotification(lastMessage.Text);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Messages = data;

                            SyncSmoochMessages?.Invoke();
                        }
                    }
                    else
                    {
                        SyncSmoochNoMessages?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("Smooch Messages Synchronization service ERROR: " + ex.ToString());
            }

            dataSyncTimer.Change(dataSyncDelay, Timeout.Infinite);

            return;
        }

        private void StartForeground()
        {
            Intent intentNotif = new Intent(this, typeof(MainActivity));
            intentNotif.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intentNotif, 0);

            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            wl = pm.NewWakeLock(WakeLockFlags.Partial, "DCDILock");
            wl.Acquire();
        }

        private void SendMessageNotification(string body)
        {
            var intent = new Intent(this, typeof(SupportActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Mipmap.notification_message)
                .SetContentTitle("Support DI")
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetDefaults((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate))
                .SetContentIntent(pendingIntent);

            if ((int)Android.OS.Build.VERSION.SdkInt >= 16)
            {
                notificationBuilder.SetStyle(new NotificationCompat.BigTextStyle().BigText(body));
                notificationBuilder.SetPriority((int)NotificationPriority.High);
            }
            if ((int)Android.OS.Build.VERSION.SdkInt >= 21)
            {
                notificationBuilder.SetVisibility((int)NotificationVisibility.Public);
            }

            var notificationManager = NotificationManager.FromContext(this);

            Notification notification = null;
            if ((int)Android.OS.Build.VERSION.SdkInt >= 16)
            {
                notification = notificationBuilder.Build();
            }
            else
            {
                notification = notificationBuilder.Notification;
            }

            notificationManager.Notify(0, notification);
        }

        #region Android Service Members

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //Set sticky as we are a long running operation
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            AppWrapper.Service = null;

            if (wl != null)
                wl.Release();

            if (dataSyncTimer != null)
                dataSyncTimer.Change(Timeout.Infinite, Timeout.Infinite);

            StopForeground(true);
        }

        #endregion
    }
}
