using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using DI.Droid.Adapters;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Interfaces;
using DI.Shared.Managers;
using DI.Shared.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Widget;
using Android.Content;
using DI.Shared;
using Android.Support.V7.Widget;
using static Android.Widget.AbsListView;
using Android.Runtime;
using DI.Shared.Enums;

namespace DI.Droid.Fragments
{
    public class TracksFragment : BaseListFragment, IOnScrollListener
    {
        List<TripViewModel> values = new List<TripViewModel>();
        List<TrackingTrip> entities = new List<TrackingTrip>();

        FilterType ActiveFilter = FilterType.All;
        TextView filterShowTodayData;
        TextView filterShowYesterdayData;
        TextView filterShowAllData;
        ProgressBar progressBarUpdating;

        string TimeStart = string.Empty;
        string TimeEnd = DataManager.DateTimeToUnixTimestampInt(DateTime.UtcNow).ToString();
        int UploadedValuesCount = 0;
        bool Locked = false;
        bool AllRange = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            progressBarUpdating = partial.FindViewById<ProgressBar>(Resource.Id.progressBarUpdating);
            filterShowTodayData = partial.FindViewById<TextView>(Resource.Id.filterShowTodayData);
            filterShowYesterdayData = partial.FindViewById<TextView>(Resource.Id.filterShowYesterdayData);
            filterShowAllData = partial.FindViewById<TextView>(Resource.Id.filterShowAllData);

            InitControls();

            return partial;
        }

        public override void OnResume()
        {
            base.OnResume();

            UploadedValuesCount = 0;
            if (ActiveFilter != FilterType.All)
            {
                entities.Clear();
            }
            _recyclerView.Visibility = ViewStates.Gone;
        }

        protected override void InitControls()
        {
            base.InitControls();

            filterShowTodayData.Text = AppResources.Today;
            filterShowYesterdayData.Text = AppResources.Yesterday;
            filterShowAllData.Text = AppResources.All;

            SetupGestures();

            GetEmptyListTextView().Text = AppResources.NoGridItems;
            _adapter = new TripRVAdapter(_recyclerView.Context, values, Activity.Resources);
            _recyclerView.SetAdapter(_adapter);

            _recyclerView.HasFixedSize = true;

            var layoutManager = new LinearLayoutManager(Activity);

            var onScrollListener = new XamarinRecyclerViewOnScrollListener(layoutManager);
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
            {
                if (!Locked && !AllRange && ActiveFilter == FilterType.All)
                {
                    Locked = true;
                    progressBarUpdating.Visibility = ViewStates.Visible;

                    UploadedValuesCount = values.Count;

                    UpdateDataWithPagingAsync();
                }
            };

            _recyclerView.AddOnScrollListener(onScrollListener);
            _recyclerView.SetLayoutManager(layoutManager);
        }

        void SetupGestures()
        {
            filterShowTodayData.Click += delegate
            {
                _recyclerView.Visibility = ViewStates.Gone;
                entities = new List<TrackingTrip>();
                UploadedValuesCount = 0;

                filterShowTodayData.SetBackgroundResource(Resource.Drawable.button_round_green);
                filterShowYesterdayData.SetBackgroundResource(Resource.Drawable.button_round_gray);
                filterShowAllData.SetBackgroundResource(Resource.Drawable.button_round_gray);

                TimeStart = DataManager.DateTimeToUnixTimestampInt(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0)).ToString();
                TimeEnd = DataManager.DateTimeToUnixTimestampInt(DateTime.UtcNow).ToString();

                ActiveFilter = FilterType.Today;
                GetEmptyListTextView().Visibility = ViewStates.Gone;
                UpdateData();
            };

            filterShowYesterdayData.Click += delegate
            {
                _recyclerView.Visibility = ViewStates.Gone;
                entities = new List<TrackingTrip>();
                UploadedValuesCount = 0;

                filterShowTodayData.SetBackgroundResource(Resource.Drawable.button_round_gray);
                filterShowYesterdayData.SetBackgroundResource(Resource.Drawable.button_round_green);
                filterShowAllData.SetBackgroundResource(Resource.Drawable.button_round_gray);

                TimeStart = DataManager.DateTimeToUnixTimestampInt(new DateTime(DateTime.UtcNow.AddDays(-1).Year, DateTime.UtcNow.AddDays(-1).Month, DateTime.UtcNow.AddDays(-1).Day, 0, 0, 0)).ToString();
                TimeEnd = DataManager.DateTimeToUnixTimestampInt(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0)).ToString();

                ActiveFilter = FilterType.Yesterday;
                GetEmptyListTextView().Visibility = ViewStates.Gone;
                UpdateData();
            };

            filterShowAllData.Click += delegate
            {
                _recyclerView.Visibility = ViewStates.Gone;
                entities = new List<TrackingTrip>();
                UploadedValuesCount = 0;

                filterShowTodayData.SetBackgroundResource(Resource.Drawable.button_round_gray);
                filterShowYesterdayData.SetBackgroundResource(Resource.Drawable.button_round_gray);
                filterShowAllData.SetBackgroundResource(Resource.Drawable.button_round_green);

                TimeStart = string.Empty;
                TimeEnd = DataManager.DateTimeToUnixTimestampInt(DateTime.UtcNow).ToString();

                ActiveFilter = FilterType.All;
                GetEmptyListTextView().Visibility = ViewStates.Gone;
                UpdateData();
            };
        }

        #region abstract

        protected override FloatingActionButton GetFloatingActionButton()
        {
            return null;
        }

        protected override Color GetItemBackgroundColor()
        {
            return Color.White;
        }

        protected override Color GetSelectedItemBackgroundColor()
        {
            return Color.LightGray;
        }

        async Task UpdateDataWithPagingAsync()
        {
            (this.Activity as MainActivity).IsTouchMovesLocked = true;
            filterShowTodayData.Enabled = false;
            filterShowYesterdayData.Enabled = false;
            filterShowAllData.Enabled = false;
            if (SessionManager.СontractData != null)
            {
                List<TrackingTrip> data = new List<TrackingTrip>();

                if (TimeStart != string.Empty)
                {
                    data = await APIDataManager.GetUserTrips(SessionManager.СontractData.Car.ToString(), TimeStart, TimeEnd);
                }
                else
                {
                    data = await APIDataManager.GetUserTripsWithPaging(SessionManager.СontractData.Car.ToString(), TimeEnd);
                }

                if (data != null && data.Count > 0)
                {
                    entities.AddRange(data);

                    await MapTripsDataToGrid();

                    if (ActiveFilter == FilterType.All)
                    {
                        int? lastTrip = entities.Min(e => e.TimeEnd);
                        if (lastTrip.HasValue)
                        {
                            TimeEnd = DataManager.DateTimeToUnixTimestampInt(DataManager.UnixTimeStampToDateTime(lastTrip.Value).AddSeconds(-1).ToUniversalTime()).ToString();
                        }
                    }

                    if (UploadedValuesCount != 0)
                    {
                        this.Activity.RunOnUiThread(() =>
                        {
                            _recyclerView.GetRecycledViewPool().Clear();

                            _adapter.NotifyDataSetChanged();
                            _recyclerView.ScrollToPosition(UploadedValuesCount - 1);
                        });
                    }
                }
                else
                {
                    AllRange = true;

                    if (TimeStart != string.Empty)
                    {
                        this.Activity.RunOnUiThread(() =>
                        {
                            values.Clear();
                            _recyclerView.GetRecycledViewPool().Clear();
                            _adapter.NotifyDataSetChanged();
                        });
                    }
                }

                progressBarUpdating.Visibility = ViewStates.Gone;
                _recyclerView.Visibility = ViewStates.Visible;
            }

            Locked = false;
            (this.Activity as MainActivity).IsTouchMovesLocked = false;
            filterShowTodayData.Enabled = true;
            filterShowYesterdayData.Enabled = true;
            filterShowAllData.Enabled = true;
        }

        protected override async Task UpdateDataAsync()
        {
            AllRange = false;

            await UpdateDataWithPagingAsync();

            if (values.Count == 0)
            {
                GetEmptyListTextView().Visibility = ViewStates.Visible;
            }

        }

        async Task MapTripsDataToGrid()
        {
            try
            {
                TrackingTrip[] trips = new TrackingTrip[entities.Count];
                entities.CopyTo(trips);
                values.Clear();

                DateTime? currentDate = null;

                if (trips != null)
                {
                    foreach (var trip in trips)
                    {
                        if (currentDate.HasValue)
                        {
                            if (currentDate.Value.Date != DataManager.UnixTimeStampToDateTime(trip.TimeStart).Date)
                            {
                                var modelTime = new TripViewModel();
                                modelTime.Id = 0;
                                modelTime.Date = DataManager.UnixTimeStampToDateTime(trip.TimeStart);
                                values.Add(modelTime);

                                currentDate = modelTime.Date;
                            }
                        }
                        else
                        {
                            var modelTime = new TripViewModel();
                            modelTime.Id = 0;
                            modelTime.Date = DataManager.UnixTimeStampToDateTime(trip.TimeStart);
                            values.Add(modelTime);

                            currentDate = modelTime.Date;
                        }

                        var model = new TripViewModel();
                        model.Id = trip.Id;
                        model.TripNumber = 0;
                        if (trip.Trip.HasValue)
                            model.TripNumber = trip.Trip.Value;

                        model.TimeStart = DataManager.UnixTimeStampToDateTime(trip.TimeStart).ToString("HH:mm");
                        model.TimeFinish = string.Empty;
                        if (trip.TimeEnd.HasValue)
                        {
                            model.TimeFinish = DataManager.UnixTimeStampToDateTime(trip.TimeEnd.Value).ToString("HH:mm");
                        }

                        model.AddressStart = string.Empty;
                        GeoAddress start = trip.AddressStartDetails;
                        if (start != null && start.AddressDetais != null)
                        {
                            if (!string.IsNullOrEmpty(start.AddressDetais.State))
                            {
                                model.AddressStart += start.AddressDetais.State;
                            }
                            if (!string.IsNullOrEmpty(start.AddressDetais.City))
                            {
                                if (!string.IsNullOrEmpty(model.AddressStart))
                                {
                                    model.AddressStart += ", ";
                                }
                                model.AddressStart += start.AddressDetais.City;
                            }
                            if (!string.IsNullOrEmpty(start.AddressDetais.Road))
                            {
                                if (!string.IsNullOrEmpty(model.AddressStart))
                                {
                                    model.AddressStart += ", ";
                                }
                                model.AddressStart += start.AddressDetais.Road;
                            }
                        }

                        model.AddressFinish = string.Empty;
                        GeoAddress end = trip.AddressEndDetails;
                        if (end != null && end.AddressDetais != null)
                        {
                            if (!string.IsNullOrEmpty(end.AddressDetais.State))
                            {
                                model.AddressFinish += end.AddressDetais.State;
                            }
                            if (!string.IsNullOrEmpty(end.AddressDetais.City))
                            {
                                if (!string.IsNullOrEmpty(model.AddressFinish))
                                {
                                    model.AddressFinish += ", ";
                                }
                                model.AddressFinish += end.AddressDetais.City;
                            }
                            if (!string.IsNullOrEmpty(end.AddressDetais.Road))
                            {
                                if (!string.IsNullOrEmpty(model.AddressFinish))
                                {
                                    model.AddressFinish += ", ";
                                }
                                model.AddressFinish += end.AddressDetais.Road;
                            }
                        }

                        model.AvgSpeed = trip.AvgSpeed;
                        model.MaxSpeed = trip.MaxSpeed;
                        model.Duration = 0;
                        if (trip.Duration.HasValue)
                        {
                            double min = trip.Duration.Value / 60;
                            model.Duration = Convert.ToInt32(Math.Round(min, 0));
                        }

                        double km = Convert.ToDouble(trip.Distance) / 1000;
                        model.Distance = Math.Round(km, 1);
                        model.Date = trip.CreatedAt;

                        List<Tracking> trackings = await APIDataManager.GetTripTrackings(SessionManager.СontractData.Car.ToString(), trip.TimeStart.ToString(), trip.TimeEnd.Value.ToString());
                        model.SharpAccelCount = 0;
                        model.SharpBrakeCount = 0;
                        if (trackings != null && trackings.Count > 0)
                        {
                            var sharpAccels = trackings.Where(e => e.AccelFront > Constants.NormalAccelLimit || e.AccelLateral > Constants.NormalAccelLimit);
                            if (sharpAccels != null && sharpAccels.Count() > 0)
                                model.SharpAccelCount = sharpAccels.Count();

                            var sharpBrakes = trackings.Where(e => e.Brake > Constants.NormalBrakeLimit);
                            if (sharpBrakes != null && sharpBrakes.Count() > 0)
                                model.SharpBrakeCount = sharpBrakes.Count();
                        }

                        values.Add(model);
                    }
                }
            }
            catch(Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("TracksFragment MapTripsDataToGrid ERROR: " + ex.ToString());
            }
        }

        protected override void ItemClickedOn(int position)
        {
            if (values.Count == 0)
                return;

            try
            {
                TripViewModel model = (TripViewModel)values[position];
                if (model != null && model.Id != 0)
                {
                    var activity = new Intent(this.Context, typeof(TripDetailsActivity));
                    activity.PutExtra(Constants.ID, model.Id.ToString());
                    StartActivity(activity);
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected override ISelectable GetSelectedItem(int position)
        {
            return null;
        }

        protected override async Task AddNewAction()
        {
        }

        protected override async Task EditAction(ISelectable selectedItem)
        {
        }

        protected override async Task DeleteAction(ISelectable selectedItem)
        {
        }

        protected override int GetLayoutId()
        {
            return Resource.Layout.Tracks;
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            throw new NotImplementedException();
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class XamarinRecyclerViewOnScrollListener : RecyclerView.OnScrollListener
    {
        public delegate void LoadMoreEventHandler(object sender, EventArgs e);
        public event LoadMoreEventHandler LoadMoreEvent;

        private LinearLayoutManager LayoutManager;

        public XamarinRecyclerViewOnScrollListener(LinearLayoutManager layoutManager)
        {
            LayoutManager = layoutManager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var pastVisiblesItems = LayoutManager.FindFirstVisibleItemPosition();

            if ((visibleItemCount + pastVisiblesItems) >= totalItemCount)
            {
                LoadMoreEvent(this, null);
            }
        }
    }
}
