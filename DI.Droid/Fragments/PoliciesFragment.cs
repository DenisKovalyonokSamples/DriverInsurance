using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared;
using DI.Shared.DataAccess;
using DI.Shared.Interfaces;
using DI.Shared.Managers;
using DI.Shared.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DI.Droid.Extensions;
using DI.Droid.Adapters.Callbacks;
using DI.Shared.Entities.API;
using DI.Droid.Adapters;
using DI.Shared.Enums;
using System.Globalization;

namespace DI.Droid.Fragments
{
    public class PoliciesFragment : BaseFormFragment
    {
        List<InsuranceCompanyViewModel> values = new List<InsuranceCompanyViewModel>();
        protected RecyclerView recyclerView;
        protected RecyclerView.Adapter adapter;
        public ISelectable selectedItem;
        ScrollView layoutContractsContainer;

        RelativeLayout layoutShowAllContractsTitle;
        TextView textShowAllContractsTitle;
        TextView textWeFoundContractsForYou;
        TextView textContractsDeadline;

        RelativeLayout layoutPolicyRules;
        RelativeLayout layoutExtendPolicy;
        RelativeLayout layoutAddPolicyContainer;
        Button layoutGetPolicy;

        ScrollView scrollMainContainer;

        TextView textInsuranceSumTitle;
        TextView textInsuranceSum;
        TextView textInsuranceAwardTitle;
        TextView textInsuranceAward;
        TextView textFranchise;
        TextView textPolicyLimitDateTitle;
        TextView textPolicyLimitDate;
        TextView textPolicyRulesTitle;
        TextView textPolicyCostForecastTitle;
        TextView textPolicyCostForecast;
        TextView textExtendPolicyTitle;
        TextView textNoPolicyTitle;
        TextView textPolicyNumberTitle;
        TextView textPolicyNumber;
        TextView textFranchiseTitle;
        TextView textPreviousContractsTitle;

        TextView textCurrentScoreTitle;
        TextView textCurrentScore;

        ImageView imageViewShowMore;

        string policyConditionsUrl = string.Empty;

        LinearLayout layoutMessageContainer;
        TextView textFutureFeature;

        RelativeLayout layoutCompanyName;
        TextView textCurrentCompanyName;

        LinearLayout layoutServiceConditions;
        TextView textServiceConditionsTitle;
        TextView textServiceConditions;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            layoutServiceConditions = partial.FindViewById<LinearLayout>(Resource.Id.layoutServiceConditions);
            textServiceConditionsTitle = partial.FindViewById<TextView>(Resource.Id.textServiceConditionsTitle);
            textServiceConditions = partial.FindViewById<TextView>(Resource.Id.textServiceConditions);

            layoutCompanyName = partial.FindViewById<RelativeLayout>(Resource.Id.layoutCompanyName);
            textCurrentCompanyName = partial.FindViewById<TextView>(Resource.Id.textCurrentCompanyName);

            layoutPolicyRules = partial.FindViewById<RelativeLayout>(Resource.Id.layoutPolicyRules);
            layoutExtendPolicy = partial.FindViewById<RelativeLayout>(Resource.Id.layoutExtendPolicy);
            layoutAddPolicyContainer = partial.FindViewById<RelativeLayout>(Resource.Id.layoutAddPolicyContainer);
            layoutGetPolicy = partial.FindViewById<Button>(Resource.Id.layoutGetPolicy);
            layoutMessageContainer = partial.FindViewById<LinearLayout>(Resource.Id.layoutMessageContainer);
            layoutContractsContainer = partial.FindViewById<ScrollView>(Resource.Id.layoutContractsContainer);

            scrollMainContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollMainContainer);

            textInsuranceSumTitle = partial.FindViewById<TextView>(Resource.Id.textInsuranceSumTitle);
            textInsuranceSum = partial.FindViewById<TextView>(Resource.Id.textInsuranceSum);
            textInsuranceAwardTitle = partial.FindViewById<TextView>(Resource.Id.textInsuranceAwardTitle);
            textInsuranceAward = partial.FindViewById<TextView>(Resource.Id.textInsuranceAward);
            textFranchise = partial.FindViewById<TextView>(Resource.Id.textFranchise);
            textPolicyLimitDateTitle = partial.FindViewById<TextView>(Resource.Id.textPolicyLimitDateTitle);
            textPolicyLimitDate = partial.FindViewById<TextView>(Resource.Id.textPolicyLimitDate);
            textPolicyRulesTitle = partial.FindViewById<TextView>(Resource.Id.textPolicyRulesTitle);
            textPolicyCostForecastTitle = partial.FindViewById<TextView>(Resource.Id.textPolicyCostForecastTitle);
            textPolicyCostForecast = partial.FindViewById<TextView>(Resource.Id.textPolicyCostForecast);
            textExtendPolicyTitle = partial.FindViewById<TextView>(Resource.Id.textExtendPolicyTitle);
            textNoPolicyTitle = partial.FindViewById<TextView>(Resource.Id.textNoPolicyTitle);
            textPolicyNumberTitle = partial.FindViewById<TextView>(Resource.Id.textPolicyNumberTitle);
            textPolicyNumber = partial.FindViewById<TextView>(Resource.Id.textPolicyNumber);
            textFranchiseTitle = partial.FindViewById<TextView>(Resource.Id.textFranchiseTitle);
            textPreviousContractsTitle = partial.FindViewById<TextView>(Resource.Id.textPreviousContractsTitle);

            textCurrentScoreTitle = partial.FindViewById<TextView>(Resource.Id.textCurrentScoreTitle);
            textCurrentScore = partial.FindViewById<TextView>(Resource.Id.textCurrentScore);

            imageViewShowMore = partial.FindViewById<ImageView>(Resource.Id.imageViewShowMore);

            textFutureFeature = partial.FindViewById<TextView>(Resource.Id.textFutureFeature);

            recyclerView = partial.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            layoutShowAllContractsTitle = partial.FindViewById<RelativeLayout>(Resource.Id.layoutShowAllContractsTitle);
            textShowAllContractsTitle = partial.FindViewById<TextView>(Resource.Id.textShowAllContractsTitle);
            textWeFoundContractsForYou = partial.FindViewById<TextView>(Resource.Id.textWeFoundContractsForYou);
            textContractsDeadline = partial.FindViewById<TextView>(Resource.Id.textContractsDeadline);


            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            textInsuranceSumTitle.Text = AppResources.InsuranceSum;
            textInsuranceAwardTitle.Text = AppResources.InsuranceAward;
            textPolicyLimitDateTitle.Text = AppResources.PolicyLimitDate;
            textPolicyRulesTitle.Text = AppResources.PolicyDetails.ToUpper();
            textPolicyCostForecastTitle.Text = AppResources.PolicyCostForecast;
            textExtendPolicyTitle.Text = AppResources.Extend.ToUpper();
            textNoPolicyTitle.Text = AppResources.NoPolicy;
            layoutGetPolicy.Text = AppResources.GetPolicy.ToUpper();
            textPreviousContractsTitle.Text = AppResources.PreviousContracts;
            textCurrentScoreTitle.Text = AppResources.YourCurrentResult;
            textPolicyNumberTitle.Text = AppResources.PolicyNumber;
            textFranchiseTitle.Text = AppResources.Franchise;
            textServiceConditionsTitle.Text = AppResources.ServiceConditions.ToUpper();
            textFutureFeature.Text = AppResources.QuotationWasCreated;

            textShowAllContractsTitle.Text = AppResources.ViewAllOffers.ToUpper();
            textWeFoundContractsForYou.Text = AppResources.WeFoundContractsForYou;

            imageViewShowMore.Drawable.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.content_green)), PorterDuff.Mode.SrcAtop);

            scrollMainContainer.Visibility = ViewStates.Gone;
            layoutAddPolicyContainer.Visibility = ViewStates.Gone;
            layoutMessageContainer.Visibility = ViewStates.Gone;
            layoutContractsContainer.Visibility = ViewStates.Gone;

            SetupGestures();
        }

        void SetupGestures()
        {
            layoutPolicyRules.Click += delegate
            {
                if (policyConditionsUrl != string.Empty)
                {
                    var activity = new Intent(this.Activity, typeof(PolicyDetailsActivity));
                    activity.PutExtra(Constants.WEBURL, policyConditionsUrl);
                    StartActivity(activity);
                }
            };

            layoutExtendPolicy.Click += delegate
            {
            };

            layoutGetPolicy.Click += delegate
            {
                var activity = new Intent(this.Activity, typeof(MasterDriverInfoActivity));
                StartActivity(activity);
            };

            layoutShowAllContractsTitle.Click += delegate
            {
                var activity = new Intent(this.Activity, typeof(InsuranceCompaniesActivity));
                StartActivity(activity);
            };
        }

        #region Grid

        async Task SetupGrid(List<Contract> data)
        {
            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));

            recyclerView.SetItemClickListener((rv, position, view) =>
            {
                Context context = view.Context;
                ItemClickedOn(position);
            });

            ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(recyclerView.GetAdapter());
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(recyclerView);
            recyclerView.SetItemAnimator(new DefaultItemAnimator());
            recyclerView.SetItemLongClickListener((rv, position, view) =>
            {
                Context context = view.Context;
                selectedItem = GetSelectedItem(position);
            });

            values.Clear();

            List<InsuranceCompanyViewModel> allItems = new List<InsuranceCompanyViewModel>();
            foreach (var entity in data)
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

                    allItems.Add(model);
                }
            }

            allItems = allItems.OrderBy(e => e.Cost).ToList();
            int count = 0;
            foreach (var entity in allItems)
            {
                if (count < 4)
                {
                    values.Add(entity);
                }

                count++;
            }

            adapter = new InsuranceCompanyRVAdapter(recyclerView.Context, values, Resources);
            recyclerView.SetAdapter(adapter);
            adapter.NotifyDataSetChanged();
        }

        void ItemClickedOn(int position)
        {
            if (values.Count == 0)
                return;

            InsuranceCompanyViewModel model = (InsuranceCompanyViewModel)values[position];
            if (model != null)
            {
                var activity = new Intent(this.Activity, typeof(InsuranceCompanyOfferActivity));
                activity.PutExtra(Constants.ID, model.Id.ToString());
                activity.PutExtra(Constants.PARENT, ((int)PartialType.Policies).ToString());
                StartActivity(activity);
            }
        }

        ISelectable GetSelectedItem(int position)
        {
            return null;
        }

        #endregion

        public override void OnResume()
        {
            base.OnResume();

            if (this.Activity != null && !(this.Activity as MainActivity).isInit)
            {
                if ((this.Activity as MainActivity).ActiveTab != PartialType.Policies)
                {
                    (this.Activity as MainActivity).SetMenuSelection(PartialType.Policies);
                }
            }
        }

        #region abstract

        protected override async Task LoadDataAsync()
        {
            if (SessionManager.СontractorData != null)
            {
                textCurrentScore.Text = "0";

                //Check current contract
                var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                if (contract != null)
                {
                    var contractDetails = await APIDataManager.GetContract(contract.Id.ToString());
                    if (contractDetails != null)
                    {
                        textCurrentCompanyName.Text = contractDetails.CompanyName;                       
                    }
                    if (textCurrentCompanyName.Text == string.Empty)
                    {
                        layoutCompanyName.Visibility = ViewStates.Gone;
                    }

                    NumberFormatInfo num = new NumberFormatInfo();
                    num.NumberGroupSeparator = " ";

                    textPolicyNumber.Text = contract.Number;
                    try
                    {
                        textInsuranceSum.Text = Convert.ToDecimal(contract.SumInsured.Replace(".00", "")).ToString("N2", num).Replace(".00", "") + " " + AppResources.RUB.ToLower() + ".";
                    }
                    catch(Exception ex)
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
                    
                    textPolicyLimitDate.Text = AppResources.From.ToLower() + " " + contract.ContractStart.ToString("dd.MM.yyyy") + " " + AppResources.To.ToLower() + " "+ contract.ContractEnd.ToString("dd.MM.yyyy");
                     
                    if ((contract.ContractEnd - DateTime.Now.Date).TotalDays > 14)
                    {
                        layoutExtendPolicy.Visibility = ViewStates.Gone;
                    }

                    if (!string.IsNullOrEmpty(contract.ServiceConditions))
                    {
                        textServiceConditions.Text = contract.ServiceConditions;
                    }
                    else
                    {
                        layoutServiceConditions.Visibility = ViewStates.Gone;
                    }

                    if (!string.IsNullOrEmpty(contract.InsuranceConditions))
                    {
                        policyConditionsUrl = contract.InsuranceConditions;
                    }
                    else
                    {
                        layoutPolicyRules.Visibility = ViewStates.Gone;
                    }

                    scrollMainContainer.Visibility = ViewStates.Visible;
                    layoutAddPolicyContainer.Visibility = ViewStates.Gone;
                    layoutMessageContainer.Visibility = ViewStates.Gone;
                    layoutContractsContainer.Visibility = ViewStates.Gone;
                }
                else
                {
                    var entity = sqliteManager.GetDemoModeData();
                    if (entity != null)
                    {
                        textCurrentScore.Text = entity.DayMark.ToString();
                    }
                    //Check contract offers
                    var offers = await APIDataManager.GetContractOffers(SessionManager.СontractorData.Id.ToString(), "new");
                    if (offers != null && offers.Count > 0)
                    {
                        DateTime endDate = offers.Max(e => e.ContractEnd);
                        textContractsDeadline.Text = AppResources.ContractsDeadline + " " + endDate.ToString("dd.MM.yyyy");

                        await SetupGrid(offers);

                        layoutContractsContainer.Visibility = ViewStates.Visible;
                        scrollMainContainer.Visibility = ViewStates.Gone;
                        layoutMessageContainer.Visibility = ViewStates.Gone;
                        layoutAddPolicyContainer.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        //Check if quatation request sended
                        Quotation quatation = await APIDataManager.GetQuotation(SessionManager.СontractorData.Id.ToString()); //GetQuotationByStatus(SessionManager.СontractorData.Id.ToString(), "new");
                        if (quatation != null)
                        {
                            scrollMainContainer.Visibility = ViewStates.Gone;
                            layoutMessageContainer.Visibility = ViewStates.Visible;
                            layoutAddPolicyContainer.Visibility = ViewStates.Gone;
                            layoutContractsContainer.Visibility = ViewStates.Gone;

                        }
                        else
                        {
                            var readyOffer = await APIDataManager.GetContractOffers(SessionManager.СontractorData.Id.ToString(), "ready");
                            if (readyOffer != null && readyOffer.Count > 0)
                            {
                                textFutureFeature.Text = AppResources.PolicyWasPurchased;

                                scrollMainContainer.Visibility = ViewStates.Gone;
                                layoutMessageContainer.Visibility = ViewStates.Visible;
                                layoutAddPolicyContainer.Visibility = ViewStates.Gone;
                                layoutContractsContainer.Visibility = ViewStates.Gone;
                            }
                            else
                            {
                                //Add quatation
                                _progressBarLoading.Visibility = ViewStates.Gone;
                                scrollMainContainer.Visibility = ViewStates.Gone;
                                layoutMessageContainer.Visibility = ViewStates.Gone;
                                layoutAddPolicyContainer.Visibility = ViewStates.Visible;
                                layoutContractsContainer.Visibility = ViewStates.Gone;
                            }
                        }
                    }
                }
            }
        }

        protected override async Task OnCancelAsync()
        {
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
            return null;
        }

        protected override int GetLayoutId()
        {
            return Resource.Layout.Policies;
        }

        #endregion
    }
}
