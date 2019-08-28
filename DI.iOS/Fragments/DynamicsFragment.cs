using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using DI.iOS.Fragments.Base;
using Foundation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using Syncfusion.SfChart.iOS;
using DI.iOS.Managers;
using System.Collections.ObjectModel;

namespace DI.iOS
{
    public partial class DynamicsFragment : BaseFragment
    {
        int period = 30;
        SFChart chartRating = new SFChart();
        SFChart chartMileage = new SFChart();

        public DynamicsFragment(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitControls();
        }

        void InitControls()
        {
            LBRatingTitle.Text = AppResources.Rating.ToUpper();
            LBMileageTitle.Text = AppResources.MileageTitle.ToUpper();
            LBMonthTitle.Text = AppResources.ForMonth.ToUpper();
            LBWeekTitle.Text = AppResources.ForWeek.ToUpper();

            SetupGestures();
        }

        public async void RefreshData()
        {
            RefreshCharts();
        }

        void SetupGestures()
        {
            var vMonthTabClick = new UITapGestureRecognizer(MonthTab_Click);
            VMonthTab.UserInteractionEnabled = true;
            VMonthTab.AddGestureRecognizer(vMonthTabClick);

            var vWeekTabClick = new UITapGestureRecognizer(WeekTab_Click);
            VWeekTab.UserInteractionEnabled = true;
            VWeekTab.AddGestureRecognizer(vWeekTabClick);
        }

        void MonthTab_Click()
        {
            VMonthSeparator.Hidden = false;
            VWeekSeparator.Hidden = true;

            period = 30;

            RefreshCharts();
        }

        void WeekTab_Click()
        {
            VMonthSeparator.Hidden = true;
            VWeekSeparator.Hidden = false;

            period = 7;

            RefreshCharts();
        }

        async Task RefreshCharts()
        {
            VMonthTab.UserInteractionEnabled = false;
            VWeekTab.UserInteractionEnabled = false;

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

                    var data = new NSMutableArray();

                    foreach (var date in dates)
                    {
                        if (rounds.Any(e => e.ScoreDay == date.Date))
                        {
                            var round = rounds.Where(e => e.ScoreDay == date.Date).FirstOrDefault();
                            data.Add(new SFChartDataPoint(NSObject.FromObject(round.ScoreDay.ToString("dd.MM")), NSObject.FromObject(Convert.ToInt32(Math.Round(round.PointsMoment * 10, 0)))));
                        }
                        else
                        {
                            data.Add(new SFChartDataPoint(NSObject.FromObject(date.ToString("dd.MM")), NSObject.FromObject(0)));
                        }
                    }

                    SetDataToChart(data, maxValue, minValue);
                }
            }
            else
            {
                //no data
            }

            AIRatingProgressBar.Hidden = true;
        }

        void SetDataToChart(NSMutableArray data, double maxValue, double minValue)
        {            
            var frame = VRatingChartContainer.Frame;
            frame.X = 0;
            frame.Y = 0;
            chartRating.Frame = frame;

            chartRating.Series.Clear();

            chartRating.BackgroundColor = UIColor.Clear;

            SFCategoryAxis categoryaxis = new SFCategoryAxis();
            categoryaxis.LabelPlacement = SFChartLabelPlacement.BetweenTicks;
            categoryaxis.EdgeLabelsDrawingMode = SFChartAxisEdgeLabelsDrawingMode.Shift;
            categoryaxis.Title.Color = UIColor.White;
            categoryaxis.Title.Text = new NSString(AppResources.Days);
            chartRating.PrimaryAxis = categoryaxis;

            SFNumericalAxis numericalaxis = new SFNumericalAxis();
            numericalaxis.Title.Color = UIColor.White;
            numericalaxis.Title.Text = new NSString(AppResources.Rating);
            numericalaxis.Maximum = NSObject.FromObject(maxValue);
            numericalaxis.Minimum = NSObject.FromObject(minValue);
            chartRating.SecondaryAxis = numericalaxis;

            var dataModel = new ChartLineRatingDataSource(data);
            chartRating.DataSource = dataModel as SFChartDataSource;

            VRatingChartContainer.AddSubview(chartRating);
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
                        var data = new NSMutableArray();
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
                                    data.Add(new SFChartDataPoint(NSObject.FromObject(date.ToString("dd.MM")), NSObject.FromObject(Math.Round(mileage / 1000, 0))));
                                }
                                else
                                {
                                    data.Add(new SFChartDataPoint(NSObject.FromObject(date.ToString("dd.MM")), NSObject.FromObject(0)));
                                }
                            }
                        }

                        SetDataToMileageChart(data);
                    }
                }
            }

            AIMileageProgressBar.Hidden = true;

            VMonthTab.UserInteractionEnabled = true;
            VWeekTab.UserInteractionEnabled = true;
        }

        void SetDataToMileageChart(NSMutableArray data)
        {
            var frame = VMileageChartContainer.Frame;
            frame.X = 0;
            frame.Y = 0;
            chartMileage.Frame = frame;

            chartMileage.Series.Clear();

            chartMileage.BackgroundColor = UIColor.Clear;

            SFCategoryAxis categoryaxis = new SFCategoryAxis();
            categoryaxis.LabelPlacement = SFChartLabelPlacement.BetweenTicks;
            categoryaxis.Title.Color = UIColor.White;
            categoryaxis.Title.Text = new NSString(AppResources.Days);
            chartMileage.PrimaryAxis = categoryaxis;

            SFNumericalAxis numericalaxis = new SFNumericalAxis();
            numericalaxis.Title.Color = UIColor.White;
            numericalaxis.Title.Text = new NSString(AppResources.km);
            chartMileage.SecondaryAxis = numericalaxis;

            var dataModel = new ChartColumnMileageDataSource(data);
            chartMileage.DataSource = dataModel as SFChartDataSource;

            VMileageChartContainer.AddSubview(chartMileage);
        }
    }

    public class ChartLineRatingDataSource : SFChartDataSource
    {
        NSMutableArray DataPoints;

        public ChartLineRatingDataSource(NSMutableArray array)
        {
            DataPoints = array;
        }

        public override nint NumberOfSeriesInChart(SFChart chart)
        {
            return 1;
        }

        public override SFSeries GetSeries(SFChart chart, nint index)
        {
            SFLineSeries lineSeries = new SFLineSeries();
            lineSeries.Color = ColorManager.submit_button_color;
            lineSeries.DataMarker.ShowMarker = true;
            lineSeries.DataMarker.MarkerColor = ColorManager.submit_button_color;
            lineSeries.DataMarker.MarkerBorderColor = ColorManager.submit_button_color;
            lineSeries.EnableTooltip = true;
            lineSeries.EnableAnimation = true;

            return lineSeries;
        }

        public override SFChartDataPoint GetDataPoint(SFChart chart, nint index, nint seriesIndex)
        {
            return DataPoints.GetItem<SFChartDataPoint>((nuint)index);
        }

        public override nint GetNumberOfDataPoints(SFChart chart, nint index)
        {
            return (int)DataPoints.Count;
        }
    }

    public class ChartColumnMileageDataSource : SFChartDataSource
    {
        NSMutableArray DataPoints;

        public ChartColumnMileageDataSource(NSMutableArray array)
        {
            DataPoints = array;
        }

        public override nint NumberOfSeriesInChart(SFChart chart)
        {
            return 1;
        }

        public override SFSeries GetSeries(SFChart chart, nint index)
        {
            var columnSeries = new SFColumnSeries();
            columnSeries.Color = ColorManager.submit_button_color;
            columnSeries.EnableTooltip = true;
            columnSeries.EnableAnimation = true;

            return columnSeries;
        }

        public override SFChartDataPoint GetDataPoint(SFChart chart, nint index, nint seriesIndex)
        {
            return DataPoints.GetItem<SFChartDataPoint>((nuint)index);
        }

        public override nint GetNumberOfDataPoints(SFChart chart, nint index)
        {
            return (int)DataPoints.Count;
        }
    }
}