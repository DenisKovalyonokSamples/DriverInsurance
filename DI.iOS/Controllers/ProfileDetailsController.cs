using DI.Localization;
using DI.iOS.Controllers.Base;
using Foundation;
using System;
using UIKit;

namespace DI.iOS
{
    public partial class ProfileDetailsController : BaseController
    {
        public ProfileDetailsController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBToolbarTitle.Text = AppResources.MyProfile;

            SetupGestures();
        }

        void SetupGestures()
        {
            var ivBackClick = new UITapGestureRecognizer(ReturnToPreviousController);
            IVBack.UserInteractionEnabled = true;
            IVBack.AddGestureRecognizer(ivBackClick);
        }

        void ReturnToPreviousController()
        {
            PresentingViewController.DismissViewController(true, null);
        }
    }
}