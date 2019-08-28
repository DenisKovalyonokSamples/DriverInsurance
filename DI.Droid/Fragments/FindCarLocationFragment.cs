using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Shared.DataAccess;
using DI.Shared.Managers;

namespace DI.Droid.Fragments
{
    public class FindCarLocationFragment : BaseFragment, IOnMapReadyCallback
    {
        MapView mapView;
        GoogleMap _map;
        LatLng CurrentLocation;

        bool IsTrakingPosition = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            mapView = partial.FindViewById<MapView>(Resource.Id.map);
            mapView.OnCreate(savedInstanceState);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
        }

        public override void OnResume()
        {
            base.OnResume();

            InitMapFragment();
        }

        #region Map

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

            if (SessionManager.СontractData != null)
            {
                InitCarLocationOnMap();
            }
        }

        async void InitCarLocationOnMap()
        {
            var entity = await APIDataManager.GetCarByCompany(SessionManager.СontractorData.Id.ToString());
            if (entity != null)
            {
                var car = await APIDataManager.GetCarDetails(entity.Id.ToString());
                if (car != null && car.PosEndLat.HasValue && car.PosEndLat.HasValue)
                {
                    if (_map != null)
                    {
                        _map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(car.PosEndLat.Value, car.PosEndLng.Value), 14));

                        if (SessionManager.СontractData == null)
                        {
                            _map.MyLocationEnabled = true;
                        }
                        else
                        {
                            _map.MyLocationEnabled = false;
                        }

                        MarkerOptions marker = new MarkerOptions();
                        marker.SetPosition(new LatLng(car.PosEndLat.Value, car.PosEndLng.Value));
                        marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.pin));
                        _map.AddMarker(marker);

                        return;
                    }
                }
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            if (_map != null)
            {
                try
                {
                    MapsInitializer.Initialize(this.Activity);
                }
                catch (GooglePlayServicesNotAvailableException e)
                {

                }

                _map.UiSettings.ZoomControlsEnabled = true;
                _map.MyLocationEnabled = false;
                _map.MyLocationButtonClick += _map_MyLocationButtonClick;
                _map.MyLocationChange += _map_MyLocationChange;
                _map.CameraChange += delegate
                {
                    var coordinate = _map.CameraPosition.Target;

                    if (coordinate.Longitude == 0 || coordinate.Latitude == 0)
                        return;

                    CurrentLocation = coordinate;
                };

                if (SessionManager.СontractData == null)
                {
                    if (ContextCompat.CheckSelfPermission(this.Activity, Android.Manifest.Permission.AccessFineLocation) == Permission.Granted
                                && ContextCompat.CheckSelfPermission(this.Activity, Android.Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
                    {
                        _map.MyLocationEnabled = true;
                        IsTrakingPosition = true;

                        View locationButton = mapView.FindViewWithTag("GoogleMapMyLocationButton");
                        RelativeLayout.LayoutParams rlp = (RelativeLayout.LayoutParams)locationButton.LayoutParameters;
                    }
                    else
                    {
                        _map.MyLocationEnabled = false;
                    }
                }

                mapView.OnResume();
            }
        }

        private void _map_MyLocationButtonClick(object sender, GoogleMap.MyLocationButtonClickEventArgs e)
        {
            IsTrakingPosition = true;
        }

        private void _map_MyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            CurrentLocation = new LatLng(e.Location.Latitude, e.Location.Longitude);

            if (CurrentLocation.Longitude == 0 || CurrentLocation.Latitude == 0)
                return;

            if (!IsTrakingPosition)
                return;

            if (e.Location.HasAccuracy && e.Location.Accuracy <= 10)
                IsTrakingPosition = false;

            CameraUpdate cameraUpdate1 = CameraUpdateFactory.NewLatLngZoom(CurrentLocation, 17);
            _map.MoveCamera(cameraUpdate1);
        }

        #endregion

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.FindCarLocation;
        }

        #endregion
    }
}
