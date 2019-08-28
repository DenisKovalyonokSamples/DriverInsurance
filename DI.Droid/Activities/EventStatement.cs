using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using System.Linq;
using DI.Localization;
using Android.Content.PM;
using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Support.V4.Content;
using System.Collections.Generic;
using DI.Droid.Fragments;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Managers;
using Android.Support.Design.Widget;
using Android.Views.InputMethods;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class EventStatementActivity : BaseFormActivity
    {
        List<DictionaryItem> IncidentTypes;
        string SelectedType;

        DatePickerDialog datePickerDialog;
        TextInputLayout editDate;
        TextView editDateVM;
        TextView textEventType;
        Spinner eventTypeSpinner;
        TextView eventTypeVM;
        TextInputLayout editAddress;
        TextView addressVM;
        TextView textSendEventDescription;

        MenuFragment menuFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.EventStatement);
            SetTitleBack();

            textEventType = FindViewById<TextView>(Resource.Id.textEventType);
            editDate = FindViewById<TextInputLayout>(Resource.Id.editDate);
            editDateVM = FindViewById<TextView>(Resource.Id.editDateVM);
            eventTypeSpinner = FindViewById<Spinner>(Resource.Id.eventTypeSpinner);
            eventTypeVM = FindViewById<TextView>(Resource.Id.eventTypeVM);
            editAddress = FindViewById<TextInputLayout>(Resource.Id.editAddress);
            addressVM = FindViewById<TextView>(Resource.Id.addressVM);
            textSendEventDescription = FindViewById<TextView>(Resource.Id.textSendEventDescription);

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

            textEventType.Text = AppResources.EventType;
            editDate.Hint = AppResources.EventDate;
            editAddress.Hint = AppResources.EnterAddress;
            GetSubmitButton().Text = AppResources.Send.ToUpper();

            textSendEventDescription.Text = AppResources.SendEventDescription;

            SetupGestures();
        }

        void SetupGestures()
        {
            editDate.EditText.Touch += SelectDate_OnTouch;
        }

        void SelectDate_OnTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                case MotionEventActions.Down:
                    InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                    imm.HideSoftInputFromWindow(editDate.EditText.WindowToken, 0);

                    editDate.RequestFocus();

                    DateTime selectedDate;
                    if (editDate.EditText.Text == string.Empty)
                        selectedDate = DateTime.Now;
                    else
                        selectedDate = DateTime.Parse(editDate.EditText.Text);

                    datePickerDialog = new DatePickerDialog(this, OnDateSet, selectedDate.Year, selectedDate.Month - 1, selectedDate.Day);
                    datePickerDialog.Show();
                    break;
                default:
                    break;
            }
        }

        public void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            editDate.EditText.Text = e.Date.ToShortDateString();
        }

        void SetSpinnerSelectedItemColor(Spinner control)
        {
            control.SetSelection(0, true);
            View v = control.SelectedView;
            ((TextView)v).SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.description_message_color)));
            ((TextView)v).TextSize = 20;

            control.ItemSelected += delegate
            {
                View view = control.SelectedView;
                ((TextView)view).SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.bgr_white)));
                ((TextView)view).TextSize = 20;

                if (control.SelectedItemPosition == 0)
                {
                    textEventType.Visibility = ViewStates.Invisible;
                    ((TextView)view).SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.description_message_color)));
                }
                else
                {
                    textEventType.Visibility = ViewStates.Visible;
                }
            };
        }

        void spinnerEventType_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            SelectedType = spinner.GetItemAtPosition(e.Position).ToString();
        }

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
            return AppResources.Claim;
        }

        protected override async Task LoadDataAsync()
        {
            List<string> values = new List<string>();
            values.Add(AppResources.EventType);

            IncidentTypes = await APIDataManager.GetDictionaryItems("incidenttype");
            if (IncidentTypes != null && IncidentTypes.Count > 0)
            {
                foreach(var type in IncidentTypes)
                {
                    values.Add(type.Value);
                }
            }

            var adapterEventType = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, values);
            adapterEventType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            eventTypeSpinner.Adapter = adapterEventType;
            SetSpinnerSelectedItemColor(eventTypeSpinner);

            eventTypeSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerEventType_ItemSelected);
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
            var car = await APIDataManager.GetCarByCompany(SessionManager.СontractorData.Id.ToString());
            var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());

            if (car != null && contract != null)
            {
                var entity = new Incident();
                entity.Address = editAddress.EditText.Text;
                entity.Car = car.Id;
                entity.Contract = contract.Id;
                entity.Deleted = 0;
                if (car.Device.HasValue)
                {
                    entity.Device = car.Device.Value;
                }
                entity.IncidentDate = Convert.ToDateTime(editDate.EditText.Text);

                var typeCode = IncidentTypes.Where(e => e.Value == SelectedType).FirstOrDefault();
                entity.IncidentType = typeCode.Code;
                entity.Status = "new";

                var createdEntity = await APIDataManager.CreateIncident(entity);
            }

            Finish();
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            if (editDate.EditText.Text == string.Empty)
            {
                editDateVM.Text = AppResources.EventDateRequired.ToUpper();
                editDateVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                editDateVM.Visibility = ViewStates.Invisible;
            }

            if (eventTypeSpinner.SelectedItemPosition == 0)
            {
                eventTypeVM.Text = AppResources.EventTypeRequired.ToUpper();
                eventTypeVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                eventTypeVM.Visibility = ViewStates.Invisible;
            }

            if (editAddress.EditText.Text == string.Empty)
            {
                addressVM.Text = AppResources.EventAddressRequired.ToUpper();
                addressVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                addressVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            return true;
        }

        #endregion
    }
}
