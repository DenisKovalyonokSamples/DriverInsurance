using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Com.Syncfusion.Charts;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DI.Droid.Fragments
{
    public class DynamicsFragment : BaseFragment
    {
        int period = 30;

        Color greenColor;
        Color blueColor;

        TextView textRatingTitle;
        ProgressBar progressBarRatingLoading;
        SfChart chartRating;

        TextView textMileageTitle;
        ProgressBar progressBarMileageLoading;
        SfChart chartMileage;

        Button buttonMonthTab;
        Button buttonWeekTab;

        LinearLayout selectorMonthTab;
        LinearLayout selectorWeekTab;

        LinearLayout layoutTabs;
        LinearLayout layoutTabSelectors;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            layoutTabs = partial.FindViewById<LinearLayout>(DI.Droid.Resource.Id.layoutTabs);
            layoutTabSelectors = partial.FindViewById<LinearLayout>(DI.Droid.Resource.Id.layoutTabSelectors);

            selectorMonthTab = partial.FindViewById<LinearLayout>(DI.Droid.Resource.Id.selectorMonthTab);
            selectorWeekTab = partial.FindViewById<LinearLayout>(DI.Droid.Resource.Id.selectorWeekTab);

            textRatingTitle = partial.FindViewById<TextView>(DI.Droid.Resource.Id.textRatingTitle);
            progressBarRatingLoading = partial.FindViewById<ProgressBar>(DI.Droid.Resource.Id.progressBarRatingLoading);
            chartRating = partial.FindViewById<SfChart>(DI.Droid.Resource.Id.sfRatingChart);

            textMileageTitle = partial.FindViewById<TextView>(DI.Droid.Resource.Id.textMileageTitle);
            progressBarMileageLoading = partial.FindViewById<ProgressBar>(DI.Droid.Resource.Id.progressBarMileageLoading);
            chartMileage = partial.FindViewById<SfChart>(DI.Droid.Resource.Id.sfMileageChart);

            buttonMonthTab = partial.FindViewById<Button>(DI.Droid.Resource.Id.buttonMonthTab);
            buttonWeekTab = partial.FindViewById<Button>(DI.Droid.Resource.Id.buttonWeekTab);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            textRatingTitle.Text = AppResources.Rating.ToUpper();
            textMileageTitle.Text = AppResources.MileageTitle.ToUpper();
            buttonMonthTab.Text = AppResources.ForMonth.ToUpper();
            buttonWeekTab.Text = AppResources.ForWeek.ToUpper();

            greenColor = new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.content_green));
            blueColor = new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.actionbar_blue));

            SetupGestures();

            RefreshCharts();
        }

        void SetupGestures()
        {
            buttonMonthTab.Click += delegate
                {
                    selectorMonthTab.Visibility = ViewStates.Visible;
                    selectorWeekTab.Visibility = ViewStates.Invisible;

                    period = 30;
                    RefreshCharts();
                };

            buttonWeekTab.Click += delegate
                {
                    selectorMonthTab.Visibility = ViewStates.Invisible;
                    selectorWeekTab.Visibility = ViewStates.Visible;

                    period = 7;
                    RefreshCharts();
                };
        }

        async Task RefreshCharts()
        {
            layoutTabs.Visibility = ViewStates.Invisible;
            layoutTabSelectors.Visibility = ViewStates.Invisible;

            chartRating.Visibility = ViewStates.Gone;
            progressBarRatingLoading.Visibility = ViewStates.Visible;
            chartMileage.Visibility = ViewStates.Gone;
            progressBarMileageLoading.Visibility = ViewStates.Visible;

            try
            {
                await InitMileageChart();
                await InitRatingChart();
            }
            catch (Exception ex)
            {

            }
        }

        async Task InitRatingChart()
        {
            if (SessionManager.СontractData != null)
            {
                List<ScoringRound> rounds = await APIDataManager.GetScoringRoundsForPeriod(SessionManager.СontractData.Id.ToString(), period);
                if (rounds != null && rounds.Count > 0)
                {
                    var max = rounds.Max(e => e.PointsMoment);
                    double maxValue = Convert.ToDouble(Math.Round(max * 10, 0)) + 10;
                    var min = rounds.Min(e => e.PointsMoment);
                    double minValue = Convert.ToDouble(Math.Round(min * 10, 0)) - 10;

                    rounds = rounds.OrderBy(e => e.ScoreDay).ToList();

                    List<DateTime> dates = new List<DateTime>();
                    for (int i = 1; i < period + 1; i++)
                    {
                        dates.Add(DateTime.UtcNow.AddDays(-i));
                    }

                    dates = dates.OrderBy(e => e).ToList();

                    var data = new ObservableArrayList();

                    foreach (var date in dates)
                    {
                        if (rounds.Any(e => e.ScoreDay == date.Date))
                        {
                            var round = rounds.Where(e => e.ScoreDay == date.Date).FirstOrDefault();
                            data.Add(new ChartDataPoint(round.ScoreDay.ToString("dd.MM"), Convert.ToInt32(Math.Round(round.PointsMoment * 10, 0))));
                        }
                        else
                        {
                            data.Add(new ChartDataPoint(date.ToString("dd.MM"), 0));
                        }
                    }

                    SetDataToChart(data, maxValue, minValue);
                }
            }
            else
            {
                //no data
            }

            progressBarRatingLoading.Visibility = ViewStates.Gone;
            chartRating.Visibility = ViewStates.Visible;
        }

        void SetDataToChart(ObservableArrayList data, double maxValue, double minValue)
        {
            chartRating.Series.Clear();

            chartRating.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.background_dark_blue)));

            CategoryAxis categoryaxis = new CategoryAxis();
            categoryaxis.LabelPlacement = LabelPlacement.BetweenTicks;
            categoryaxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Shift;
            categoryaxis.Title.TextColor = Color.White;
            categoryaxis.Title.Text = AppResources.Days;
            chartRating.PrimaryAxis = categoryaxis;

            NumericalAxis numericalaxis = new NumericalAxis();
            numericalaxis.Title.TextColor = Color.White;
            numericalaxis.Title.Text = AppResources.Rating;
            numericalaxis.Maximum = maxValue;
            numericalaxis.Minimum = minValue;
            chartRating.SecondaryAxis = numericalaxis;

            LineSeries lineSeries1 = new LineSeries();
            lineSeries1.DataSource = data;
            lineSeries1.Color = new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.submit_button_color));
            lineSeries1.DataMarker.ShowMarker = true;
            lineSeries1.DataMarker.MarkerColor = new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.submit_button_color));
            lineSeries1.DataMarker.MarkerStrokeColor = new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.submit_button_color));
            lineSeries1.StrokeWidth = 3;

            lineSeries1.TooltipEnabled = true;
            lineSeries1.AnimationEnabled = true;
            chartRating.TooltipCreated += ChartRating_TooltipCreated;
            chartRating.TooltipDismiss += ChartRating_TooltipDismiss;
            chartRating.Series.Add(lineSeries1);
        }

        private void ChartRating_TooltipCreated(object sender, SfChart.TooltipCreatedEventArgs e)
        {
            buttonMonthTab.Enabled = false;
            buttonWeekTab.Enabled = false;
        }

        private void ChartRating_TooltipDismiss(object sender, SfChart.TooltipDismissEventArgs e)
        {
            buttonMonthTab.Enabled = true;
            buttonWeekTab.Enabled = true;
        }

        async Task InitMileageChart()
        {
            if (SessionManager.СontractData != null)
            {
                var entity = await APIDataManager.GetCarByCompany(SessionManager.СontractorData.Id.ToString());
                if (entity != null)
                {
                    string daystart = DateTime.UtcNow.AddDays(-(period + 1)).Date.ToString("yyyy-MM-dd");
                    List<TrackingDay> days = await APIDataManager.GetTrackingDaysForPeriod(entity.Id.ToString(), period + 1, daystart);
                    if (days != null)
                    {
                        var data = new ObservableArrayList();
                        int currentMileage = 0;
                        if (days != null && days.Count > 0)
                        {
                            List<DateTime> dates = new List<DateTime>();
                            for (int i = 1; i < period + 1; i++)
                            {
                                dates.Add(DateTime.UtcNow.AddDays(-i));
                            }

                            dates = dates.OrderBy(e => e).ToList();
                            days = days.OrderBy(e => e.Day).ToList();

                            foreach (var date in dates)
                            {
                                if (days.Any(e => e.Day == date.Date))
                                {
                                    float mileage = days.Where(e => e.Day == date.Date).FirstOrDefault().Value;
                                    data.Add(new ChartDataPoint(date.ToString("dd.MM"), Math.Round(mileage / 1000, 0)));
                                }
                                else
                                {
                                    data.Add(new ChartDataPoint(date.ToString("dd.MM"), 0));
                                }
                            }
                        }

                        SetDataToMileageChart(data);
                    }
                }
            }

            progressBarMileageLoading.Visibility = ViewStates.Gone;
            chartMileage.Visibility = ViewStates.Visible;

            layoutTabs.Visibility = ViewStates.Visible;
            layoutTabSelectors.Visibility = ViewStates.Visible;
        }

        void SetDataToMileageChart(ObservableArrayList data)
        {
            chartMileage.Series.Clear();

            chartMileage.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.background_dark_blue)));

            CategoryAxis categoryaxis = new CategoryAxis();
            categoryaxis.LabelPlacement = LabelPlacement.BetweenTicks;
            categoryaxis.Title.TextColor = Color.White;
            categoryaxis.Title.Text = AppResources.Days;
            chartMileage.PrimaryAxis = categoryaxis;

            NumericalAxis numericalaxis = new NumericalAxis();
            numericalaxis.Title.TextColor = Color.White;
            numericalaxis.Title.Text = AppResources.km;
            chartMileage.SecondaryAxis = numericalaxis;

            var columnSeries1 = new ColumnSeries();
            columnSeries1.Color = new Color(ContextCompat.GetColor(this.Activity, DI.Droid.Resource.Color.submit_button_color));
            columnSeries1.DataSource = data;

            chartMileage.Series.Add(columnSeries1);

            columnSeries1.TooltipEnabled = true;
            columnSeries1.AnimationEnabled = true;
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return DI.Droid.Resource.Layout.Dynamics;
        }

        #endregion
    }
}
