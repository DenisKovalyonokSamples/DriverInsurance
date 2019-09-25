using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using DI.Droid.Services;
using DI.Shared.Managers;

namespace DI.Droid
{
    [Application]
    public class AppWrapper : Application
    {
        public static Intent ServiceIntent { get; set; }
        public static SynchronizationService Service { get; set; }

        public AppWrapper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            SessionManager.AppVersion = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionName;
        }
    }
}