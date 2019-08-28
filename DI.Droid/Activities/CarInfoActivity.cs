using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using DI.Shared;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.Runtime;
using Android.Graphics;
using Android.Support.V4.Content;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class CarInfoActivity : BaseFormActivity
    {
        Android.Views.IMenu Menu;

        Spinner spinnerCarBrands;
        Spinner spinnerCarModels;
        Spinner spinnerYearOfIssue;
        Spinner spinnerAlarmPresence;
        Spinner spinnerDeviceBrands;
        Spinner spinnerDeviceTypes;
        Spinner spinnerDeviceModels;

        EditText editMileage;
        EditText editSerialNumber;

        TextView mileageVM;
        TextView serialNumberVM;
        TextView carBrandsVM;
        TextView carModelsVM;
        TextView yearOfIssueVM;
        TextView alarmPresenceVM;

        TextView deviceBrandsVM;
        TextView deviceTypesVM;
        TextView deviceModelsVM;

        bool showDeleteButton = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.CarInfo);
            SetTitleBack();

            spinnerCarBrands = FindViewById<Spinner>(Resource.Id.spinnerCarBrands);
            spinnerCarModels = FindViewById<Spinner>(Resource.Id.spinnerCarModels);
            spinnerYearOfIssue = FindViewById<Spinner>(Resource.Id.spinnerYearOfIssue);
            spinnerAlarmPresence = FindViewById<Spinner>(Resource.Id.spinnerAlarmPresence);
            spinnerDeviceBrands = FindViewById<Spinner>(Resource.Id.spinnerDeviceBrands);
            spinnerDeviceTypes = FindViewById<Spinner>(Resource.Id.spinnerDeviceTypes);
            spinnerDeviceModels = FindViewById<Spinner>(Resource.Id.spinnerDeviceModels);

            editMileage = FindViewById<EditText>(Resource.Id.editMileage);
            editSerialNumber = FindViewById<EditText>(Resource.Id.editSerialNumber);

            mileageVM = FindViewById<TextView>(Resource.Id.mileageVM);
            serialNumberVM = FindViewById<TextView>(Resource.Id.serialNumberVM);
            carBrandsVM = FindViewById<TextView>(Resource.Id.carBrandsVM);
            carModelsVM = FindViewById<TextView>(Resource.Id.carModelsVM);
            yearOfIssueVM = FindViewById<TextView>(Resource.Id.yearOfIssueVM);
            alarmPresenceVM = FindViewById<TextView>(Resource.Id.alarmPresenceVM);
            deviceBrandsVM = FindViewById<TextView>(Resource.Id.deviceBrandsVM);
            deviceTypesVM = FindViewById<TextView>(Resource.Id.deviceTypesVM);
            deviceModelsVM = FindViewById<TextView>(Resource.Id.deviceModelsVM);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            editMileage.Hint = AppResources.Mileage.ToUpper();
            editSerialNumber.Hint = AppResources.SerialNumber.ToUpper();

            GetSubmitButton().Text = AppResources.Save.ToUpper();
        }

        void SetSpinnerSelectedItemColor(Spinner control)
        {
            control.SetSelection(0, true);
            View v = control.SelectedView;
            ((TextView)v).SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.bgr_white)));

            control.ItemSelected += delegate
            {
                View view = control.SelectedView;
                ((TextView)view).SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.bgr_white)));
            };
        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.edit_menu, menu);
            if (menu != null)
            {
                Menu = menu;
                menu.FindItem(Resource.Id.action_delete).SetTitle(AppResources.Delete);

                if (!showDeleteButton)
                {
                    menu.FindItem(Resource.Id.action_delete).SetVisible(false);
                }
            }
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_delete:
                    ShowDeleteItemDialog();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #region Dialogs 

        protected void ShowDeleteItemDialog()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(AppResources.DeleteItemTitle);
            alert.SetMessage(AppResources.DeleteItemMessage);
            alert.SetPositiveButton(AppResources.Yes, async (senderAlert, args) =>
            {
            });
            alert.SetNegativeButton(AppResources.No, (senderAlert, args) =>
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
            return AppResources.Car.ToUpper();
        }

        protected override async Task LoadDataAsync()
        {
            if (string.IsNullOrWhiteSpace(Intent.GetStringExtra(Constants.ID)))
            {
                //TODO: Create Entity
            }
            else
            {
                //TODO: Get Entity

                if (Menu != null)
                {
                    Menu.FindItem(Resource.Id.action_delete).SetVisible(true);
                }
                else
                {
                    showDeleteButton = true;
                }
            }

            JavaList<string> groupCarBrandList = new JavaList<string>();
            groupCarBrandList.Add(AppResources.SelectCarBrand.ToUpper());
            groupCarBrandList.Add("Validation Pass");

            var adapterCarBrand = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, groupCarBrandList);
            adapterCarBrand.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerCarBrands.Adapter = adapterCarBrand;
            SetSpinnerSelectedItemColor(spinnerCarBrands);


            JavaList<string> groupCarModelList = new JavaList<string>();
            groupCarModelList.Add(AppResources.SelectCarModel.ToUpper());
            groupCarModelList.Add("Validation Pass");

            var adapterCarModel = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, groupCarModelList);
            adapterCarModel.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerCarModels.Adapter = adapterCarModel;
            SetSpinnerSelectedItemColor(spinnerCarModels);


            JavaList<string> groupYearOfIssueList = new JavaList<string>();
            groupYearOfIssueList.Add(AppResources.SelectYearOfIssue.ToUpper());
            groupYearOfIssueList.Add("Validation Pass");

            var adapterYearOfIssue = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, groupYearOfIssueList);
            adapterYearOfIssue.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerYearOfIssue.Adapter = adapterYearOfIssue;
            SetSpinnerSelectedItemColor(spinnerYearOfIssue);


            JavaList<string> groupAlarmPresenceList = new JavaList<string>();
            groupAlarmPresenceList.Add(AppResources.AlarmPresence.ToUpper());
            groupAlarmPresenceList.Add(AppResources.Yes);
            groupAlarmPresenceList.Add(AppResources.No);

            var adapterAlarmPresence = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, groupAlarmPresenceList);
            adapterAlarmPresence.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerAlarmPresence.Adapter = adapterAlarmPresence;
            SetSpinnerSelectedItemColor(spinnerAlarmPresence);

            JavaList<string> groupDeviceBrandList = new JavaList<string>();
            groupDeviceBrandList.Add(AppResources.SelectDeviceBrand.ToUpper());
            groupDeviceBrandList.Add("Validation Pass");

            var adapterDeviceBrand = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, groupDeviceBrandList);
            adapterDeviceBrand.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerDeviceBrands.Adapter = adapterDeviceBrand;
            SetSpinnerSelectedItemColor(spinnerDeviceBrands);

            JavaList<string> groupDeviceTypeList = new JavaList<string>();
            groupDeviceTypeList.Add(AppResources.SelectDeviceType.ToUpper());
            groupDeviceTypeList.Add("Validation Pass");

            var adapterDeviceType = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, groupDeviceTypeList);
            adapterDeviceType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerDeviceTypes.Adapter = adapterDeviceType;
            SetSpinnerSelectedItemColor(spinnerDeviceTypes);

            JavaList<string> groupDeviceModelList = new JavaList<string>();
            groupDeviceModelList.Add(AppResources.SelectDeviceModel.ToUpper());
            groupDeviceModelList.Add("Validation Pass");

            var adapterDeviceModel = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, groupDeviceModelList);
            adapterDeviceModel.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerDeviceModels.Adapter = adapterDeviceModel;
            SetSpinnerSelectedItemColor(spinnerDeviceModels);
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
            //TODO: Save to your API

            Finish();
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            double cost;

            if (editMileage.Text == string.Empty)
            {
                mileageVM.Text = AppResources.MileageRequired.ToUpper();
                mileageVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else if (!double.TryParse(editMileage.Text, out cost))
            {
                mileageVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                mileageVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                mileageVM.Visibility = ViewStates.Invisible;
            }

            if (editSerialNumber.Text == string.Empty)
            {
                serialNumberVM.Text = AppResources.SerialNumberRequired.ToUpper();
                serialNumberVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                serialNumberVM.Visibility = ViewStates.Invisible;
            }

            if (spinnerCarBrands.SelectedItemPosition == 0)
            {
                carBrandsVM.Text = AppResources.CarBrandRequired.ToUpper();
                carBrandsVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                carBrandsVM.Visibility = ViewStates.Invisible;
            }

            if (spinnerCarModels.SelectedItemPosition == 0)
            {
                carModelsVM.Text = AppResources.CarModelRequired.ToUpper();
                carModelsVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                carModelsVM.Visibility = ViewStates.Invisible;
            }

            if (spinnerYearOfIssue.SelectedItemPosition == 0)
            {
                yearOfIssueVM.Text = AppResources.YearOfIssueRequired.ToUpper();
                yearOfIssueVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                yearOfIssueVM.Visibility = ViewStates.Invisible;
            }

            if (spinnerAlarmPresence.SelectedItemPosition == 0)
            {
                alarmPresenceVM.Text = AppResources.AlarmPresenceRequired.ToUpper();
                alarmPresenceVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                alarmPresenceVM.Visibility = ViewStates.Invisible;
            }

            if (spinnerDeviceBrands.SelectedItemPosition == 0)
            {
                deviceBrandsVM.Text = AppResources.DeviceBrandRequired.ToUpper();
                deviceBrandsVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                deviceBrandsVM.Visibility = ViewStates.Invisible;
            }

            if (spinnerDeviceTypes.SelectedItemPosition == 0)
            {
                deviceTypesVM.Text = AppResources.DeviceTypeRequired.ToUpper();
                deviceTypesVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                deviceTypesVM.Visibility = ViewStates.Invisible;
            }

            if (spinnerDeviceModels.SelectedItemPosition == 0)
            {
                deviceModelsVM.Text = AppResources.DeviceModelRequired.ToUpper();
                deviceModelsVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                deviceModelsVM.Visibility = ViewStates.Invisible;
            }

            if (hasErrors)
                return false;

            return true;
        }

        #endregion
    }
}
