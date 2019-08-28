using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.Entities.SQL;
using DI.Shared.Managers;
using System;

namespace DI.Droid.Fragments
{
    public class DemoModeFragment : BaseFragment
    {
        TextView textDemoTitle;
        TextView textDayMark;
        TextView textDayDynamics;
        TextView textDriveMarkTitle;
        TextView textPeriodMark;
        TextView textDriveMarkForPeriodTitle;

        RelativeLayout layoutStartTrack;
        TextView textStartTrackTitle;

        TextView textResultScoreTitle;
        TextView textTrips;
        ImageView imageTripStars;
        TextView textAccelerations;
        ImageView imageAccelerationStars;
        TextView textMileage;
        ImageView imageMileageStars;

        TextView textContractDescription;
        TextView textRateDescription;

        ImageView imageRateDynamics;
        ImageView imageCircleMark;

        ScrollView scrollMainContainer;

        RelativeLayout layoutTrips;
        RelativeLayout layoutTripsDescription;
        TextView textTripsDescription;
        LinearLayout separatorTripsDescription;

        RelativeLayout layoutAccelerations;
        RelativeLayout layoutAccelerationsDescription;
        TextView textAccelerationsDescription;
        LinearLayout separatorAccelerationsDescription;

        RelativeLayout layoutMileage;
        RelativeLayout layoutMileageDescription;
        TextView textMileageDescription;
        LinearLayout separatorMileageDescription;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            layoutTrips = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTrips);
            layoutTripsDescription = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTripsDescription);
            textTripsDescription = partial.FindViewById<TextView>(Resource.Id.textTripsDescription);
            separatorTripsDescription = partial.FindViewById<LinearLayout>(Resource.Id.separatorTripsDescription);

            layoutAccelerations = partial.FindViewById<RelativeLayout>(Resource.Id.layoutAccelerations);
            layoutAccelerationsDescription = partial.FindViewById<RelativeLayout>(Resource.Id.layoutAccelerationsDescription);
            textAccelerationsDescription = partial.FindViewById<TextView>(Resource.Id.textAccelerationsDescription);
            separatorAccelerationsDescription = partial.FindViewById<LinearLayout>(Resource.Id.separatorAccelerationsDescription);

            layoutMileage = partial.FindViewById<RelativeLayout>(Resource.Id.layoutMileage);
            layoutMileageDescription = partial.FindViewById<RelativeLayout>(Resource.Id.layoutMileageDescription);
            textMileageDescription = partial.FindViewById<TextView>(Resource.Id.textMileageDescription);
            separatorMileageDescription = partial.FindViewById<LinearLayout>(Resource.Id.separatorMileageDescription);

            textDemoTitle = partial.FindViewById<TextView>(Resource.Id.textDemoTitle);
            textDayMark = partial.FindViewById<TextView>(Resource.Id.textDayMark);
            textDayDynamics = partial.FindViewById<TextView>(Resource.Id.textDayDynamics);
            textDriveMarkTitle = partial.FindViewById<TextView>(Resource.Id.textDriveMarkTitle);

            textPeriodMark = partial.FindViewById<TextView>(Resource.Id.textPeriodMark);
            textDriveMarkForPeriodTitle = partial.FindViewById<TextView>(Resource.Id.textDriveMarkForPeriodTitle);
            layoutStartTrack = partial.FindViewById<RelativeLayout>(Resource.Id.layoutStartTrack);
            textStartTrackTitle = partial.FindViewById<TextView>(Resource.Id.textStartTrackTitle);
            textResultScoreTitle = partial.FindViewById<TextView>(Resource.Id.textResultScoreTitle);
            textTrips = partial.FindViewById<TextView>(Resource.Id.textTrips);
            imageTripStars = partial.FindViewById<ImageView>(Resource.Id.imageTripStars);
            textAccelerations = partial.FindViewById<TextView>(Resource.Id.textAccelerations);
            imageAccelerationStars = partial.FindViewById<ImageView>(Resource.Id.imageAccelerationStars);
            textMileage = partial.FindViewById<TextView>(Resource.Id.textMileage);
            imageMileageStars = partial.FindViewById<ImageView>(Resource.Id.imageMileageStars);
            textContractDescription = partial.FindViewById<TextView>(Resource.Id.textContractDescription);
            textRateDescription = partial.FindViewById<TextView>(Resource.Id.textRateDescription);

            imageRateDynamics = partial.FindViewById<ImageView>(Resource.Id.imageRateDynamics);
            imageCircleMark = partial.FindViewById<ImageView>(Resource.Id.imageCircleMark);

            scrollMainContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollMainContainer);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            scrollMainContainer.Visibility = ViewStates.Gone;

            textDemoTitle.Text = AppResources.DemoMode.ToUpper();
            textDriveMarkTitle.Text = AppResources.MarkForDay;
            textDriveMarkForPeriodTitle.Text = AppResources.MarkForPeriod;

            textStartTrackTitle.Text = AppResources.StartTrack.ToUpper();

            textResultScoreTitle.Text = AppResources.ResultScoreDescription;
            textTrips.Text = AppResources.Trips;
            textAccelerations.Text = AppResources.Accelerations;
            textMileage.Text = AppResources.Mileage;

            textContractDescription.Text = AppResources.ContractDescriptionMessage;
            textRateDescription.Text = AppResources.RateDescriptionMessage;

            textTripsDescription.Text = AppResources.TripsDescription;
            textAccelerationsDescription.Text = AppResources.AccelerationsDescription;
            textMileageDescription.Text = AppResources.MileageDescription;

            SetDemoData();

            SetupGestures();
        }

        void SetDemoData()
        {
            var entity = sqliteManager.GetDemoModeData();
            if (entity == null)
            {
                textDayMark.Text = "0";

                textDayDynamics.Visibility = ViewStates.Gone;
                textPeriodMark.Visibility = ViewStates.Gone;
                textDriveMarkForPeriodTitle.Visibility = ViewStates.Gone;
                imageCircleMark.Visibility = ViewStates.Gone;

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
                    textPeriodMark.Visibility = ViewStates.Gone;
                    textDriveMarkForPeriodTitle.Visibility = ViewStates.Gone;
                    imageCircleMark.Visibility = ViewStates.Gone;
                }

                textDayMark.Text = entity.DayMark.ToString();
                textPeriodMark.Text = entity.PeriodMark.ToString();

                if (entity.RateDynamics != 0)
                {
                    if (entity.RateDynamics > 0)
                    {
                        textDayDynamics.Text = "+" + entity.RateDynamics.ToString();
                        textDayDynamics.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.submit_button_color)));
                    }
                    else
                    {
                        textDayDynamics.Text = entity.RateDynamics.ToString();
                        textDayDynamics.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.statusbar_red)));
                    }
                }
                else
                {
                    textDayDynamics.Visibility = ViewStates.Gone;
                }

                SetStarsRatingSource(entity.Trips, imageTripStars);
                SetStarsRatingSource(entity.Accelerations, imageAccelerationStars);
                SetStarsRatingSource(entity.Mileage, imageMileageStars);
            }

            scrollMainContainer.Visibility = ViewStates.Visible;
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

        void SetStarsRatingSource(int value, ImageView control)
        {
            if (value < 0)
            {
                control.SetImageResource(Resource.Mipmap.star_line_zero);
            }
            if (value >= 1 && value < 2)
            {
                control.SetImageResource(Resource.Mipmap.star_line_one);
            }
            if (value >= 2 && value < 3)
            {
                control.SetImageResource(Resource.Mipmap.star_line_two);
            }
            if (value >= 3 && value < 4)
            {
                control.SetImageResource(Resource.Mipmap.star_line_three);
            }
            if (value >= 4 && value < 5)
            {
                control.SetImageResource(Resource.Mipmap.star_line_four);
            }
            if (value >= 5)
            {
                control.SetImageResource(Resource.Mipmap.star_line_five);
            }
        }

        void SetupGestures()
        {
            layoutTrips.Click += delegate
            {
                if (layoutTripsDescription.Visibility == ViewStates.Gone)
                {
                    layoutTripsDescription.Visibility = ViewStates.Visible;
                    separatorTripsDescription.Visibility = ViewStates.Visible;
                }
                else
                {
                    layoutTripsDescription.Visibility = ViewStates.Gone;
                    separatorTripsDescription.Visibility = ViewStates.Gone;
                }
            };

            layoutAccelerations.Click += delegate
            {
                if (layoutAccelerationsDescription.Visibility == ViewStates.Gone)
                {
                    layoutAccelerationsDescription.Visibility = ViewStates.Visible;
                    separatorAccelerationsDescription.Visibility = ViewStates.Visible;
                }
                else
                {
                    layoutAccelerationsDescription.Visibility = ViewStates.Gone;
                    separatorAccelerationsDescription.Visibility = ViewStates.Gone;
                }
            };

            layoutMileage.Click += delegate
            {
                if (layoutMileageDescription.Visibility == ViewStates.Gone)
                {
                    layoutMileageDescription.Visibility = ViewStates.Visible;
                    separatorMileageDescription.Visibility = ViewStates.Visible;
                }
                else
                {
                    layoutMileageDescription.Visibility = ViewStates.Gone;
                    separatorMileageDescription.Visibility = ViewStates.Gone;
                }
            };

            layoutStartTrack.Click += delegate
            {
                if (textStartTrackTitle.Text == AppResources.StartTrack.ToUpper())
                {
                    textStartTrackTitle.Text = AppResources.StopTrack.ToUpper();
                }
                else
                {
                    textStartTrackTitle.Text = AppResources.StartTrack.ToUpper();
                }
            };
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.DemoMode;
        }

        #endregion
    }
}
