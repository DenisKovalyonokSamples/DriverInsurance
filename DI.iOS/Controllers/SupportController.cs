using DI.Localization;
using DI.iOS.Controllers.Base;
using Foundation;
using System;
using UIKit;

namespace DI.iOS
{
    public partial class SupportController : BaseController
    {
        public SupportController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBToolbarTitle.Text = AppResources.Support;

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