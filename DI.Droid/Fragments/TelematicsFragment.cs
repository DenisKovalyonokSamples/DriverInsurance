using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using System.Threading.Tasks;

namespace DI.Droid.Fragments
{
    public class TelematicsFragment : BaseFormFragment
    {
        TextInputLayout editBrandType;
        TextInputLayout editModelType;
        TextInputLayout editDeviceId;
        TextInputLayout editStateType;
        TextInputLayout editDate;

        TextView textCalibrationDescription;
        ScrollView scrollMainContainer;
        LinearLayout layoutMessage;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            editBrandType = partial.FindViewById<TextInputLayout>(Resource.Id.editBrandType);
            editModelType = partial.FindViewById<TextInputLayout>(Resource.Id.editModelType);
            editStateType = partial.FindViewById<TextInputLayout>(Resource.Id.editStateType);
            editDeviceId = partial.FindViewById<TextInputLayout>(Resource.Id.editDeviceId);
            editDate = partial.FindViewById<TextInputLayout>(Resource.Id.editDate);

            textCalibrationDescription = partial.FindViewById<TextView>(Resource.Id.textCalibrationDescription);
            scrollMainContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollMainContainer);
            layoutMessage = partial.FindViewById<LinearLayout>(Resource.Id.layoutMessage);

            InitControls();

            return partial;
        }

        protected override void InitControls()
        {
            base.InitControls();

            editBrandType.Hint = AppResources.Brand;
            editModelType.Hint = AppResources.Model;
            editDeviceId.Hint = AppResources.DeviceId;
            editStateType.Hint = AppResources.CurrentState;
            editDate.Hint = AppResources.InstallDate;

            editBrandType.Enabled = false;
            editModelType.Enabled = false;
            editDeviceId.Enabled = false;
            editDate.Enabled = false;

            scrollMainContainer.Visibility = ViewStates.Gone;

            GetCancelButton().Text = AppResources.StartCalibration.ToUpper();

            textCalibrationDescription.Text = AppResources.CalibrationDescription;

            SetupGestures();
        }

        void SetupGestures()
        {
        }

        #region abstract

        protected override async Task LoadDataAsync()
        {
            if (SessionManager.СontractorData != null)
            {
                var car = await APIDataManager.GetCarByCompany(SessionManager.СontractorData.Id.ToString());
                if (car != null && car.Device.HasValue)
                {
                    var device = await APIDataManager.GetDevice(car.Device.Value.ToString());
                    if (device != null)
                    {
                        editBrandType.EditText.Text = device.Brand;
                        editModelType.EditText.Text = device.Model;
                        editDeviceId.EditText.Text = device.Imei;

                        editStateType.Visibility = ViewStates.Gone;
                        GetCancelButton().Visibility = ViewStates.Gone;
                        layoutMessage.Visibility = ViewStates.Gone;

                        if (car.InstallationDate.HasValue)
                        {
                            editDate.EditText.Text = car.InstallationDate.Value.ToShortDateString();
                        }
                        else
                        {
                            editDate.Visibility = ViewStates.Gone;
                        }
                    }
                }
            }

            scrollMainContainer.Visibility = ViewStates.Visible;
        }

        protected override async Task OnCancelAsync()
        {
            //Start calibration
        }

        protected override async Task OnSubmitAsync()
        {
            //TODO: Save to your API
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            return true;
        }

        protected override Button GetSubmitButton()
        {
            return null;
        }
        protected override Button GetCancelButton()
        {
            return GetPartialView().FindViewById<Button>(Resource.Id.buttonCancel);
        }

        protected override int GetLayoutId()
        {
            return Resource.Layout.Telematics;
        }

        #endregion
    }
}
