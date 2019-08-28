using Android.App;
using Android.Widget;
using Android.OS;
using DI.Droid.Base;
using DI.Localization;
using Android.Content.PM;
using DI.Droid.Fragments;
using Android.Views;
using Android.Content;
using System;
using DI.Shared;
using DI.Shared.Enums;
using System.Threading.Tasks;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using DI.Shared.Entities.API;
using System.Globalization;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class InsuranceCompanyOfferActivity : BaseFormActivity
    {
        Contract CurrentCompany;

        Button layoutBuyPolicy;
        RelativeLayout layoutCompanyName;

        TextView textCurrentCompanyName;

        TextView textInsuranceSumTitle;
        TextView textInsuranceSum;
        TextView textFranchiseTitle;
        TextView textFranchise;
        TextView textInsuranceAwardTitle;
        TextView textInsuranceAward;

        TextView textPolicyPeriodTitle;
        TextView textPolicyPeriod;
        TextView textServiceСonditionsTitle;
        TextView textServiceСonditionsDescription;

        ScrollView scrollMainContainer;

        MenuFragment menuFragment;

        int companyId;
        int parentView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.InsuranceCompanyOffer);
            SetTitleBack();

            companyId = Convert.ToInt32(Intent.GetStringExtra(Constants.ID));
            parentView = Convert.ToInt32(Intent.GetStringExtra(Constants.PARENT));

            layoutBuyPolicy = FindViewById<Button>(Resource.Id.layoutBuyPolicy);
            scrollMainContainer = FindViewById<ScrollView>(Resource.Id.scrollMainContainer);
            textCurrentCompanyName = FindViewById<TextView>(Resource.Id.textCurrentCompanyName);
            textInsuranceSumTitle = FindViewById<TextView>(Resource.Id.textInsuranceSumTitle);
            textInsuranceSum = FindViewById<TextView>(Resource.Id.textInsuranceSum);
            textInsuranceAwardTitle = FindViewById<TextView>(Resource.Id.textInsuranceAwardTitle);
            textInsuranceAward = FindViewById<TextView>(Resource.Id.textInsuranceAward);
            textFranchiseTitle = FindViewById<TextView>(Resource.Id.textFranchiseTitle);
            textFranchise = FindViewById<TextView>(Resource.Id.textFranchise);
            textPolicyPeriodTitle = FindViewById<TextView>(Resource.Id.textPolicyPeriodTitle);
            textPolicyPeriod = FindViewById<TextView>(Resource.Id.textPolicyPeriod);
            textServiceСonditionsTitle = FindViewById<TextView>(Resource.Id.textServiceСonditionsTitle);
            textServiceСonditionsDescription = FindViewById<TextView>(Resource.Id.textServiceСonditionsDescription);

            layoutCompanyName = FindViewById<RelativeLayout>(Resource.Id.layoutCompanyName);

            menuFragment = new MenuFragment();
            if (!this.IsFinishing)
            {
                var partialMenuSetup = SupportFragmentManager.BeginTransaction();
                partialMenuSetup.Add(Resource.Id.fragmentMenu, menuFragment, "MenuFragment");
                partialMenuSetup.CommitAllowingStateLoss();
            }

            InitControls();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (parentView == (int)PartialType.Policies)
                    {
                        Finish();
                    }
                    if (parentView == (int)PartialType.InsuranceCompanies)
                    {
                        var activity = new Intent(this, typeof(InsuranceCompaniesActivity));
                        StartActivity(activity);

                        Finish();
                    }

                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        void InitControls()
        {
            base.InitControls();

            scrollMainContainer.Visibility = ViewStates.Gone;
            textInsuranceSumTitle.Text = AppResources.InsuranceSum;
            textInsuranceAwardTitle.Text = AppResources.InsuranceAward;
            textFranchiseTitle.Text = AppResources.Franchise;
            textPolicyPeriodTitle.Text = AppResources.IncurancePeriod;
            textServiceСonditionsTitle.Text = AppResources.ServiceConditions.ToUpper();
            layoutBuyPolicy.Text = AppResources.BuyIncurance.ToUpper();

            GetSubmitButton().Text = AppResources.BuyIncurance.ToUpper();

            SetupGestures();
        }

        void SetupGestures()
        {
            layoutBuyPolicy.Click += delegate
            {
                //TODO: Buy incurance
            };

            layoutCompanyName.Click += delegate
            {
                if (CurrentCompany != null && CurrentCompany.CompanyDesrciption != null && CurrentCompany.CompanyDesrciption != string.Empty)
                {
                    ShowErrorItemDialog(CurrentCompany.CompanyName, CurrentCompany.CompanyDesrciption);
                }
            };
        }

        #region Dialogs 

        protected void ShowErrorItemDialog(string title, string message)
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetNegativeButton(AppResources.Close.ToUpper(), (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }

        #endregion

        #region abstract

        protected override async Task LoadDataAsync()
        {
            var entity = await APIDataManager.GetContractOffer(companyId);
            if (entity != null)
            {
                var contract = await APIDataManager.GetContract(entity.Id.ToString());
                if (contract != null)
                {
                    textCurrentCompanyName.Text = contract.CompanyName;

                    NumberFormatInfo num = new NumberFormatInfo();
                    num.NumberGroupSeparator = " ";

                    try
                    {
                        textInsuranceSum.Text = Convert.ToDecimal(contract.SumInsured.Replace(".00", "")).ToString("N2", num).Replace(".00", "") + " " + AppResources.RUB.ToLower() + ".";
                    }
                    catch (Exception ex)
                    {
                        textInsuranceSum.Text = contract.SumInsured + " " + AppResources.RUB.ToLower() + ".";
                    }
                    try
                    {
                        textFranchise.Text = Convert.ToDecimal(contract.Franchise.Replace(".00", "")).ToString("N2", num).Replace(".00", "") + " " + AppResources.RUB.ToLower() + ".";
                    }
                    catch (Exception ex)
                    {
                        textFranchise.Text = contract.Franchise + " " + AppResources.RUB.ToLower() + ".";
                    }
                    try
                    {
                        textInsuranceAward.Text = Convert.ToDecimal(contract.InsurancePremium.Replace(".00", "")).ToString("N2", num).Replace(".00", "") + " " + AppResources.RUB.ToLower() + ".";
                    }
                    catch (Exception ex)
                    {
                        textInsuranceAward.Text = contract.InsurancePremium + " " + AppResources.RUB.ToLower() + ".";
                    }

                    var diffMonths = (entity.ContractEnd.Month + entity.ContractEnd.Year * 12) - (entity.ContractStart.Month + entity.ContractStart.Year * 12);
                    textPolicyPeriod.Text = diffMonths.ToString() + " " + AppResources.MON.ToLower();

                    textServiceСonditionsDescription.Text = entity.ServiceConditions;

                    CurrentCompany = contract;

                    scrollMainContainer.Visibility = ViewStates.Visible;
                }
            }
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
            var offers = await APIDataManager.GetContractOffers(SessionManager.СontractorData.Id.ToString(), "new");
            if (offers != null && offers.Count > 0)
            {
                foreach(var entity in offers)
                {
                    if (entity.Id == companyId)
                    {
                        var update = new StatusValue();
                        update.Status = "ready";

                        var result = await APIDataManager.UpdateContract(entity, update);
                    }
                }
            }

            Finish();
        }

        protected override async Task<bool> IsDataValidAsync()
        {
            return true;
        }

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
            return AppResources.Offer;
        }

        #endregion
    }
}
