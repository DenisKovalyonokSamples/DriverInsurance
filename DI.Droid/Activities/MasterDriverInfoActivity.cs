using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Views;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using System;
using DI.Shared.DataAccess;
using Android.Support.Design.Widget;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class MasterDriverInfoActivity : BaseFormActivity
    {
        TextInputLayout editAge;
        TextInputLayout editDrivingExperience;
        TextInputLayout editName;
        TextInputLayout editCarBrand;
        TextInputLayout editCarModel;
        TextInputLayout editYear;
        TextInputLayout editRegistrationNumber;
        TextInputLayout editRegion;

        TextView ageVM;
        TextView drivingExperienceVM;
        TextView nameVM;
        TextView carBrandVM;
        TextView carModelVM;
        TextView yearVM;
        TextView registrationNumberVM;
        TextView regionVM;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MasterDriverInfo);
            SetTitleBack();

            editAge = FindViewById<TextInputLayout>(Resource.Id.editAge);
            editDrivingExperience = FindViewById<TextInputLayout>(Resource.Id.editDrivingExperience);
            editName = FindViewById<TextInputLayout>(Resource.Id.editName);
            editCarBrand = FindViewById<TextInputLayout>(Resource.Id.editCarBrand);
            editCarModel = FindViewById<TextInputLayout>(Resource.Id.editCarModel);
            editYear = FindViewById<TextInputLayout>(Resource.Id.editYear);
            editRegistrationNumber = FindViewById<TextInputLayout>(Resource.Id.editRegistrationNumber);
            editRegion = FindViewById<TextInputLayout>(Resource.Id.editRegion);

            ageVM = FindViewById<TextView>(Resource.Id.ageVM);
            drivingExperienceVM = FindViewById<TextView>(Resource.Id.drivingExperienceVM);
            nameVM = FindViewById<TextView>(Resource.Id.nameVM);
            carBrandVM = FindViewById<TextView>(Resource.Id.carBrandVM);
            carModelVM = FindViewById<TextView>(Resource.Id.carModelVM);
            yearVM = FindViewById<TextView>(Resource.Id.yearVM);
            registrationNumberVM = FindViewById<TextView>(Resource.Id.registrationNumberVM);
            regionVM = FindViewById<TextView>(Resource.Id.regionVM);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            editAge.Hint = AppResources.Age;
            editDrivingExperience.Hint = AppResources.DrivingExperience;
            editName.Hint = AppResources.WhatIsYourName;
            editCarBrand.Hint = AppResources.CarBrand;
            editCarModel.Hint = AppResources.CarModel;
            editYear.Hint = AppResources.CarYear;
            editRegistrationNumber.Hint = AppResources.RegistrationNumber;

            editRegion.Hint = AppResources.OperationArea;
            editRegion.EditText.Text = "New York";
            editRegion.Enabled = false;

            GetSubmitButton().Text = AppResources.SendRequest.ToUpper();
        }

        #region Dialogs 

        protected void ShowErrorItemDialog()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(AppResources.Error);
            alert.SetMessage(AppResources.ErrorMessage);
            alert.SetNegativeButton(AppResources.Apply.ToUpper(), (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
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
            return AppResources.DriverInformation.ToUpper();
        }

        protected override async Task LoadDataAsync()
        {
        }

        protected override Button GetCancelButton()
        {
            return null;
        }

        protected override Button GetSubmitButton()
        {
            return FindViewById<Button>(Resource.Id.buttonSave);
        }

        protected override void OnDataNotValid()
        {
        }

        protected override async Task OnCancelAsync()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            bool success = false;

            SessionManager.СontractorData.FirstName = editName.EditText.Text;
            SessionManager.СontractorData = await APIDataManager.UpdateCompany(SessionManager.СontractorData);

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

            Car createdCar = null;
            if (existedCar == null)
            {
                createdCar = await APIDataManager.CreateCar(car);
            }
            else
            {
                createdCar = await APIDataManager.UpdateCar(car);
            }

            if (createdCar != null)
            {
                var entity = new Quotation();
                entity.Car = createdCar.Id;
                entity.Client = SessionManager.СontractorData.Id;
                entity.Status = "new";
                entity.DriverAge = Convert.ToInt32(editAge.EditText.Text);
                entity.DriverExp = Convert.ToInt32(editDrivingExperience.EditText.Text);
                entity.Region = editRegion.EditText.Text;
                entity.Deleted = 0;

                Quotation createdQuotation = await APIDataManager.CreateQuotation(entity);
                if (createdQuotation != null)
                {
                    success = true;
                }
            } 
            
            if (success)
            {
                Finish();
            }
            else
            {
                ShowErrorItemDialog();
                HideProgressBar();
                GetSubmitButton().Visibility = ViewStates.Visible;
            }
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            int age;
            int experience;
            int year;

            if (editAge.EditText.Text == string.Empty)
            {
                ageVM.Text = AppResources.AgeRequired.ToUpper();
                ageVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else if (!int.TryParse(editAge.EditText.Text, out age))
            {
                ageVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                ageVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                ageVM.Visibility = ViewStates.Invisible;
            }

            if (editDrivingExperience.EditText.Text == string.Empty)
            {
                drivingExperienceVM.Text = AppResources.DrivingExperienceRequired.ToUpper();
                drivingExperienceVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else if (!int.TryParse(editDrivingExperience.EditText.Text, out experience))
            {
                drivingExperienceVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                drivingExperienceVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                drivingExperienceVM.Visibility = ViewStates.Invisible;
            }

            if (editName.EditText.Text == string.Empty)
            {
                nameVM.Text = AppResources.NameRequired.ToUpper();
                nameVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                nameVM.Visibility = ViewStates.Invisible;
            }

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

            if (editRegion.EditText.Text == string.Empty)
            {
                regionVM.Text = AppResources.OperationAreaRequired.ToUpper();
                regionVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                regionVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            var car = await APIDataManager.GetCarByRegNumber(editRegistrationNumber.EditText.Text);
            if (car != null)
            {
                registrationNumberVM.Text = AppResources.RegistrationNumberAlreadyRegistered.ToUpper();
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

        #endregion
    }
}
