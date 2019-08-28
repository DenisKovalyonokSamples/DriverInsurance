using DI.Localization;
using DI.iOS.Fragments.Base;
using Foundation;
using System;
using UIKit;

namespace DI.iOS
{
    public partial class PolicyRequestFragment : BaseFragment
    {
        public PolicyRequestFragment (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBCurrentScoreTitle.Text = AppResources.YourCurrentResult;
            LBCostTitle.Text = AppResources.NoPolicy;

            BTSubmit.SetTitle(AppResources.GetPolicy.ToUpper(), UIControlState.Normal);

            SetupGestures();
        }

        void SetupGestures()
        {

        }
    }
}