using Android.App;
using Android.OS;
using DI.Droid.Base;
using System.Linq;
using DI.Localization;
using Android.Content.PM;
using DI.Shared.ViewModels;
using System.Collections.Generic;
using DI.Droid.Adapters;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using Android.Views;
using DI.Shared.Interfaces;
using Android.Content;
using DI.Shared;
using DI.Droid.Fragments;
using DI.Shared.Enums;
using DI.Shared.DataAccess;
using DI.Shared.Managers;
using System;
using System.Globalization;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class InsuranceCompaniesActivity : BaseListActivity
    {
        List<InsuranceCompanyViewModel> values = new List<InsuranceCompanyViewModel>();

        MenuFragment menuFragment;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.InsuranceCompanies);
            SetTitleBack();

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

            GetEmptyListTextView().Text = AppResources.NoGridItems;      

            SetupGrid();
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
            return AppResources.InsuranceCompanies.ToUpper();
        }

        protected override FloatingActionButton GetFloatingActionButton()
        {
            return null;
        }

        protected override async Task UpdateDataAsync()
        {
            values.Clear();

            var offers = await APIDataManager.GetContractOffers(SessionManager.СontractorData.Id.ToString(), "new");
            if (offers != null && offers.Count > 0)
            {
                foreach (var entity in offers)
                {
                    var contract = await APIDataManager.GetContract(entity.Id.ToString());
                    if (contract != null)
                    {
                        var model = new InsuranceCompanyViewModel();
                        model.Id = entity.Id;
                        model.Name = contract.CompanyName;
                        model.Description = AppResources.PolicyPrice.ToUpper() + ": " + entity.InsurancePremium + " " + AppResources.RUB;

                        double cost = 10000000;
                        if (Double.TryParse(entity.InsurancePremium, NumberStyles.Number, CultureInfo.InvariantCulture, out cost))
                        {
                            model.Cost = cost;
                        }

                        values.Add(model);
                    }
                }

                values = values.OrderBy(e => e.Cost).ToList();
                adapter = new InsuranceCompanyRVAdapter(recyclerView.Context, values, Resources);
                recyclerView.SetAdapter(adapter);
            }

            if (values.Count == 0)
            {
                GetEmptyListTextView().Visibility = ViewStates.Visible;
            }
        }

        protected override void ItemClickedOn(int position)
        {
            if (values.Count == 0)
                return;

            InsuranceCompanyViewModel model = (InsuranceCompanyViewModel)values[position];
            if (model != null)
            {
                var activity = new Intent(this, typeof(InsuranceCompanyOfferActivity));
                activity.PutExtra(Constants.ID, model.Id.ToString());
                activity.PutExtra(Constants.PARENT, ((int)PartialType.InsuranceCompanies).ToString());
                StartActivity(activity);

                Finish();
            }
        }

        protected override ISelectable GetSelectedItem(int position)
        {
            return null;
        }

        protected override async Task AddNewAction()
        {
        }

        protected override async Task EditAction(ISelectable selectedItem)
        {
        }

        protected override async Task DeleteAction(ISelectable selectedItem)
        {
        }

        #endregion
    }
}
