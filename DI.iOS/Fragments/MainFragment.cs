using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using DI.iOS.Fragments.Base;
using Foundation;
using System;
using UIKit;
using System.Linq;

namespace DI.iOS
{
    public partial class MainFragment : BaseFragment
    {
        HockeyApp.iOS.BITHockeyManager HockeyManager = HockeyApp.iOS.BITHockeyManager.SharedHockeyManager;

        public MainFragment(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            CVDemoMode.Hidden = true;
            CVDynamics.Hidden = true;
            CVCurrentRating.Hidden = true;

            VCurrentRatingTab.Hidden = true;
            CCurrentRatingHeight.Constant = 0;
            VDynamicsTab.Hidden = true;
            CDynamicsHeight.Constant = 0;

            AIProgressBar.Hidden = false;

            CheckContract();
        }

        async void CheckContract()
        {
            try
            {
                if (SessionManager.СontractData == null)
                {
                    if (SessionManager.СontractorData != null)
                    {
                        SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                    }
                }

                if (SessionManager.СontractData != null)
                {
                    AIProgressBar.Hidden = true;

                    LBCurrentRatingTitle.Text = AppResources.CurrentRating.ToUpper();
                    LBDynamicsTitle.Text = AppResources.Dynamics.ToUpper();
                    CCurrentRatingHeight.Constant = 46;
                    CDynamicsHeight.Constant = 46;
                    VCurrentRatingTab.Hidden = false;
                    VDynamicsTab.Hidden = false;

                    ActivateCurrentRatingFragment();
                }
                else
                {
                    AIProgressBar.Hidden = true;
                    CVDemoMode.Hidden = false;
                }
            }
            catch (Exception ex)
            {
                HockeyManager.MetricsManager.TrackEvent("CheckContract ERROR: " + ex.ToString());
            }

            SetupGestures();
        }

        void SetupGestures()
        {
            var vCurrentRatingClick = new UITapGestureRecognizer(ActivateCurrentRatingFragment);
            VCurrentRatingTab.UserInteractionEnabled = true;
            VCurrentRatingTab.AddGestureRecognizer(vCurrentRatingClick);

            var vDynamicsClick = new UITapGestureRecognizer(ActivateDynamicsFragment);
            VDynamicsTab.UserInteractionEnabled = true;
            VDynamicsTab.AddGestureRecognizer(vDynamicsClick);
        }

        void ActivateCurrentRatingFragment()
        {
            VCurrentRatingSeparator.Hidden = false;
            VDynamicsSeparator.Hidden = true;

            CVDynamics.Hidden = true;
            CVCurrentRating.Hidden = false;

            CurrentRatingFragment controller = (CurrentRatingFragment)ChildViewControllers.First(e => e.GetType() == typeof(CurrentRatingFragment));
            if (controller != null)
                controller.RefreshData();
        }

        void ActivateDynamicsFragment()
        {
            VCurrentRatingSeparator.Hidden = true;
            VDynamicsSeparator.Hidden = false;

            CVCurrentRating.Hidden = true;
            CVDynamics.Hidden = false;

            DynamicsFragment controller = (DynamicsFragment)ChildViewControllers.First(e => e.GetType() == typeof(DynamicsFragment));
            if (controller != null)
                controller.RefreshData();
        }
    }
}