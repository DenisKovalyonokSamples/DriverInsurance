using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DI.Droid.Fragments
{
    public class UserInfoFragment : BaseFragment
    {
        TextView textCurrentBonuses;
        TextView textBonuses;

        TextView textMarksTitle;
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

        ImageView imageRateDynamics;
        ImageView imageCircleMark;

        TextView textTripsCountTitle;
        TextView textTripsCount;
        TextView textMileageSumTitle;
        TextView textMileageSum;

        ScrollView scrollMainContainer;
        ProgressBar progressBarLoading;

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

        RelativeLayout layoutTipsCount;
        RelativeLayout layoutMileageCount;
        TextView textMileageTipsDescription;
        RelativeLayout layoutMileageTipsDescription;
        LinearLayout layoutMileageTipsDescriptionSeparator;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            layoutMileageTipsDescription = partial.FindViewById<RelativeLayout>(Resource.Id.layoutMileageTipsDescription);
            layoutMileageTipsDescriptionSeparator = partial.FindViewById<LinearLayout>(Resource.Id.layoutMileageTipsDescriptionSeparator);
            textMileageTipsDescription = partial.FindViewById<TextView>(Resource.Id.textMileageTipsDescription);
            layoutTipsCount = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTipsCount);
            layoutMileageCount = partial.FindViewById<RelativeLayout>(Resource.Id.layoutMileageCount);

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

            textBonuses = partial.FindViewById<TextView>(Resource.Id.textBonuses);
            textCurrentBonuses = partial.FindViewById<TextView>(Resource.Id.textCurrentBonuses);

            textMarksTitle = partial.FindViewById<TextView>(Resource.Id.textMarksTitle);
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

            imageRateDynamics = partial.FindViewById<ImageView>(Resource.Id.imageRateDynamics);
            imageCircleMark = partial.FindViewById<ImageView>(Resource.Id.imageCircleMark);

            textTripsCountTitle = partial.FindViewById<TextView>(Resource.Id.textTripsCountTitle);
            textTripsCount = partial.FindViewById<TextView>(Resource.Id.textTripsCount);
            textMileageSumTitle = partial.FindViewById<TextView>(Resource.Id.textMileageSumTitle);
            textMileageSum = partial.FindViewById<TextView>(Resource.Id.textMileageSum);

            scrollMainContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollMainContainer);
            progressBarLoading = partial.FindViewById<ProgressBar>(Resource.Id.progressBarLoading);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            textBonuses.Text = AppResources.BonusPoints;
            textMarksTitle.Text = AppResources.Marks.ToUpper();
            textDriveMarkTitle.Text = AppResources.MarkForDay;
            textDriveMarkForPeriodTitle.Text = AppResources.MarkForPeriod;

            textStartTrackTitle.Text = AppResources.StartScoring.ToUpper();

            textResultScoreTitle.Text = AppResources.ResultScoreDescription;
            textTrips.Text = AppResources.Trips;
            textAccelerations.Text = AppResources.Accelerations;
            textMileage.Text = AppResources.Mileage;

            textTripsCountTitle.Text = AppResources.Trips;
            textMileageSumTitle.Text = AppResources.Mileage;

            textTripsDescription.Text = AppResources.TripsDescription;
            textAccelerationsDescription.Text = AppResources.AccelerationsDescription;
            textMileageDescription.Text = AppResources.MileageDescription;

            SetupGestures();

            SetRoundScoreData();
        }

        async void SetRoundScoreData()
        {
            scrollMainContainer.Visibility = ViewStates.Gone;
            progressBarLoading.Visibility = ViewStates.Visible;

            if (SessionManager.СontractData != null)
            {
                var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                if (contract != null && contract.Bonus != null)
                {
                    textCurrentBonuses.Text = contract.Bonus.Replace(".00", "");
                }
                else if (SessionManager.СontractData != null && SessionManager.СontractData.Bonus != null)
                {
                    textCurrentBonuses.Text = SessionManager.СontractData.Bonus.Replace(".00", "");
                }
                else
                {
                    textCurrentBonuses.Text = "0";
                }

                List<ScoringRound> markForPeriod = await APIDataManager.GetMarkForPeriod(SessionManager.СontractData.Id.ToString());
                if (markForPeriod != null && markForPeriod.Count > 0)
                {
                    DateTime max = markForPeriod.Max(e => e.ScoreDay);
                    var currentPeriod = markForPeriod.Where(e => e.ScoreDay == max).FirstOrDefault();
                    textPeriodMark.Text = Convert.ToInt32(Math.Round(currentPeriod.Points * 10, 0)).ToString();
                }
                else
                {
                    textPeriodMark.Text = "0";
                }

                List<ScoringRound> rounds = await APIDataManager.GetMarkForDay(SessionManager.СontractData.Id.ToString(), 2);
                if (rounds != null && rounds.Count > 0)
                {
                    DateTime maxDate = rounds.Max(e => e.ScoreDay);
                    var currentRound = rounds.Where(e => e.ScoreDay == maxDate).FirstOrDefault();
                    textDayMark.Text = Convert.ToInt32(Math.Round(currentRound.PointsMoment * 10, 0)).ToString();                  

                    var previousRound = rounds.Where(e => e.Id != currentRound.Id).FirstOrDefault();
                    if (previousRound != null)
                    {
                        int dynamic = Convert.ToInt32(Math.Round(currentRound.PointsMoment * 10, 0) - Math.Round(previousRound.PointsMoment * 10, 0));
                        if (dynamic > 0)
                        {
                            textDayDynamics.Text = "+" + dynamic.ToString();
                            textDayDynamics.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.submit_button_color)));
                        }
                        else if (dynamic < 0)
                        {
                            textDayDynamics.Text = dynamic.ToString();
                            textDayDynamics.SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.statusbar_red)));
                        }
                        else
                        {
                            textDayDynamics.Text = string.Empty;
                        }
                    }
                    else
                    {
                        textDayDynamics.Text = string.Empty;
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
                        textTripsCount.Text = Convert.ToInt32(Math.Round(motion_count, 0)).ToString();

                        float distance = 0;

                        var distance_parameter = roundParameters.Where(e => e.ParameterCode == "distance").FirstOrDefault();
                        if (distance_parameter != null)
                        {
                            distance = distance_parameter.Value;
                        }
                        textMileageSum.Text = Convert.ToInt32(Math.Round(distance, 0)).ToString() + " " + AppResources.km;
                    }
                }
                else
                {
                    textDayDynamics.Text = string.Empty;
                }
            }

            progressBarLoading.Visibility = ViewStates.Gone;
            scrollMainContainer.Visibility = ViewStates.Visible;
        }

        void SetTripsStarsSource(float value)
        {
            if (value >= 0 && value < 0.5)
            {
                imageTripStars.SetImageResource(Resource.Mipmap.star_line_zero);
            }
            if (value >= 0.5 && value < 1)
            {
                imageTripStars.SetImageResource(Resource.Mipmap.star_line_one);
            }
            if (value >= 1 && value < 1.5)
            {
                imageTripStars.SetImageResource(Resource.Mipmap.star_line_two);
            }
            if (value >= 1.5 && value < 2)
            {
                imageTripStars.SetImageResource(Resource.Mipmap.star_line_three);
            }
            if (value >= 2 && value < 2.5)
            {
                imageTripStars.SetImageResource(Resource.Mipmap.star_line_four);
            }
            if (value >= 2.5)
            {
                imageTripStars.SetImageResource(Resource.Mipmap.star_line_five);
            }
        }

        void SetAccelerationsStarsSource(float value)
        {
            if (value >= 0 && value < 0.4)
            {
                imageAccelerationStars.SetImageResource(Resource.Mipmap.star_line_zero);
            }
            if (value >= 0.4 && value < 0.8)
            {
                imageAccelerationStars.SetImageResource(Resource.Mipmap.star_line_one);
            }
            if (value >= 0.8 && value < 1.2)
            {
                imageAccelerationStars.SetImageResource(Resource.Mipmap.star_line_two);
            }
            if (value >= 1.2 && value < 1.6)
            {
                imageAccelerationStars.SetImageResource(Resource.Mipmap.star_line_three);
            }
            if (value >= 1.6 && value < 2)
            {
                imageAccelerationStars.SetImageResource(Resource.Mipmap.star_line_four);
            }
            if (value >= 2)
            {
                imageAccelerationStars.SetImageResource(Resource.Mipmap.star_line_five);
            }
        }

        void SetMileageStarsSource(float value)
        {
            if (value >= 0 && value < 1)
            {
                imageMileageStars.SetImageResource(Resource.Mipmap.star_line_zero);
            }
            if (value >= 1 && value < 2)
            {
                imageMileageStars.SetImageResource(Resource.Mipmap.star_line_one);
            }
            if (value >= 2 && value < 3)
            {
                imageMileageStars.SetImageResource(Resource.Mipmap.star_line_two);
            }
            if (value >= 3 && value < 4)
            {
                imageMileageStars.SetImageResource(Resource.Mipmap.star_line_three);
            }
            if (value >= 4 && value < 5)
            {
                imageMileageStars.SetImageResource(Resource.Mipmap.star_line_four);
            }
            if (value >= 5)
            {
                imageMileageStars.SetImageResource(Resource.Mipmap.star_line_five);
            }
        }

        void SetupGestures()
        {
            layoutTipsCount.Click += delegate
            {
                if (layoutMileageTipsDescription.Visibility == ViewStates.Gone)
                {
                    layoutMileageTipsDescription.Visibility = ViewStates.Visible;
                    layoutMileageTipsDescriptionSeparator.Visibility = ViewStates.Visible;
                    textMileageTipsDescription.Text = AppResources.TripsCountDescription;
                }
                else
                {
                    if (textMileageTipsDescription.Text == AppResources.TripsCountDescription)
                    {
                        layoutMileageTipsDescription.Visibility = ViewStates.Gone;
                        layoutMileageTipsDescriptionSeparator.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        textMileageTipsDescription.Text = AppResources.TripsCountDescription;
                    }
                }
            };

            layoutMileageCount.Click += delegate
            {
                if (layoutMileageTipsDescription.Visibility == ViewStates.Gone)
                {
                    layoutMileageTipsDescription.Visibility = ViewStates.Visible;
                    layoutMileageTipsDescriptionSeparator.Visibility = ViewStates.Visible;
                    textMileageTipsDescription.Text = AppResources.MileageCountDescription;
                }
                else
                {
                    if (textMileageTipsDescription.Text == AppResources.MileageCountDescription)
                    {
                        layoutMileageTipsDescription.Visibility = ViewStates.Gone;
                        layoutMileageTipsDescriptionSeparator.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        textMileageTipsDescription.Text = AppResources.MileageCountDescription;
                    }
                }
            };

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
                if (textStartTrackTitle.Text == AppResources.StartScoring.ToUpper())
                {
                    textStartTrackTitle.Text = AppResources.StopScoring.ToUpper();
                }
                else
                {
                    textStartTrackTitle.Text = AppResources.StartScoring.ToUpper();
                }
            };
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.UserInfo;
        }

        #endregion
    }
}
