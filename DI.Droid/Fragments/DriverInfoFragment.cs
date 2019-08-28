using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Views.InputMethods;
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
    public class DriverInfoFragment : BaseFormFragment
    {
        TextInputLayout editSurname;
        TextInputLayout editName;
        TextInputLayout editPatronymic;
        TextInputLayout editDate;
        TextInputLayout editDrivingExperience;
        TextInputLayout editEmail;
        TextInputLayout editPhone;

        TextView surnameVM;
        TextView nameVM;
        TextView patronymicVM;
        TextView editDateVM;
        TextView drivingExperienceVM;
        TextView emailVM;
        TextView phoneVM;

        TextView textNotificationsTitle;
        TextView textNativeNotificationTitle;
        TextView textSMSNotificationTitle;

        CheckBox checkNativeNotification;
        CheckBox checkSMSNotification;

        DatePickerDialog datePickerDialog;
        ScrollView scrollMainContainer;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            editSurname = partial.FindViewById<TextInputLayout>(Resource.Id.editSurname);
            editName = partial.FindViewById<TextInputLayout>(Resource.Id.editName);
            editPatronymic = partial.FindViewById<TextInputLayout>(Resource.Id.editPatronymic);
            editDate = partial.FindViewById<TextInputLayout>(Resource.Id.editDate);
            editDrivingExperience = partial.FindViewById<TextInputLayout>(Resource.Id.editDrivingExperience);
            editEmail = partial.FindViewById<TextInputLayout>(Resource.Id.editEmail);
            editPhone = partial.FindViewById<TextInputLayout>(Resource.Id.editPhone);

            surnameVM = partial.FindViewById<TextView>(Resource.Id.surnameVM);
            nameVM = partial.FindViewById<TextView>(Resource.Id.nameVM);
            patronymicVM = partial.FindViewById<TextView>(Resource.Id.patronymicVM);
            editDateVM = partial.FindViewById<TextView>(Resource.Id.editDateVM);
            drivingExperienceVM = partial.FindViewById<TextView>(Resource.Id.drivingExperienceVM);
            emailVM = partial.FindViewById<TextView>(Resource.Id.emailVM);
            phoneVM = partial.FindViewById<TextView>(Resource.Id.phoneVM);

            scrollMainContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollMainContainer);

            textNotificationsTitle = partial.FindViewById<TextView>(Resource.Id.textNotificationsTitle);
            textNativeNotificationTitle = partial.FindViewById<TextView>(Resource.Id.textNativeNotificationTitle);
            textSMSNotificationTitle = partial.FindViewById<TextView>(Resource.Id.textSMSNotificationTitle);

            checkNativeNotification = partial.FindViewById<CheckBox>(Resource.Id.checkNativeNotification);
            checkSMSNotification = partial.FindViewById<CheckBox>(Resource.Id.checkSMSNotification);

            InitControls();

            return partial;
        }

        protected override void InitControls()
        {
            base.InitControls();

            editSurname.Hint = AppResources.Surname;
            editName.Hint = AppResources.Name;
            editPatronymic.Hint = AppResources.Patronymic;
            editDate.Hint = AppResources.DateOfBirth;
            editDrivingExperience.Hint = AppResources.DrivingExperience;
            editEmail.Hint = AppResources.Email;
            editPhone.Hint = AppResources.Phone;

            textNotificationsTitle.Text = AppResources.SafeModeNotifications.ToUpper();
            textNativeNotificationTitle.Text = AppResources.NativeNotification;
            textSMSNotificationTitle.Text = AppResources.SMSNotification;

            editDrivingExperience.Visibility = ViewStates.Gone;
            drivingExperienceVM.Visibility = ViewStates.Gone;

            scrollMainContainer.Visibility = ViewStates.Gone;
            GetSubmitButton().Text = AppResources.Save.ToUpper();

            SetupGestures();
        }

        void SelectDate_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    InputMethodManager imm = (InputMethodManager)this.Activity.GetSystemService("input_method");
                    imm.HideSoftInputFromWindow(editDate.EditText.WindowToken, 0);

                    editDate.RequestFocus();

                    DateTime selectedDate;
                    if (editDate.EditText.Text == string.Empty)
                        selectedDate = DateTime.Now;
                    else
                        selectedDate = DateTime.Parse(editDate.EditText.Text);

                    datePickerDialog = new DatePickerDialog(this.Activity, OnDateSet, selectedDate.Year, selectedDate.Month - 1, selectedDate.Day);
                    datePickerDialog.Show();
                    break;
                default:
                    break;
            }
        }

        void SetupGestures()
        {
            editDate.EditText.Touch += SelectDate_OnTouch;            
        }

        public void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            editDate.EditText.Text = e.Date.ToShortDateString();
        }

        #region abstract

        protected override async Task LoadDataAsync()
        {           
            if (SessionManager.СontractorData != null)
            {
                editName.EditText.Text = SessionManager.СontractorData.FirstName;
                editSurname.EditText.Text = SessionManager.СontractorData.LastName;
                editPatronymic.EditText.Text = SessionManager.СontractorData.MiddleName;
                editEmail.EditText.Text = SessionManager.СontractorData.Email;

                if (SessionManager.СontractorData.Dob.HasValue)
                {
                    editDate.EditText.Text = SessionManager.СontractorData.Dob.Value.ToShortDateString();
                }
                editPhone.EditText.Text = SessionManager.СontractorData.Phone;
                editPhone.Enabled = false;

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
            editSurname.EditText.Enabled = false;
            editName.Enabled = false;
            editPatronymic.Enabled = false;
            editDate.Enabled = false;
            editDrivingExperience.Enabled = false;
            editEmail.Enabled = false;
            editPhone.Enabled = false;

            checkNativeNotification.Enabled = false;
            checkSMSNotification.Enabled = false;
        }

        protected override async Task OnCancelAsync()
        {
        }

        protected override async Task OnSubmitAsync()
        {
            Company company = null;
            if (SessionManager.СontractorData != null)
            {
                SessionManager.СontractorData.LastName = editSurname.EditText.Text;
                SessionManager.СontractorData.FirstName = editName.EditText.Text;
                SessionManager.СontractorData.MiddleName = editPatronymic.EditText.Text;
                SessionManager.СontractorData.Dob = Convert.ToDateTime(editDate.EditText.Text);
                SessionManager.СontractorData.Email = editEmail.EditText.Text;

                company = await APIDataManager.UpdateCompany(SessionManager.СontractorData);
            }

            if (company == null)
            {
                Toast.MakeText(this.Activity, AppResources.ErrorMessage, ToastLength.Long).Show();
            }
            else
            {
                SessionManager.СontractorData = company;
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

            int age;
            int experience;

            if (editSurname.EditText.Text == string.Empty)
            {
                surnameVM.Text = AppResources.SurnameRequired.ToUpper();
                surnameVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                surnameVM.Visibility = ViewStates.Invisible;
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

            if (editPatronymic.EditText.Text == string.Empty)
            {
                patronymicVM.Text = AppResources.PatronymicRequired.ToUpper();
                patronymicVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                patronymicVM.Visibility = ViewStates.Invisible;
            }

            if (editDate.EditText.Text == string.Empty)
            {
                editDateVM.Text = AppResources.DateOfBirthRequired.ToUpper();
                editDateVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                editDateVM.Visibility = ViewStates.Invisible;
            }

            if (editEmail.EditText.Text == string.Empty)
            {
                emailVM.Text = AppResources.EmailRequired.ToUpper();
                emailVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                emailVM.Visibility = ViewStates.Invisible;
            }

            if (editPhone.EditText.Text == string.Empty)
            {
                phoneVM.Text = AppResources.PhoneRequired.ToUpper();
                phoneVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                phoneVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

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
            return Resource.Layout.DriverInfo;
        }

        #endregion
    }
}
