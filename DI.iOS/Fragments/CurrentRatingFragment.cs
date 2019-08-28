using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using DI.iOS.Fragments.Base;
using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using DI.iOS.Managers;

namespace DI.iOS
{
    public partial class CurrentRatingFragment : BaseFragment
    {
        float SectionDescriptionHeight = 61;
        float CountDescriptionHeight = 38;

        public CurrentRatingFragment(IntPtr handle) : base(handle)
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
            LBBonusPointsTitle.Text = AppResources.BonusPoints;
            LBPageTitle.Text = AppResources.Marks.ToUpper();
            LBDayMarkTitle.Text = AppResources.MarkForDay;
            LBPeriodMarkTitle.Text = AppResources.MarkForPeriod;

            LBTrips.Text = AppResources.Trips;
            LBAccelerations.Text = AppResources.Accelerations;
            LBMileage.Text = AppResources.Mileage;

            LBTripsCount.Text = AppResources.Trips;
            LBMileageCount.Text = AppResources.Mileage;

            LBTripsDescription.Text = AppResources.TripsDescription;
            LBAccelerationsDescription.Text = AppResources.AccelerationsDescription;
            LBMileageDescription.Text = AppResources.MileageDescription;

            VTripsDescription.Hidden = true;
            CTripsDescriptionHeight.Constant = 0;
            VAccelerationsDescription.Hidden = true;
            CAccelerationsDescriptionHeight.Constant = 0;
            VMileageDescription.Hidden = true;
            CMileageDescriptionHeight.Constant = 0;
            VCountsDescription.Hidden = true;
            CCountsDescriptionHeight.Constant = 0;

            SetupGestures();
        }

        public async void RefreshData()
        {
            SVContainer.Hidden = true; 
            AIProgressBar.Hidden = false;
            AIProgressBar.StartAnimating();

            if (SessionManager.СontractData != null)
            {
                var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                if (contract != null && contract.Bonus != null)
                {
                    LBBonus.Text = contract.Bonus.Replace(".00", "");
                }
                else if (SessionManager.СontractData != null && SessionManager.СontractData.Bonus != null)
                {
                    LBBonus.Text = SessionManager.СontractData.Bonus.Replace(".00", "");
                }
                else
                {
                    LBBonus.Text = "0";
                }

                List<ScoringRound> markForPeriod = await APIDataManager.GetMarkForPeriod(SessionManager.СontractData.Id.ToString());
                if (markForPeriod != null && markForPeriod.Count > 0)
                {
                    DateTime max = markForPeriod.Max(e => e.ScoreDay);
                    var currentPeriod = markForPeriod.Where(e => e.ScoreDay == max).FirstOrDefault();
                    LBPeriodMarkValue.Text = Convert.ToInt32(Math.Round(currentPeriod.Points * 10, 0)).ToString();
                }
                else
                {
                    LBPeriodMarkValue.Text = "0";
                }

                List<ScoringRound> rounds = await APIDataManager.GetMarkForDay(SessionManager.СontractData.Id.ToString(), 2);
                if (rounds != null && rounds.Count > 0)
                {
                    DateTime maxDate = rounds.Max(e => e.ScoreDay);
                    var currentRound = rounds.Where(e => e.ScoreDay == maxDate).FirstOrDefault();
                    LBDayMarkValue.Text = Convert.ToInt32(Math.Round(currentRound.PointsMoment * 10, 0)).ToString();

                    var previousRound = rounds.Where(e => e.Id != currentRound.Id).FirstOrDefault();
                    if (previousRound != null)
                    {
                        int dynamic = Convert.ToInt32(Math.Round(currentRound.PointsMoment * 10, 0) - Math.Round(previousRound.PointsMoment * 10, 0));
                        if (dynamic > 0)
                        {
                            LBDayDynamics.Text = "+" + dynamic.ToString();
                            LBDayDynamics.TextColor = ColorManager.submit_button_color;
                        }
                        else if (dynamic < 0)
                        {
                            LBDayDynamics.Text = dynamic.ToString();
                            LBDayDynamics.TextColor = ColorManager.statusbar_red;
                        }
                        else
                        {
                            LBDayDynamics.Text = string.Empty;
                        }
                    }
                    else
                    {
                        LBDayDynamics.Text = string.Empty;
                    }

                    List<ScoringRoundParameter> roundParameters = await APIDataManager.GetScoringRoundParameters(currentRound.Id.ToString());
                    if (roundParameters != null && roundParameters.Count > 0)
                    {
                        //Trip Rating
                        float count_trip_day = 0;
                        float avg_time_trip = 0;

                        var count_trip_day_parameter = roundParameters.Where(e => e.ParameterCode == "count_trip_day").FirstOrDefault();
                        if (count_trip_day_parameter != null)
                        {
                            count_trip_day = count_trip_day_parameter.Point;
                        }

                        var avg_time_trip_parameter = roundParameters.Where(e => e.ParameterCode == "avg_time_trip").FirstOrDefault();
                        if (avg_time_trip_parameter != null)
                        {
                            avg_time_trip = avg_time_trip_parameter.Point;
                        }

                        SetTripsStarsSource(count_trip_day + avg_time_trip);

                        //Accelerations Rating
                        float count_accfront300 = 0;
                        float count_acclat150 = 0;
                        float count_brake300 = 0;

                        var count_accfront300_parameter = roundParameters.Where(e => e.ParameterCode == "count_accfront300").FirstOrDefault();
                        if (count_accfront300_parameter != null)
                        {
                            count_accfront300 = count_accfront300_parameter.Point;
                        }

                        var count_acclat150_parameter = roundParameters.Where(e => e.ParameterCode == "count_acclat150").FirstOrDefault();
                        if (count_acclat150_parameter != null)
                        {
                            count_acclat150 = count_acclat150_parameter.Point;
                        }

                        var count_brake300_parameter = roundParameters.Where(e => e.ParameterCode == "count_brake300").FirstOrDefault();
                        if (count_brake300_parameter != null)
                        {
                            count_brake300 = count_brake300_parameter.Point;
                        }

                        SetAccelerationsStarsSource(count_accfront300 + count_acclat150 + count_brake300);

                        //Mileage Rating
                        float distance_moscow = 0;
                        float distance_nomsk = 0;
                        float time_peakhour = 0;
                        float time_night = 0;
                        float distance_weekend = 0;

                        var distance_moscow_parameter = roundParameters.Where(e => e.ParameterCode == "distance_moscow").FirstOrDefault();
                        if (distance_moscow_parameter != null)
                        {
                            distance_moscow = distance_moscow_parameter.Point;
                        }

                        var distance_nomsk_parameter = roundParameters.Where(e => e.ParameterCode == "distance_nomsk").FirstOrDefault();
                        if (distance_nomsk_parameter != null)
                        {
                            distance_nomsk = distance_nomsk_parameter.Point;
                        }

                        var time_peakhour_parameter = roundParameters.Where(e => e.ParameterCode == "time_peakhour").FirstOrDefault();
                        if (time_peakhour_parameter != null)
                        {
                            time_peakhour = time_peakhour_parameter.Point;
                        }

                        var time_night_parameter = roundParameters.Where(e => e.ParameterCode == "time_night").FirstOrDefault();
                        if (time_night_parameter != null)
                        {
                            time_night = time_night_parameter.Point;
                        }

                        var distance_weekend_parameter = roundParameters.Where(e => e.ParameterCode == "distance_weekend").FirstOrDefault();
                        if (distance_weekend_parameter != null)
                        {
                            distance_weekend = distance_weekend_parameter.Point;
                        }

                        SetMileageStarsSource(distance_moscow + distance_nomsk + time_peakhour + time_night + distance_weekend);

                        float motion_count = 0;

                        var motion_count_parameter = roundParameters.Where(e => e.ParameterCode == "count_trip").FirstOrDefault();
                        if (motion_count_parameter != null)
                        {
                            motion_count = motion_count_parameter.Value;
                        }
                        LBTripsCountValue.Text = Convert.ToInt32(Math.Round(motion_count, 0)).ToString();

                        float distance = 0;

                        var distance_parameter = roundParameters.Where(e => e.ParameterCode == "distance").FirstOrDefault();
                        if (distance_parameter != null)
                        {
                            distance = distance_parameter.Value;
                        }
                        LBMileageCountValue.Text = Convert.ToInt32(Math.Round(distance, 0)).ToString() + " " + AppResources.km;
                    }
                }
                else
                {
                    LBDayDynamics.Text = string.Empty;
                }
            }

            AIProgressBar.StopAnimating();
            AIProgressBar.Hidden = true;
            SVContainer.Hidden = false;
        }

        void SetTripsStarsSource(float value)
        {
            if (value >= 0 && value < 0.5)
            {
                IVTripsRating.Image = UIImage.FromBundle("star_line_zero/star_line_zero"); 
            }
            if (value >= 0.5 && value < 1)
            {
                IVTripsRating.Image = UIImage.FromBundle("star_line_one/star_line_one");
            }
            if (value >= 1 && value < 1.5)
            {
                IVTripsRating.Image = UIImage.FromBundle("star_line_two/star_line_two");
            }
            if (value >= 1.5 && value < 2)
            {
                IVTripsRating.Image = UIImage.FromBundle("star_line_three/star_line_three");
            }
            if (value >= 2 && value < 2.5)
            {
                IVTripsRating.Image = UIImage.FromBundle("star_line_four/star_line_four");
            }
            if (value >= 2.5)
            {
                IVTripsRating.Image = UIImage.FromBundle("star_line_five/star_line_five");
            }
        }

        void SetAccelerationsStarsSource(float value)
        {
            if (value >= 0 && value < 0.4)
            {
                IVAccelerationsRating.Image = UIImage.FromBundle("star_line_zero/star_line_zero");
            }
            if (value >= 0.4 && value < 0.8)
            {
                IVAccelerationsRating.Image = UIImage.FromBundle("star_line_one/star_line_one");
            }
            if (value >= 0.8 && value < 1.2)
            {
                IVAccelerationsRating.Image = UIImage.FromBundle("star_line_two/star_line_two");
            }
            if (value >= 1.2 && value < 1.6)
            {
                IVAccelerationsRating.Image = UIImage.FromBundle("star_line_three/star_line_three");
            }
            if (value >= 1.6 && value < 2)
            {
                IVAccelerationsRating.Image = UIImage.FromBundle("star_line_four/star_line_four");
            }
            if (value >= 2)
            {
                IVAccelerationsRating.Image = UIImage.FromBundle("star_line_five/star_line_five");
            }
        }

        void SetMileageStarsSource(float value)
        {
            if (value >= 0 && value < 1)
            {
                IVMileageRating.Image = UIImage.FromBundle("star_line_zero/star_line_zero");
            }
            if (value >= 1 && value < 2)
            {
                IVMileageRating.Image = UIImage.FromBundle("star_line_one/star_line_one");
            }
            if (value >= 2 && value < 3)
            {
                IVMileageRating.Image = UIImage.FromBundle("star_line_two/star_line_two");
            }
            if (value >= 3 && value < 4)
            {
                IVMileageRating.Image = UIImage.FromBundle("star_line_three/star_line_three");
            }
            if (value >= 4 && value < 5)
            {
                IVMileageRating.Image = UIImage.FromBundle("star_line_four/star_line_four");
            }
            if (value >= 5)
            {
                IVMileageRating.Image = UIImage.FromBundle("star_line_five/star_line_five");
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

            var vTripsCountClick = new UITapGestureRecognizer(TripsCount_Click);
            VTripsCount.UserInteractionEnabled = true;
            VTripsCount.AddGestureRecognizer(vTripsCountClick);

            var vMileageCountClick = new UITapGestureRecognizer(MileageCount_Click);
            VMileageCount.UserInteractionEnabled = true;
            VMileageCount.AddGestureRecognizer(vMileageCountClick);
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

        void TripsCount_Click()
        {
            if (VCountsDescription.Hidden)
            {
                LBCountsDescription.Text = AppResources.TripsCountDescription;

                VCountsDescription.Hidden = false;
                CCountsDescriptionHeight.Constant = CountDescriptionHeight;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height + CountDescriptionHeight);
            }
            else
            {
                if (LBCountsDescription.Text == AppResources.TripsCountDescription)
                {
                    VCountsDescription.Hidden = true;
                    CCountsDescriptionHeight.Constant = 0;

                    SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height - CountDescriptionHeight);
                }
                else
                {
                    LBCountsDescription.Text = AppResources.TripsCountDescription;
                }
            }
        }

        void MileageCount_Click()
        {
            if (VCountsDescription.Hidden)
            {
                LBCountsDescription.Text = AppResources.MileageCountDescription;

                VCountsDescription.Hidden = false;
                CCountsDescriptionHeight.Constant = CountDescriptionHeight;

                SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height + CountDescriptionHeight);
            }
            else
            {
                if (LBCountsDescription.Text == AppResources.MileageCountDescription)
                {
                    VCountsDescription.Hidden = true;
                    CCountsDescriptionHeight.Constant = 0;

                    SVContainer.ContentSize = new CoreGraphics.CGSize(UIScreen.MainScreen.Bounds.Width, SVContainer.ContentSize.Height - CountDescriptionHeight);
                }
                else
                {
                    LBCountsDescription.Text = AppResources.MileageCountDescription;
                }
            }
        }
    }
}