using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using System;
using System.Threading.Tasks;

namespace DI.Droid.Fragments
{
    public class CarFragment : BaseFormFragment
    {
        TextInputLayout editCarBrand;
        TextInputLayout editCarModel;
        TextInputLayout editYear;
        TextInputLayout editRegistrationNumber;
        EditText editMileage;
        Spinner spinnerAlarmPresence;
        EditText editAlarmBrand;
        EditText editAlarmModel;

        TextView carBrandVM;
        TextView carModelVM;
        TextView yearVM;
        TextView registrationNumberVM;
        TextView alarmPresenceVM;
        TextView mileageVM;
        TextView alarmBrandVM;
        TextView alarmModelVM;

        ScrollView scrollMainContainer;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            editCarBrand = partial.FindViewById<TextInputLayout>(Resource.Id.editCarBrand);
            editCarModel = partial.FindViewById<TextInputLayout>(Resource.Id.editCarModel);
            editYear = partial.FindViewById<TextInputLayout>(Resource.Id.editYear);
            editRegistrationNumber = partial.FindViewById<TextInputLayout>(Resource.Id.editRegistrationNumber);
            editMileage = partial.FindViewById<EditText>(Resource.Id.editMileage);
            spinnerAlarmPresence = partial.FindViewById<Spinner>(Resource.Id.spinnerAlarmPresence);
            editAlarmBrand = partial.FindViewById<EditText>(Resource.Id.editAlarmBrand);
            editAlarmModel = partial.FindViewById<EditText>(Resource.Id.editAlarmModel);

            carBrandVM = partial.FindViewById<TextView>(Resource.Id.carBrandVM);
            carModelVM = partial.FindViewById<TextView>(Resource.Id.carModelVM);
            yearVM = partial.FindViewById<TextView>(Resource.Id.yearVM);
            registrationNumberVM = partial.FindViewById<TextView>(Resource.Id.registrationNumberVM);
            mileageVM = partial.FindViewById<TextView>(Resource.Id.mileageVM);
            alarmPresenceVM = partial.FindViewById<TextView>(Resource.Id.alarmPresenceVM);
            alarmBrandVM = partial.FindViewById<TextView>(Resource.Id.alarmBrandVM);
            alarmModelVM = partial.FindViewById<TextView>(Resource.Id.alarmModelVM);

            scrollMainContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollMainContainer);

            InitControls();

            return partial;
        }

        void InitControls()
        {
            base.InitControls();

            editCarBrand.Hint = AppResources.CarBrand;
            editCarModel.Hint = AppResources.CarModel;
            editYear.Hint = AppResources.CarYear;
            editRegistrationNumber.Hint = AppResources.RegistrationNumber;
            editMileage.Hint = AppResources.Mileage;
            editAlarmBrand.Hint = AppResources.AlarmBrand;
            editAlarmModel.Hint = AppResources.AlarmModel;

            editMileage.Visibility = ViewStates.Gone;
            mileageVM.Visibility = ViewStates.Gone;

            spinnerAlarmPresence.Visibility = ViewStates.Gone;
            alarmPresenceVM.Visibility = ViewStates.Gone;
            editAlarmBrand.Visibility = ViewStates.Gone;
            alarmBrandVM.Visibility = ViewStates.Gone;
            editAlarmModel.Visibility = ViewStates.Gone;
            alarmModelVM.Visibility = ViewStates.Gone;

            scrollMainContainer.Visibility = ViewStates.Gone;

            GetSubmitButton().Text = AppResources.Save.ToUpper();
        }

        void SetSpinnerSelectedItemColor(Spinner control)
        {
            control.SetSelection(0, true);
            View v = control.SelectedView;
            ((TextView)v).SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));

            control.ItemSelected += delegate
            {
                View view = control.SelectedView;
                ((TextView)view).SetTextColor(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.bgr_white)));
            };
        }

        #region abstract

        protected override async Task LoadDataAsync()
        {
            if (SessionManager.СontractorData != null)
            {
                var car = await APIDataManager.GetCarByCompany(SessionManager.СontractorData.Id.ToString()); 
                if (car != null)
                {
                    editCarBrand.EditText.Text = car.Brand;
                    editCarModel.EditText.Text = car.Model;
                    editYear.EditText.Text = car.Year.ToString();
                    editRegistrationNumber.EditText.Text = car.Regnum;
                }

                var quatation = await APIDataManager.GetQuotation(SessionManager.СontractorData.Id.ToString());
                if (quatation != null)
                {
                    LockFields();
                    GetSubmitButton().Visibility = ViewStates.Gone;
                }
                else
                {
                    GetSubmitButton().Visibility = ViewStates.Visible;
                }
            }

            scrollMainContainer.Visibility = ViewStates.Visible;
        }

        void LockFields()
        {
            editCarBrand.Enabled = false;
            editCarModel.Enabled = false;
            editYear.Enabled = false;
            editRegistrationNumber.Enabled = false;
        }

        protected override async Task OnCancelAsync()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            Car createdCar = null;

            if (SessionManager.СontractorData != null)
            {
                var car = new Car();

                var existedCar = await APIDataManager.GetCarByCompany(SessionManager.СontractorData.Id.ToString());
                if (existedCar != null)
                {
                    car = existedCar;
                }

                car.Brand = editCarBrand.EditText.Text;
                car.Model = editCarModel.EditText.Text;
                car.Year = Convert.ToInt32(editYear.EditText.Text);
                car.Regnum = editRegistrationNumber.EditText.Text;
                car.Owner = SessionManager.СontractorData.Id;
                car.Vin = Guid.NewGuid().ToString();
                car.Deleted = 0;

                
                if (existedCar == null)
                {
                    createdCar = await APIDataManager.CreateCar(car);
                }
                else
                {
                    createdCar = await APIDataManager.UpdateCar(car);
                }
            }

            if (createdCar == null)
            {
                Toast.MakeText(this.Activity, AppResources.ErrorMessage, ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this.Activity, AppResources.DataSuccessfulyUpdated, ToastLength.Long).Show();
            }

            HideProgressBar();
            GetSubmitButton().Visibility = ViewStates.Visible;
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            int year;
            int mileage;

            if (editCarBrand.EditText.Text == string.Empty)
            {
                carBrandVM.Text = AppResources.CarBrandRequired.ToUpper();
                carBrandVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                carBrandVM.Visibility = ViewStates.Invisible;
            }

            if (editCarModel.EditText.Text == string.Empty)
            {
                carModelVM.Text = AppResources.CarModelRequired.ToUpper();
                carModelVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                carModelVM.Visibility = ViewStates.Invisible;
            }

            if (editYear.EditText.Text == string.Empty)
            {
                yearVM.Text = AppResources.CarYearRequired.ToUpper();
                yearVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else if (!int.TryParse(editYear.EditText.Text, out year))
            {
                yearVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                yearVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else if (year < 1950 || year > DateTime.Now.Year)
            {
                yearVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                yearVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                yearVM.Visibility = ViewStates.Invisible;
            }

            if (editRegistrationNumber.EditText.Text == string.Empty)
            {
                registrationNumberVM.Text = AppResources.RegistrationNumberRequired.ToUpper();
                registrationNumberVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                registrationNumberVM.Visibility = ViewStates.Invisible;
            }          

            if (hasErrors)
                return false;

            return true;
        }

        protected override Button GetSubmitButton()
        {
            return GetPartialView().FindViewById<Button>(Resource.Id.buttonSave);
        }
        protected override Button GetCancelButton()
        {
            return null;
        }

        protected override int GetLayoutId()
        {
            return Resource.Layout.Car;
        }

        #endregion
    }
}