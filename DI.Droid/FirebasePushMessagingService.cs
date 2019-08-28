using System;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using Android.Media;
using Android.Support.V7.App;

namespace DI.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    class FirebasePushMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                base.OnMessageReceived(message);

                SendNotification(message.GetNotification().Body);
            }
            catch (Exception ex)
            {
            }

        }
        private void SendNotification(string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetContentTitle("Driver Insurance")
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
    }
}