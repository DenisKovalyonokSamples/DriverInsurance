using DI.Localization;
using DI.iOS.Controllers.Base;
using Foundation;
using System;
using UIKit;

namespace DI.iOS
{
    public partial class ErrorInfoController : BaseController
    {
        public ErrorInfoController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBToolbarTitle.Text = AppResources.AppName;
            LBErrorDescription.Text = AppResources.NoServerConnectionMessage;
        }
    }
}