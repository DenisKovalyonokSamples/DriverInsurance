using DI.Localization;
using DI.Shared.Managers;
using DI.iOS.Fragments.Base;
using System;
using UIKit;
using DI.iOS.Managers;
using DI.Shared.Entities.SQL;

namespace DI.iOS
{
    public partial class DemoModeFragment : BaseFragment
    {
        float SectionDescriptionHeight = 61;
        float CountDescriptionHeight = 38;

        public DemoModeFragment (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            SetupScrollView();
        }

        void SetupScrollView()
        {
            SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, VContainerHeight.Constant - SectionDescriptionHeight * 3 - CountDescriptionHeight);
            VContainerWidth.Constant = UIScreen.MainScreen.Bounds.Width;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            BTSubmit.SetTitle(AppResources.StartTrack.ToUpper(), UIControlState.Normal);

            LBPageTitle.Text = AppResources.DemoMode.ToUpper();
            LBDayMarkTitle.Text = AppResources.MarkForDay;
            LBPeriodMarkTitle.Text = AppResources.MarkForPeriod;

            LBTrips.Text = AppResources.Trips;
            LBAccelerations.Text = AppResources.Accelerations;
            LBMileage.Text = AppResources.Mileage;

            LBTripsDescription.Text = AppResources.TripsDescription;
            LBAccelerationsDescription.Text = AppResources.AccelerationsDescription;
            LBMileageDescription.Text = AppResources.MileageDescription;

            LBInfoBlockOne.Text = AppResources.ContractDescriptionMessage;
            LBInfoBlockTwo.Text = AppResources.RateDescriptionMessage;

            VTripsDescription.Hidden = true;
            CTripsDescriptionHeight.Constant = 0;
            VAccelerationsDescription.Hidden = true;
            CAccelerationsDescriptionHeight.Constant = 0;
            VMileageDescription.Hidden = true;
            CMileageDescriptionHeight.Constant = 0;

            SetDemoData();

            SetupGestures();
        }

        void SetDemoData()
        {
            SVContainer.Hidden = true;
            AIProgressBar.Hidden = false;
            AIProgressBar.StartAnimating();

            var entity = sqliteManager.GetDemoModeData();
            if (entity == null)
            {
                LBDayMarkValue.Text = "0";

                LBDayDynamics.Hidden = true;
                LBPeriodMarkValue.Hidden = true;
                LBPeriodMarkTitle.Hidden = true;
                IVPeriodMark.Hidden = true;

                var model = new DemoModeData();
                model.Accelerations = 0;
                model.DayMark = 0;
                model.Mileage = 0;
                model.PeriodMark = 0;
                model.RoundDate = DateTime.UtcNow;
                model.Trips = 0;
                model.RateDynamics = 0;
                model.IsInit = true;
                model.WithPeriod = false;
                sqliteManager.SaveDemoModeData(model);
            }
            else
            {
                if (entity.RoundDate.Date != DateTime.UtcNow.Date)
                {
                    entity = CalculateNewScore(entity);
                }

                if (!entity.WithPeriod)
                {
                    LBPeriodMarkValue.Hidden = true;
                    LBPeriodMarkTitle.Hidden = true;
                    IVPeriodMark.Hidden = true;
                }

                LBDayMarkValue.Text = entity.DayMark.ToString();
                LBPeriodMarkValue.Text = entity.PeriodMark.ToString();

                if (entity.RateDynamics != 0)
                {
                    if (entity.RateDynamics > 0)
                    {
                        LBDayDynamics.Text = "+" + entity.RateDynamics.ToString();
                        LBDayDynamics.TextColor = ColorManager.submit_button_color;
                    }
                    else
                    {
                        LBDayDynamics.Text = entity.RateDynamics.ToString();
                        LBDayDynamics.TextColor = ColorManager.statusbar_red;
                    }
                }
                else
                {
                    LBDayDynamics.Text = string.Empty;
                }

                SetStarsRatingSource(entity.Trips, IVTripsRating);
                SetStarsRatingSource(entity.Accelerations, IVAccelerationsRating);
                SetStarsRatingSource(entity.Mileage, IVMileageRating);
            }

            AIProgressBar.StopAnimating();
            AIProgressBar.Hidden = true;
            SVContainer.Hidden = false;
        }

        DemoModeData CalculateNewScore(DemoModeData currentData)
        {
            var model = new DemoModeData();
            model.Accelerations = DataManager.GenerateRandomNumber(3, 5);
            model.DayMark = DataManager.GenerateRandomNumber(65, 85);
            model.Mileage = DataManager.GenerateRandomNumber(3, 5);
            model.PeriodMark = DataManager.GenerateRandomNumber(70, 80);
            model.RoundDate = DateTime.UtcNow;
            model.Trips = DataManager.GenerateRandomNumber(3, 5);
            model.RateDynamics = model.DayMark - currentData.DayMark;

            if (currentData.IsInit)
            {
                model.IsInit = false;
                model.WithPeriod = false;
                model.RateDynamics = 0;
            }
            else
            {
                model.IsInit = false;
                model.WithPeriod = true;
            }

            sqliteManager.ClearDemoModeDatas();
            sqliteManager.SaveDemoModeData(model);

            return sqliteManager.GetDemoModeData();
        }

        void SetStarsRatingSource(int value, UIImageView control)
        {
            if (value < 0)
            {
                control.Image = UIImage.FromBundle("star_line_zero/star_line_zero");
            }
            if (value >= 1 && value < 2)
            {
                control.Image = UIImage.FromBundle("star_line_one/star_line_one");
            }
            if (value >= 2 && value < 3)
            {
                control.Image = UIImage.FromBundle("star_line_two/star_line_two");
            }
            if (value >= 3 && value < 4)
            {
                control.Image = UIImage.FromBundle("star_line_three/star_line_three");
            }
            if (value >= 4 && value < 5)
            {
                control.Image = UIImage.FromBundle("star_line_four/star_line_four");
            }
            if (value >= 5)
            {
                control.Image = UIImage.FromBundle("star_line_five/star_line_five");
            }
        }

        void SetupGestures()
        {
            var vTripsContainerClick = new UITapGestureRecognizer(Trips_Click);
            VTripsContainer.UserInteractionEnabled = true;
            VTripsContainer.AddGestureRecognizer(vTripsContainerClick);

            var vAccelerationsContainerClick = new UITapGestureRecognizer(Accelerations_Click);
            VAccelerationsContainer.UserInteractionEnabled = true;
            VAccelerationsContainer.AddGestureRecognizer(vAccelerationsContainerClick);

            var vMileageContainerClick = new UITapGestureRecognizer(Mileage_Click);
            VMileageContainer.UserInteractionEnabled = true;
            VMileageContainer.AddGestureRecognizer(vMileageContainerClick);

            BTSubmit.TouchUpInside += delegate
            {
                if (BTSubmit.CurrentTitle == AppResources.StartTrack.ToUpper())
                {
                    BTSubmit.SetTitle(AppResources.StopTrack.ToUpper(), UIControlState.Normal);
                }
                else
                {
                    BTSubmit.SetTitle(AppResources.StartTrack.ToUpper(), UIControlState.Normal);
                }
            };
        }

        void Trips_Click()
        {
            if (VTripsDescription.Hidden)
            {
                VTripsDescription.Hidden = false;
                CTripsDescriptionHeight.Constant = SectionDescriptionHeight;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height + SectionDescriptionHeight);
            }
            else
            {
                VTripsDescription.Hidden = true;
                CTripsDescriptionHeight.Constant = 0;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height - SectionDescriptionHeight);
            }
        }

        void Accelerations_Click()
        {
            if (VAccelerationsDescription.Hidden)
            {
                VAccelerationsDescription.Hidden = false;
                CAccelerationsDescriptionHeight.Constant = SectionDescriptionHeight;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height + SectionDescriptionHeight);
            }
            else
            {
                VAccelerationsDescription.Hidden = true;
                CAccelerationsDescriptionHeight.Constant = 0;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height - SectionDescriptionHeight);
            }
        }

        void Mileage_Click()
        {
            if (VMileageDescription.Hidden)
            {
                VMileageDescription.Hidden = false;
                CMileageDescriptionHeight.Constant = SectionDescriptionHeight;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height + SectionDescriptionHeight);
            }
            else
            {
                VMileageDescription.Hidden = true;
                CMileageDescriptionHeight.Constant = 0;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height - SectionDescriptionHeight);
            }
        }
    }
}