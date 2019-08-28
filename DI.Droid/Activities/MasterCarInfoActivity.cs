using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using DI.Shared;
using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.Runtime;
using Android.Graphics;
using Android.Support.V4.Content;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class MasterCarInfoActivity : BaseFormActivity
    {
        Spinner spinnerCarBrands;
        Spinner spinnerCarModels;
        Spinner spinnerYearOfIssue;
        Spinner spinnerAlarmPresence;

        EditText editCost;

        TextView costVM;
        TextView carBrandsVM;
        TextView carModelsVM;
        TextView yearOfIssueVM;
        TextView alarmPresenceVM;

        int age;
        int drivingExperience;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MasterCarInfo);
            SetTitleBack();

            age = Convert.ToInt32(Intent.GetStringExtra(Constants.AGE));
            drivingExperience = Convert.ToInt32(Intent.GetStringExtra(Constants.DRIVINGEXPERIENCE));

            spinnerCarBrands = FindViewById<Spinner>(Resource.Id.spinnerCarBrands);
            spinnerCarModels = FindViewById<Spinner>(Resource.Id.spinnerCarModels);
            spinnerYearOfIssue = FindViewById<Spinner>(Resource.Id.spinnerYearOfIssue);
            spinnerAlarmPresence = FindViewById<Spinner>(Resource.Id.spinnerAlarmPresence);

            editCost = FindViewById<EditText>(Resource.Id.editCost);

            costVM = FindViewById<TextView>(Resource.Id.costVM);
            carBrandsVM = FindViewById<TextView>(Resource.Id.carBrandsVM);
            carModelsVM = FindViewById<TextView>(Resource.Id.carModelsVM);
            yearOfIssueVM = FindViewById<TextView>(Resource.Id.yearOfIssueVM);
            alarmPresenceVM = FindViewById<TextView>(Resource.Id.alarmPresenceVM);

            InitControls();
        }

        void InitControls()
        {
            base.InitControls();

            editCost.Hint = AppResources.Cost.ToUpper();

            GetSubmitButton().Text = AppResources.Done.ToUpper();
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
            return AppResources.CarInformation.ToUpper();
        }

        protected override async Task LoadDataAsync()
        {
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
             
            var activity = new Intent(this, typeof(InsuranceCompaniesActivity));
            StartActivity(activity);

            Finish();
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            bool hasErrors = false;

            double cost;

            if (editCost.Text == string.Empty)
            {
                costVM.Text = AppResources.CostRequired.ToUpper();
                costVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else if (!double.TryParse(editCost.Text, out cost))
            {
                costVM.Text = AppResources.IncorrectDataEntered.ToUpper();
                costVM.Visibility = ViewStates.Visible;

                hasErrors = true;
            }
            else
            {
                costVM.Visibility = ViewStates.Invisible;
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

            if (hasErrors)
                return false;

            return true;
        }

        #endregion
    }
}
