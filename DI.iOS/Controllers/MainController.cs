using DI.Localization;
using DI.Shared.Enums;
using DI.iOS.Controllers.Base;
using DI.iOS.Managers;
using Foundation;
using System;
using UIKit;

namespace DI.iOS
{
    public partial class MainController : BaseController
    {
        public bool isInit = true;
        public PartialType ActiveTab = PartialType.CurrentRating;

        public MainController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLayoutSubviews()
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

            LBMainTitle.Text = AppResources.Main;
            LBPoliciesTitle.Text = AppResources.Policies;
            LBBonusesTitle.Text = AppResources.Bonuses;
            LBUsefulTitle.Text = AppResources.Usefull;
            LBMoreTitle.Text = AppResources.More;

            LBMainTitle.TextColor = UIColor.White;
            LBPoliciesTitle.TextColor = ColorManager.description_message_color;
            LBBonusesTitle.TextColor = ColorManager.description_message_color;
            LBUsefulTitle.TextColor = ColorManager.description_message_color;
            LBMoreTitle.TextColor = ColorManager.description_message_color;

            IVMain.Image = IVMain.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IVMain.TintColor = UIColor.White;
            IVPolicies.Image = IVPolicies.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IVPolicies.TintColor = ColorManager.description_message_color;
            IVBonuses.Image = IVBonuses.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IVBonuses.TintColor = ColorManager.description_message_color;
            IVUseful.Image = IVUseful.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IVUseful.TintColor = ColorManager.description_message_color;
            IVMore.Image = IVMore.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IVMore.TintColor = ColorManager.description_message_color;

            SetupGestures();
        }

        void SetupGestures()
        {
            var vMainClick = new UITapGestureRecognizer(ActivateMainFragment);
            VMenuMain.UserInteractionEnabled = true;
            VMenuMain.AddGestureRecognizer(vMainClick);

            var vPoliciesClick = new UITapGestureRecognizer(ActivatePoliciesFragment);
            VMenuPolicies.UserInteractionEnabled = true;
            VMenuPolicies.AddGestureRecognizer(vPoliciesClick);

            var vBonusesClick = new UITapGestureRecognizer(ActivateBonusesFragment);
            VMenuBonuses.UserInteractionEnabled = true;
            VMenuBonuses.AddGestureRecognizer(vBonusesClick);

            var vUsefulClick = new UITapGestureRecognizer(ActivateUsefulFragment);
            VMenuUseful.UserInteractionEnabled = true;
            VMenuUseful.AddGestureRecognizer(vUsefulClick);

            var vMoreClick = new UITapGestureRecognizer(ActivateMoreFragment);
            VMenuMore.UserInteractionEnabled = true;
            VMenuMore.AddGestureRecognizer(vMoreClick);
        }

        void ActivateMainFragment()
        {
            SetFragmentEnvironment(PartialType.CurrentRating);
        }

        void ActivatePoliciesFragment()
        {
            SetFragmentEnvironment(PartialType.Policies);
        }

        void ActivateBonusesFragment()
        {
            SetFragmentEnvironment(PartialType.Bonuses);
        }

        void ActivateUsefulFragment()
        {
            SetFragmentEnvironment(PartialType.Usefull);
        }

        void ActivateMoreFragment()
        {
            SetFragmentEnvironment(PartialType.Profile);
        }

        public void SetFragmentEnvironment(PartialType type)
        {
            ResetFragments();

            switch (type)
            {
                case (PartialType.CurrentRating):
                    LBToolbarTitle.Text = AppResources.Main;
                    CVMain.Hidden = false;
                    VMainSelector.Hidden = false;
                    LBMainTitle.TextColor = UIColor.White;
                    IVMain.TintColor = UIColor.White;
                    break;
                case (PartialType.Policies):
                    LBToolbarTitle.Text = AppResources.Policies;
                    CVPolicies.Hidden = false;
                    VPoliciesSelector.Hidden = false;
                    LBPoliciesTitle.TextColor = UIColor.White;
                    IVPolicies.TintColor = UIColor.White;
                    break;
                case (PartialType.Bonuses):
                    LBToolbarTitle.Text = AppResources.Bonuses;
                    CVBonuses.Hidden = false;
                    VBonusesSelector.Hidden = false;
                    LBBonusesTitle.TextColor = UIColor.White;
                    IVBonuses.TintColor = UIColor.White;
                    break;
                case (PartialType.Usefull):
                    LBToolbarTitle.Text = AppResources.Usefull;
                    CVUseful.Hidden = false;
                    VUsefulSelector.Hidden = false;
                    LBUsefulTitle.TextColor = UIColor.White;
                    IVUseful.TintColor = UIColor.White;
                    break;
                case (PartialType.Profile):
                    LBToolbarTitle.Text = AppResources.Profile;
                    CVMore.Hidden = false;
                    VMoreSelector.Hidden = false;
                    LBMoreTitle.TextColor = UIColor.White;
                    IVMore.TintColor = UIColor.White;
                    break;
            }
        }

        void ResetFragments()
        {
            CVMain.Hidden = true;
            CVPolicies.Hidden = true;
            CVBonuses.Hidden = true;
            CVUseful.Hidden = true;
            CVMore.Hidden = true;

            VMainSelector.Hidden = true;
            VPoliciesSelector.Hidden = true;
            VBonusesSelector.Hidden = true;
            VUsefulSelector.Hidden = true;
            VMoreSelector.Hidden = true;

            IVMain.TintColor = ColorManager.description_message_color;
            IVPolicies.TintColor = ColorManager.description_message_color;
            IVBonuses.TintColor = ColorManager.description_message_color;
            IVUseful.TintColor = ColorManager.description_message_color;
            IVMore.TintColor = ColorManager.description_message_color;

            LBMainTitle.TextColor = ColorManager.description_message_color;
            LBPoliciesTitle.TextColor = ColorManager.description_message_color;
            LBBonusesTitle.TextColor = ColorManager.description_message_color;
            LBUsefulTitle.TextColor = ColorManager.description_message_color;
            LBMoreTitle.TextColor = ColorManager.description_message_color;
        }
    }
}