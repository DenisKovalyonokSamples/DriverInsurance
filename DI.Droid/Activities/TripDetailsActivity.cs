using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Base;
using DI.Droid.Fragments;
using DI.Localization;
using DI.Shared;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Android.Gms.Maps.GoogleMap;
using static Android.Widget.TextView;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class TripDetailsActivity : BaseFormActivity, IOnMapReadyCallback, IInfoWindowAdapter, IOnInfoWindowClickListener, ISnapshotReadyCallback
    {
        MenuFragment menuFragment;

        LinearLayout layoutTripData;
        TextView TripNumber;
        TextView StartAddress;
        TextView Mileage;
        TextView MileageTitle;
        TextView Time;
        TextView TimeTitle;
        TextView AvgSpeed;
        TextView SpeedTitle;
        TextView Accelerates;
        TextView AcceleratesTitle;
        TextView Brakes;
        TextView BrakesTitle;
        TextView MaxSpeed;
        TextView MaxSpeedTitle;
        TextView FinishAddress;

        TextView TimeStart;
        TextView TimeFinish;
        ImageView AcceleratesCircle;
        ImageView BrakesCircle;

        ProgressBar progressBarLoading;

        MapView mapView;
        GoogleMap _map;
        LatLng CurrentLocation;
        ImageView imageViewExpand;

        RelativeLayout mapContainer;
        LinearLayout dataContainer;

        int tripId;
        bool IsMapExpanded = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TripDetails);
            SetTitleBack();

            tripId = Convert.ToInt32(Intent.GetStringExtra(Constants.ID));

            mapView = FindViewById<MapView>(Resource.Id.map);
            mapView.OnCreate(bundle);

            mapContainer = FindViewById<RelativeLayout>(Resource.Id.mapContainer);
            dataContainer = FindViewById<LinearLayout>(Resource.Id.dataContainer);
            layoutTripData = FindViewById<LinearLayout>(Resource.Id.layoutTripData);
            TripNumber = FindViewById<TextView>(Resource.Id.gridItemTripId);
            StartAddress = FindViewById<TextView>(Resource.Id.gridItemStartAddress);
            Mileage = FindViewById<TextView>(Resource.Id.gridItemMileage);
            MileageTitle = FindViewById<TextView>(Resource.Id.gridItemMileageTitle);
            Time = FindViewById<TextView>(Resource.Id.gridItemTime);
            TimeTitle = FindViewById<TextView>(Resource.Id.gridItemTimeTitle);
            AvgSpeed = FindViewById<TextView>(Resource.Id.gridItemAvgSpeed);
            SpeedTitle = FindViewById<TextView>(Resource.Id.gridItemAvgSpeedTitle);
            TimeStart = FindViewById<TextView>(Resource.Id.gridItemTimeStart);
            TimeFinish = FindViewById<TextView>(Resource.Id.gridItemTimeFinish);
            AcceleratesCircle = FindViewById<ImageView>(Resource.Id.gridItemAcceleratesCircle);
            BrakesCircle = FindViewById<ImageView>(Resource.Id.gridItemBrakesCircle);
            Accelerates = FindViewById<TextView>(Resource.Id.gridItemAccelerates);
            AcceleratesTitle = FindViewById<TextView>(Resource.Id.gridItemAcceleratesTitle);
            Brakes = FindViewById<TextView>(Resource.Id.gridItemBrakes);
            BrakesTitle = FindViewById<TextView>(Resource.Id.gridItemBrakesTitle);
            MaxSpeed = FindViewById<TextView>(Resource.Id.gridItemMaxSpeed);
            MaxSpeedTitle = FindViewById<TextView>(Resource.Id.gridItemMaxSpeedTitle);
            FinishAddress = FindViewById<TextView>(Resource.Id.gridItemFinishAddress);
            imageViewExpand = FindViewById<ImageView>(Resource.Id.imageViewExpand);
            progressBarLoading = FindViewById<ProgressBar>(Resource.Id.progressBarLoading);

            menuFragment = new MenuFragment();

            if (!this.IsFinishing)
            {
                var partialMenuSetup = SupportFragmentManager.BeginTransaction();
                partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
                partialMenuSetup.CommitAllowingStateLoss();
            }

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            layoutTripData.Visibility = ViewStates.Invisible;

            MileageTitle.Text = AppResources.Mileage;
            TimeTitle.Text = AppResources.Time;
            SpeedTitle.Text = AppResources.AverageSpeed;
            AcceleratesTitle.Text = AppResources.SharpAcceleration;
            BrakesTitle.Text = AppResources.SharpBraking;
            MaxSpeedTitle.Text = AppResources.MaximumSpeed;
            GetSubmitButton().Text = AppResources.ShareResult.ToUpper();

            SetupGestures();
        }

        void SetupGestures()
        {
            imageViewExpand.Click += delegate
            {
                if (IsMapExpanded)
                {
                    dataContainer.Visibility = ViewStates.Visible;
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(mapContainer.LayoutParameters);
                    layoutParams.BottomMargin = this.Resources.GetDimensionPixelSize(Resource.Dimension.trip_map_container_margin_bottom);
                    mapContainer.LayoutParameters = layoutParams;
                    imageViewExpand.SetImageResource(Resource.Mipmap.arrow_expand);
                }
                else
                {
                    dataContainer.Visibility = ViewStates.Gone;
                    RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(mapContainer.LayoutParameters);
                    layoutParams.BottomMargin = this.Resources.GetDimensionPixelSize(Resource.Dimension.margin_bottom);
                    mapContainer.LayoutParameters = layoutParams;
                    imageViewExpand.SetImageResource(Resource.Mipmap.arrow_compress);
                }

                IsMapExpanded = !IsMapExpanded;
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            InitMapFragment();
        }

        #region Map

        bool ShowParkingMarker = false;

        void InitMapFragment()
        {
            if (_map == null)
            {
                mapView.GetMapAsync(this);
            }
            else
            {
                mapView.OnResume();
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            if (_map != null)
            {
                try
                {
                    MapsInitializer.Initialize(this);
                }
                catch (GooglePlayServicesNotAvailableException e)
                {

                }

                _map.UiSettings.ZoomControlsEnabled = true;
                _map.MyLocationEnabled = false;
                _map.MyLocationChange += _map_MyLocationChange;
                _map.CameraChange += delegate
                {
                    var coordinate = _map.CameraPosition.Target;

                    if (coordinate.Longitude == 0 || coordinate.Latitude == 0)
                        return;

                    CurrentLocation = coordinate;
                };

                View locationButton = mapView.FindViewWithTag("GoogleMapMyLocationButton");
                locationButton.Visibility = ViewStates.Gone;

                _map.SetInfoWindowAdapter(this);
                _map.SetOnInfoWindowClickListener(this);

                mapView.OnResume();
            }
        }

        private void _map_MyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            CurrentLocation = new LatLng(e.Location.Latitude, e.Location.Longitude);

            if (CurrentLocation.Longitude == 0 || CurrentLocation.Latitude == 0)
                return;

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(CurrentLocation, 15);
            _map.MoveCamera(cameraUpdate);
        }

        double BearingBetweenLocations(LatLng latLng1, LatLng latLng2)
        {
            double PI = 3.14159;
            double lat1 = latLng1.Latitude * PI / 180;
            double long1 = latLng1.Longitude * PI / 180;
            double lat2 = latLng2.Latitude * PI / 180;
            double long2 = latLng2.Longitude * PI / 180;

            double dLon = (long2 - long1);

            double y = System.Math.Sin(dLon) * System.Math.Cos(lat2);
            double x = System.Math.Cos(lat1) * System.Math.Sin(lat2) - System.Math.Sin(lat1)
                    * System.Math.Cos(lat2) * System.Math.Cos(dLon);

            double brng = System.Math.Atan2(y, x);

            brng = RadianToDegree(brng);
            brng = (brng + 360) % 360;

            return brng;
        }

        double RadianToDegree(double angle)
        {
            double PI = 3.14159;

            return angle * (180.0 / PI);
        }

        #endregion

        #region abstract

        protected override int GetStatusBarColor()
        {
            return Resource.Color.statusbar_blue;
        }

        protected override int GetActiveBarColor()
        {
            return Resource.Color.actionbar_blue;
        }

        protected override string GetTitle()
        {
            return AppResources.Trip;
        }

        protected override async Task LoadDataAsync()
        {
            if (tripId != 0)
            {
                var trip = await APIDataManager.GetTrackingTrip(tripId);
                if (trip != null)
                {
                    TripNumber.Text = AppResources.TripId;
                    if (trip.Trip.HasValue)
                    {
                        TripNumber.Text += " " + trip.Trip.Value.ToString();
                    }

                    string addressStart = string.Empty;
                    GeoAddress start = trip.AddressStartDetails;
                    if (start != null && start.AddressDetais != null)
                    {
                        if (!string.IsNullOrEmpty(start.AddressDetais.State))
                        {
                            addressStart += start.AddressDetais.State;
                        }
                        if (!string.IsNullOrEmpty(start.AddressDetais.City))
                        {
                            if (!string.IsNullOrEmpty(addressStart))
                            {
                                addressStart += ", ";
                            }
                            addressStart += start.AddressDetais.City;
                        }
                        if (!string.IsNullOrEmpty(start.AddressDetais.Road))
                        {
                            if (!string.IsNullOrEmpty(addressStart))
                            {
                                addressStart += ", ";
                            }
                            addressStart += start.AddressDetais.Road;
                        }
                    }
                    StartAddress.Text = addressStart;

                    string addressFinish = string.Empty;
                    GeoAddress end = trip.AddressEndDetails;
                    if (end != null && end.AddressDetais != null)
                    {
                        if (!string.IsNullOrEmpty(end.AddressDetais.State))
                        {
                            addressFinish += end.AddressDetais.State;
                        }
                        if (!string.IsNullOrEmpty(end.AddressDetais.City))
                        {
                            if (!string.IsNullOrEmpty(addressFinish))
                            {
                                addressFinish += ", ";
                            }
                            addressFinish += end.AddressDetais.City;
                        }
                        if (!string.IsNullOrEmpty(end.AddressDetais.Road))
                        {
                            if (!string.IsNullOrEmpty(addressFinish))
                            {
                                addressFinish += ", ";
                            }
                            addressFinish += end.AddressDetais.Road;
                        }
                    }
                    FinishAddress.Text = addressFinish;

                    AvgSpeed.Text = trip.AvgSpeed.ToString() + " " + AppResources.kmh.ToLower();
                    MaxSpeed.Text = trip.MaxSpeed.ToString() + " " + AppResources.kmh.ToLower();

                    int duration = 0;
                    if (trip.Duration.HasValue)
                    {
                        double min = trip.Duration.Value / 60;
                        duration = Convert.ToInt32(System.Math.Round(min, 0));
                    }
                    Time.Text = duration.ToString() + " " + AppResources.Min.ToLower();

                    double km = Convert.ToDouble(trip.Distance) / 1000;
                    Mileage.Text = System.Math.Round(km, 1).ToString() + " " + AppResources.km.ToLower();

                    SharingMileage = Mileage.Text;
                    SharingAverageSpeed = AvgSpeed.Text;
                    SharingTripTime = Time.Text;

                    List<Tracking> trackings = await APIDataManager.GetTripTrackings(SessionManager.СontractData.Car.ToString(), trip.TimeStart.ToString(), trip.TimeEnd.Value.ToString());
                    if (trackings != null && trackings.Count > 0)
                    {
                        int sharpAccelCount = 0;
                        var sharpAccels = trackings.Where(e => e.AccelFront > Constants.NormalAccelLimit || e.AccelLateral > Constants.NormalAccelLimit);
                        if (sharpAccels != null && sharpAccels.Count() > 0)
                            sharpAccelCount = sharpAccels.Count();

                        int sharpBrakeCount = 0;
                        var sharpBrakes = trackings.Where(e => e.Brake > Constants.NormalBrakeLimit);
                        if (sharpBrakes != null && sharpBrakes.Count() > 0)
                            sharpBrakeCount = sharpBrakes.Count();

                        if (sharpAccelCount == 0)
                        {
                            AcceleratesCircle.SetImageResource(Resource.Mipmap.circle_white);
                        }
                        else
                        {
                            AcceleratesCircle.SetImageResource(Resource.Mipmap.circle_orange);
                        }

                        if (sharpBrakeCount == 0)
                        {
                            BrakesCircle.SetImageResource(Resource.Mipmap.circle_white);
                        }
                        else
                        {
                            BrakesCircle.SetImageResource(Resource.Mipmap.circle_orange);
                        }

                        Accelerates.Text = sharpAccelCount.ToString();
                        Brakes.Text = sharpBrakeCount.ToString();

                        TimeStart.Text = DataManager.UnixTimeStampToDateTime(trip.TimeStart).ToString("HH:mm");
                        TimeFinish.Text = string.Empty;
                        if (trip.TimeEnd.HasValue)
                        {
                            TimeFinish.Text = DataManager.UnixTimeStampToDateTime(trip.TimeEnd.Value).ToString("HH:mm");
                        }

                        //Set track data to map
                        LatLngBounds.Builder builder = new LatLngBounds.Builder();

                        if (trip.PosBeginLat.HasValue && trip.PosBeginLng.HasValue
                            && trip.PosEndLat.HasValue && trip.PosEndLng.HasValue)
                        {
                            MarkerOptions markerStart = new MarkerOptions();
                            markerStart.SetTitle(AppResources.Time + ": " + DataManager.UnixTimeStampToDateTime(trip.TimeStart).ToString("HH:mm dd.MM.yyyy"));
                            markerStart.SetSnippet(AppResources.Address + ": " + addressStart);
                            markerStart.SetPosition(new LatLng(trip.PosBeginLat.Value, trip.PosBeginLng.Value));
                            markerStart.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.pin_start));
                            _map.AddMarker(markerStart);
                            builder.Include(new LatLng(trip.PosBeginLat.Value, trip.PosBeginLng.Value));

                            for (int i = 0; i < trackings.Count; i++)
                            {
                                var track = trackings[i];
                                LatLng startPoint;
                                LatLng endPoint;
                                LatLng pastPoint;

                                if (track.PosBeginLat.HasValue && track.PosBeginLng.HasValue)
                                {
                                    startPoint = new LatLng(track.PosBeginLat.Value, track.PosBeginLng.Value);
                                    endPoint = new LatLng(track.PosEndLat, track.PosEndLng);

                                    if (i == 0)
                                    {
                                        pastPoint = new LatLng(trip.PosBeginLat.Value, trip.PosBeginLng.Value);
                                    }
                                    else
                                    {
                                        var trackPast = trackings[i - 1];
                                        pastPoint = new LatLng(trackPast.PosEndLat, trackPast.PosEndLng);
                                    }

                                    PolylineOptions lineWithPast = new PolylineOptions();
                                    lineWithPast.Add(pastPoint, startPoint);
                                    lineWithPast.InvokeColor(new Color(ContextCompat.GetColor(this, Resource.Color.submit_button_color)));
                                    lineWithPast.InvokeWidth(5);
                                    _map.AddPolyline(lineWithPast);

                                    PolylineOptions lineMiddle = new PolylineOptions();
                                    lineMiddle.Add(startPoint, endPoint);
                                    lineMiddle.InvokeColor(new Color(ContextCompat.GetColor(this, Resource.Color.submit_button_color)));
                                    lineMiddle.InvokeWidth(5);
                                    _map.AddPolyline(lineMiddle);

                                    if (i == trackings.Count - 1)
                                    {
                                        PolylineOptions line = new PolylineOptions();
                                        line.Add(endPoint, new LatLng(trip.PosEndLat.Value, trip.PosEndLng.Value));
                                        line.InvokeColor(new Color(ContextCompat.GetColor(this, Resource.Color.submit_button_color)));
                                        line.InvokeWidth(5);
                                        _map.AddPolyline(line);
                                    }

                                    string isSharpFrontAccel = AppResources.No;
                                    if (track.AccelFront > Constants.NormalAccelLimit)
                                        isSharpFrontAccel = AppResources.Yes;

                                    string isSharpLateralAccel = AppResources.No;
                                    if (track.AccelLateral > Constants.NormalAccelLimit)
                                        isSharpLateralAccel = AppResources.Yes;

                                    string isSharpBrake = AppResources.No;
                                    if (track.Brake > Constants.NormalBrakeLimit)
                                        isSharpBrake = AppResources.Yes;


                                    LatLng nextPoint;
                                    if (i < trackings.Count - 1)
                                    {
                                        var trackNext = trackings[i + 1];
                                        nextPoint = new LatLng(trackNext.PosEndLat, trackNext.PosEndLng);
                                    }
                                    else
                                    {
                                        nextPoint = new LatLng(trip.PosEndLat.Value, trip.PosEndLng.Value);
                                    }

                                    string addressMarker = string.Empty;
                                    GeoAddress address = track.GeoAddressDetails;
                                    if (address != null && address.AddressDetais != null)
                                    {
                                        if (!string.IsNullOrEmpty(address.AddressDetais.State))
                                        {
                                            addressMarker += address.AddressDetais.State;
                                        }
                                        if (!string.IsNullOrEmpty(address.AddressDetais.City))
                                        {
                                            if (!string.IsNullOrEmpty(addressMarker))
                                            {
                                                addressMarker += ", ";
                                            }
                                            addressMarker += address.AddressDetais.City;
                                        }
                                        if (!string.IsNullOrEmpty(address.AddressDetais.Road))
                                        {
                                            if (!string.IsNullOrEmpty(addressMarker))
                                            {
                                                addressMarker += ", ";
                                            }
                                            addressMarker += address.AddressDetais.Road;
                                        }
                                    }

                                    MarkerOptions markerMiddleEnd = new MarkerOptions();
                                    markerMiddleEnd.SetTitle(AppResources.Time + ": " + DataManager.UnixTimeStampToDateTime(track.TimeEnd).ToString("HH:mm dd.MM.yyyy"));
                                    markerMiddleEnd.SetSnippet(AppResources.MaximumSpeed + ": " + track.MaxSpeed.ToString() + " " + AppResources.kmh.ToLower()
                                        + "\n" + AppResources.Distance + ": " + track.Distance.ToString() + " " + AppResources.M.ToLower()
                                        + "\n" + AppResources.SharpFrontalAccelerationMore + ": " + isSharpFrontAccel
                                        + "\n" + AppResources.SharpLateralAccelerationMore + ": " + isSharpLateralAccel
                                        + "\n" + AppResources.SharpBrakingMore + ": " + isSharpBrake
                                        + "\n" + AppResources.Address + ": " + addressMarker);
                                    markerMiddleEnd.Flat(true);
                                    markerMiddleEnd.SetPosition(endPoint);

                                    if (isSharpFrontAccel == AppResources.Yes || isSharpLateralAccel == AppResources.Yes || isSharpBrake == AppResources.Yes)
                                    {
                                        markerMiddleEnd.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.ic_warning));
                                    }
                                    else
                                    {
                                        markerMiddleEnd.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.ic_navigation));
                                    }

                                    markerMiddleEnd.SetRotation(Convert.ToSingle(BearingBetweenLocations(endPoint, nextPoint)));
                                    _map.AddMarker(markerMiddleEnd);
                                    builder.Include(endPoint);
                                }
                            }

                            MarkerOptions markerFinish = new MarkerOptions();
                            if (trip.TimeEnd.HasValue)
                            {
                                markerFinish.SetTitle(AppResources.Time + ": " + DataManager.UnixTimeStampToDateTime(trip.TimeEnd.Value).ToString("HH:mm dd.MM.yyyy"));
                                markerFinish.SetSnippet(AppResources.Address + ": " + addressFinish);
                            }
                            else
                            {
                                markerFinish.SetTitle(AppResources.Address + ": " + addressFinish);
                            }
                            markerFinish.SetPosition(new LatLng(trip.PosEndLat.Value, trip.PosEndLng.Value));
                            markerFinish.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.pin_finish));
                            _map.AddMarker(markerFinish);
                            builder.Include(new LatLng(trip.PosEndLat.Value, trip.PosEndLng.Value));

                            LatLngBounds bounds = builder.Build();
                            if (mapContainer.Height > 350)
                            {                             
                                _map.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 100));
                            }
                            else
                            {
                                _map.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 10));
                            }
                        }
                    }
                    else
                    {
                        Accelerates.Text = "0";
                        Brakes.Text = "0";
                    }
                }
            }

            progressBarLoading.Visibility = ViewStates.Invisible;
            layoutTripData.Visibility = ViewStates.Visible;
        }

        protected override Button GetCancelButton()
        {
            return null;
        }

        protected override Button GetSubmitButton()
        {
            return FindViewById<Button>(Resource.Id.buttonShareResult);
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task OnCancelAsync()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            if (_map != null)
            {
                _map.Snapshot(this);
            }
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            return true;
        }

        public View GetInfoContents(Marker marker)
        {
            LinearLayout info = new LinearLayout(this);
            info.Orientation = Orientation.Vertical;

            TextView title = new TextView(this);
            title.SetTextColor(Color.Black);
            title.Gravity = GravityFlags.Left;
            title.SetText(marker.Title, BufferType.Normal);

            TextView snippet = new TextView(this);
            snippet.SetTextColor(Color.Gray);
            snippet.SetText(marker.Snippet, BufferType.Normal);

            info.AddView(title);
            info.AddView(snippet);

            return info;
        }

        public View GetInfoWindow(Marker marker)
        {
            return null;
        }

        public void OnInfoWindowClick(Marker marker)
        {
            marker.HideInfoWindow();
        }

        private void CreateDirectoryForPictures()
        {
            AppStorage._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "DIFiles");
            if (!AppStorage._dir.Exists())
            {
                AppStorage._dir.Mkdirs();
            }
        }

        string SharingMileage = string.Empty;
        string SharingTripTime = string.Empty;
        string SharingAverageSpeed = string.Empty;

        async Task ProceedSharing(string filename, byte[] filedata)
        {
            var result = await APIDataManager.UploadAttachment(System.String.Format("map_{0}.png", Guid.NewGuid()), "image/png", filedata);
            if (result != null && result.Message != null && result.Message.Count > 0)
            {
                var messageData = result.Message.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(messageData.Value))
                {
                    string message = AppResources.Mileage + ": " + SharingMileage + ", " + AppResources.TripTime + ": " + SharingTripTime + ", " + AppResources.AverageSpeed + ": " + SharingAverageSpeed + ". " + AppResources.DownloadDriverInsuranceSharingTitle + " " + Constants.ANDROID_APP_LINK;

                    Android.Support.V4.App.ShareCompat.IntentBuilder
                    .From(this)
                    .SetText(messageData.Value + " " + message)
                    .SetType("text/plain")
                    .SetChooserTitle(AppResources.ShareResult)
                    .StartChooser();
                }
            }
            _progressBar.Visibility = ViewStates.Gone;
            GetSubmitButton().Visibility = ViewStates.Visible;
        }

        public void OnSnapshotReady(Bitmap snapshot)
        {
            string filePath = string.Empty;

            if (snapshot != null)
            {
                try
                {
                    MemoryStream stream = new MemoryStream();
                    snapshot.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    byte[] bitmapData = stream.ToArray();

                    ProceedSharing(filePath, bitmapData);
                }
                catch (System.Exception e)
                {
                    _progressBar.Visibility = ViewStates.Gone;
                    GetSubmitButton().Visibility = ViewStates.Visible;
                }
            }
        }

        #endregion
    }
}
