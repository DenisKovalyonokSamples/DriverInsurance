using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared;
using DI.Shared.DataAccess;
using DI.Shared.Entities.API;
using DI.Shared.Enums;
using DI.Shared.Managers;
using System;
using System.Threading.Tasks;

namespace DI.Droid.Fragments
{
    public class BonusesFragment : BaseFragment
    {
        ProgressBar progressBarLoading;

        LinearLayout layoutBonusScore;
        RelativeLayout layoutGetPoints;
        RelativeLayout layoutRecomendationDescription;
        RelativeLayout layoutUsePoints;
        RelativeLayout layoutFranchiseDescription;
        RelativeLayout layoutTechSupport;
        RelativeLayout layoutTechSupportPlus;

        RelativeLayout layoutMakeRecomendation;
        RelativeLayout layoutBuyFranchise;
        RelativeLayout layoutActivateTechSupport;
        RelativeLayout layoutActivateTechPlusSupport;

        TextView textBonusesForDescription;
        TextView textCurrentBonusesTitle;
        TextView textCurrentBonuses;
        TextView textGetBonusesTitle;
        TextView textBonusesForTitle;
        TextView textBonusesFor;
        TextView textRecomendationBonus;

        TextView textSpentBonusesTitle;

        TextView textBuyFranchiseTitle;
        TextView textFranchiseDescription;
        TextView textFranchiseCost;
        TextView textFranchiseSpendPoints;

        TextView textTechSupportTitle;
        TextView textTechSupportDescription;
        TextView textTechSupportPlusDescription;
        TextView textTechSupportPlusCost;
        TextView textTechSupportPlusSpendPoints;
        TextView textTechSupportCost;
        TextView textTechSupportSpendPoints;

        TextView textSupportPacket;
        TextView textSupportPlusPacket;

        ImageView imageBuyFranchise;
        ImageView imageBuyTechSupport;
        ImageView imageBuyTechSupportPlus;

        ImageView imageFranchiseSpendPoints;
        RelativeLayout layoutFranchiseBought;
        TextView textFranchiseBought;

        ImageView imageTechSupportSpendPoints;
        RelativeLayout layoutTechSupportBought;
        TextView textTechSupportBought;

        ImageView imageTechSupportPlusSpendPoints;
        RelativeLayout layoutTechSupportPlusBought;
        TextView textTechSupportPlusBought;

        ScrollView scrollContainer;
        RelativeLayout layoutFutureBonusesContainer;
        TextView textFutureBonuses;

        string franchiseReason = string.Empty;

        int franchiseCost = 0;
        int currentBonus = 0;

        bool franchiseBought = false;
        bool servicePacketBought = false;
        bool servicePacketPlusBought = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            textFutureBonuses = partial.FindViewById<TextView>(Resource.Id.textFutureBonuses);
            layoutFutureBonusesContainer = partial.FindViewById<RelativeLayout>(Resource.Id.layoutFutureBonusesContainer);

            scrollContainer = partial.FindViewById<ScrollView>(Resource.Id.scrollContainer);
            progressBarLoading = partial.FindViewById<ProgressBar>(Resource.Id.progressBarLoading);
            layoutBonusScore = partial.FindViewById<LinearLayout>(Resource.Id.layoutBonusScore);
            textSupportPacket = partial.FindViewById<TextView>(Resource.Id.textSupportPacket);
            textSupportPlusPacket = partial.FindViewById<TextView>(Resource.Id.textSupportPlusPacket);
            layoutTechSupportPlus = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTechSupportPlus);
            layoutGetPoints = partial.FindViewById<RelativeLayout>(Resource.Id.layoutGetPoints);
            layoutRecomendationDescription = partial.FindViewById<RelativeLayout>(Resource.Id.layoutRecomendationDescription);
            layoutUsePoints = partial.FindViewById<RelativeLayout>(Resource.Id.layoutUsePoints);
            layoutFranchiseDescription = partial.FindViewById<RelativeLayout>(Resource.Id.layoutFranchiseDescription);
            layoutTechSupport = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTechSupport);

            layoutMakeRecomendation = partial.FindViewById<RelativeLayout>(Resource.Id.layoutMakeRecomendation);
            layoutBuyFranchise = partial.FindViewById<RelativeLayout>(Resource.Id.layoutBuyFranchise);
            layoutActivateTechSupport = partial.FindViewById<RelativeLayout>(Resource.Id.layoutActivateTechSupport);
            layoutActivateTechPlusSupport = partial.FindViewById<RelativeLayout>(Resource.Id.layoutActivateTechPlusSupport);

            textCurrentBonusesTitle = partial.FindViewById<TextView>(Resource.Id.textCurrentBonusesTitle);
            textCurrentBonuses = partial.FindViewById<TextView>(Resource.Id.textCurrentBonuses);
            textGetBonusesTitle = partial.FindViewById<TextView>(Resource.Id.textGetBonusesTitle);
            textBonusesForTitle = partial.FindViewById<TextView>(Resource.Id.textBonusesForTitle);
            textBonusesFor = partial.FindViewById<TextView>(Resource.Id.textBonusesFor);
            textBuyFranchiseTitle = partial.FindViewById<TextView>(Resource.Id.textBuyFranchiseTitle);
            textRecomendationBonus = partial.FindViewById<TextView>(Resource.Id.textRecomendationBonus);
            textSpentBonusesTitle = partial.FindViewById<TextView>(Resource.Id.textSpentBonusesTitle);
            textFranchiseDescription = partial.FindViewById<TextView>(Resource.Id.textFranchiseDescription);
            textFranchiseSpendPoints = partial.FindViewById<TextView>(Resource.Id.textFranchiseSpendPoints);
            textFranchiseCost = partial.FindViewById<TextView>(Resource.Id.textFranchiseCost);
            textTechSupportTitle = partial.FindViewById<TextView>(Resource.Id.textTechSupportTitle);
            textTechSupportDescription = partial.FindViewById<TextView>(Resource.Id.textTechSupportDescription);
            textTechSupportSpendPoints = partial.FindViewById<TextView>(Resource.Id.textTechSupportSpendPoints);
            textTechSupportCost = partial.FindViewById<TextView>(Resource.Id.textTechSupportCost);
            textBonusesForDescription = partial.FindViewById<TextView>(Resource.Id.textBonusesForDescription);

            textTechSupportPlusDescription = partial.FindViewById<TextView>(Resource.Id.textTechSupportPlusDescription);
            textTechSupportPlusCost = partial.FindViewById<TextView>(Resource.Id.textTechSupportPlusCost);
            textTechSupportPlusSpendPoints = partial.FindViewById<TextView>(Resource.Id.textTechSupportPlusSpendPoints);

            imageBuyFranchise = partial.FindViewById<ImageView>(Resource.Id.imageBuyFranchise);
            imageBuyTechSupport = partial.FindViewById<ImageView>(Resource.Id.imageBuyTechSupport);
            imageBuyTechSupportPlus = partial.FindViewById<ImageView>(Resource.Id.imageBuyTechSupportPlus);

            layoutFranchiseBought = partial.FindViewById<RelativeLayout>(Resource.Id.layoutFranchiseBought);
            imageFranchiseSpendPoints = partial.FindViewById<ImageView>(Resource.Id.imageFranchiseSpendPoints);
            textFranchiseBought = partial.FindViewById<TextView>(Resource.Id.textFranchiseBought);

            layoutTechSupportBought = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTechSupportBought);
            imageTechSupportSpendPoints = partial.FindViewById<ImageView>(Resource.Id.imageTechSupportSpendPoints);
            textTechSupportBought = partial.FindViewById<TextView>(Resource.Id.textTechSupportBought);

            layoutTechSupportPlusBought = partial.FindViewById<RelativeLayout>(Resource.Id.layoutTechSupportPlusBought);
            imageTechSupportPlusSpendPoints = partial.FindViewById<ImageView>(Resource.Id.imageTechSupportPlusSpendPoints);
            textTechSupportPlusBought = partial.FindViewById<TextView>(Resource.Id.textTechSupportPlusBought);

            InitControls();

            return partial;
        }

        protected void InitControls()
        {
            SetSectorsVisibility(ViewStates.Gone);

            textCurrentBonusesTitle.Text = AppResources.CurrentBonusPoints.ToUpper();
            textGetBonusesTitle.Text = AppResources.GetBonusPoints.ToUpper();
            textBonusesForTitle.Text = AppResources.Recommend + " " + AppResources.AppName; 
            textBonusesFor.Text = AppResources.GetPoints;
            textSpentBonusesTitle.Text = AppResources.SpendBonusPoints.ToUpper();
            textBuyFranchiseTitle.Text = AppResources.BuyFranchise;           
            textFranchiseDescription.Text = AppResources.FranchiseDescription;
            textTechSupportTitle.Text = AppResources.TechnicalSupport;
            textTechSupportDescription.Text = AppResources.TechnicalSupportDescription;
            textTechSupportSpendPoints.Text = AppResources.SpendPoints;
            textFranchiseSpendPoints.Text = AppResources.SpendPoints;
            textBonusesForDescription.Text = AppResources.RecommendAppDescription;
            textTechSupportPlusDescription.Text = AppResources.TechnicalSupportPlusDescription;
            textTechSupportPlusSpendPoints.Text = AppResources.SpendPoints;
            textFutureBonuses.Text = AppResources.FutureBonusesDescription;
            textSupportPacket.Text = AppResources.ServicePacket;
            textSupportPlusPacket.Text = AppResources.ServicePacket + " +";

            if (SessionManager.СontractData != null)
            {
                layoutFutureBonusesContainer.Visibility = ViewStates.Gone;
                LoadBonusData();
            }
            else
            {
                layoutFutureBonusesContainer.Visibility = ViewStates.Gone;
            }

            SetupGestures();
        }

        async Task LoadBonusData()
        {
            try
            {
                SetSectorsVisibility(ViewStates.Gone);
                progressBarLoading.Visibility = ViewStates.Visible;

                var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                if (contract != null && contract.Bonus != null)
                {
                    textCurrentBonuses.Text = contract.Bonus.Replace(".00", "");
                }
                else if (SessionManager.СontractData != null && SessionManager.СontractData.Bonus != null)
                {
                    textCurrentBonuses.Text = SessionManager.СontractData.Bonus.Replace(".00", "");
                }
                else
                {
                    textCurrentBonuses.Text = "0";
                }

                var transactions = await APIDataManager.GetBonusTransactions(SessionManager.СontractData.Id.ToString());

                SetSectorsVisibility(ViewStates.Visible);
                progressBarLoading.Visibility = ViewStates.Gone;

                if (transactions != null)
                {
                    foreach (var transaction in transactions)
                    {
                        if (transaction.Description != null)
                        {
                            if (transaction.Description.Contains(_franchiseReason))
                            {
                                SetFranchiseBoughtPanel();
                            }
                            if (transaction.Description == _servicePacketPlus)
                            {
                                SetTechSupportPlusBoughtPanel();
                            }
                            if (transaction.Description == _servicePacket)
                            {
                                SetTechSupportBoughtPanel();
                            }
                        }
                    }
                }

                if (int.TryParse(textCurrentBonuses.Text, out currentBonus))
                {
                    int kbm;
                    if (SessionManager.СontractorData != null)
                    {
                        SessionManager.СontractData = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                    }
                    if (SessionManager.СontractData.KbmForeseen == null)
                    {
                        SessionManager.СontractData.KbmForeseen = "0.00";
                    }

                    if (int.TryParse(SessionManager.СontractData.KbmForeseen.Replace(".00", ""), out kbm))
                    {
                        franchiseReason = _franchiseReason;
                        franchiseCost = 0;
                        string selectedCost = string.Empty;
                        if (kbm == -3)
                        {
                            selectedCost = "100.000";
                            franchiseReason += "100.000";
                            franchiseCost = CalculateFranchiseBonusCost(100000);
                        }
                        if (kbm == -2)
                        {
                            selectedCost = "50.000";
                            franchiseReason += "50.000";
                            franchiseCost = CalculateFranchiseBonusCost(50000);
                        }
                        if (kbm == -1)
                        {
                            selectedCost = "30.000";
                            franchiseReason += "30.000";
                            franchiseCost = CalculateFranchiseBonusCost(30000);
                        }
                        if (kbm == 0)
                        {
                            selectedCost = "25.000";
                            franchiseReason += "25.000";
                            franchiseCost = CalculateFranchiseBonusCost(25000);
                        }
                        if (kbm == 1)
                        {
                            selectedCost = "20.000";
                            franchiseReason += "20.000";
                            franchiseCost = CalculateFranchiseBonusCost(20000);
                        }
                        if (kbm == 2)
                        {
                            selectedCost = "15.000";
                            franchiseReason += "15.000";
                            franchiseCost = CalculateFranchiseBonusCost(15000);
                        }
                        if (kbm == 3)
                        {
                            selectedCost = "10.000";
                            franchiseReason += "10.000";
                            franchiseCost = CalculateFranchiseBonusCost(10000);
                        }
                        if (kbm == 4)
                        {
                            selectedCost = "5.000";
                            franchiseReason += "5.000";
                            franchiseCost = CalculateFranchiseBonusCost(5000);
                        }

                        if (franchiseCost == 0)
                        {
                            layoutFranchiseDescription.Visibility = ViewStates.Gone;
                            layoutBuyFranchise.Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            textFranchiseDescription.Text = AppResources.ProjectedFranchise + " - " + selectedCost + " " + AppResources.RUB.ToLower() + "\n\n" + AppResources.FranchiseDescription;

                            textFranchiseCost.Text = franchiseCost.ToString();

                            if (currentBonus < franchiseCost)
                            {
                                imageBuyFranchise.SetImageResource(Resource.Mipmap.chat_send_blocked);
                            }
                        }
                    }

                    textTechSupportCost.Text = "100";
                    if (currentBonus < 100)
                    {
                        imageBuyTechSupport.SetImageResource(Resource.Mipmap.chat_send_blocked);
                    }

                    textTechSupportPlusCost.Text = "300";
                    if (currentBonus < 300)
                    {
                        imageBuyTechSupportPlus.SetImageResource(Resource.Mipmap.chat_send_blocked);
                    }
                }
            }
            catch(Exception ex)
            {
                HockeyApp.Android.Metrics.MetricsManager.TrackEvent("LoadBonusData ERROR: " + ex.ToString());
            }
        }

        int CalculateFranchiseBonusCost(int cost)
        {
            int result = 0;

            result = Convert.ToInt32(cost * 0.56 / 10);

            return result;
        }

        void SetSectorsVisibility(ViewStates state)
        {
            layoutGetPoints.Visibility = state;
            layoutRecomendationDescription.Visibility = state;
            layoutMakeRecomendation.Visibility = state;
            layoutUsePoints.Visibility = state;
            layoutFranchiseDescription.Visibility = state;
            layoutBuyFranchise.Visibility = state;
            layoutTechSupport.Visibility = state;
            layoutActivateTechSupport.Visibility = state;
            layoutActivateTechPlusSupport.Visibility = state;
            layoutTechSupportPlus.Visibility = state;
        }

        void SetFranchiseBoughtPanel()
        {
            textFranchiseCost.Visibility = ViewStates.Gone;
            textFranchiseSpendPoints.Visibility = ViewStates.Gone;
            imageFranchiseSpendPoints.Visibility = ViewStates.Gone;
            imageBuyFranchise.Visibility = ViewStates.Gone;
            franchiseBought = true;

            textFranchiseBought.Text = AppResources.Purchased;
            layoutFranchiseBought.Visibility = ViewStates.Visible;
        }

        void SetTechSupportBoughtPanel()
        {
            textTechSupportCost.Visibility = ViewStates.Gone;
            textTechSupportSpendPoints.Visibility = ViewStates.Gone;
            imageTechSupportSpendPoints.Visibility = ViewStates.Gone;
            imageBuyTechSupport.Visibility = ViewStates.Gone;
            servicePacketBought = true;

            textTechSupportBought.Text = AppResources.Purchased;
            layoutTechSupportBought.Visibility = ViewStates.Visible;
        }

        void SetTechSupportPlusBoughtPanel()
        {
            textTechSupportPlusCost.Visibility = ViewStates.Gone;
            textTechSupportPlusSpendPoints.Visibility = ViewStates.Gone;
            imageTechSupportPlusSpendPoints.Visibility = ViewStates.Gone;
            imageBuyTechSupportPlus.Visibility = ViewStates.Gone;
            servicePacketPlusBought = true;

            textTechSupportPlusBought.Text = AppResources.Purchased;
            layoutTechSupportPlusBought.Visibility = ViewStates.Visible;
        }

        void SetupGestures()
        {
            layoutBonusScore.Click += delegate
            {
                var activity = new Intent(this.Activity, typeof(BonusLogActivity));
                StartActivity(activity);
            };

            layoutMakeRecomendation.Click += delegate
            {
                string bonusCode = SessionManager.UserData.UserCode;

                Android.Support.V4.App.ShareCompat.IntentBuilder
                .From(this.Activity)
                .SetText(string.Format(AppResources.ShareAppBody, AppResources.AppName, Constants.ANDROID_APP_LINK, bonusCode))
                .SetType("text/plain")
                .SetChooserTitle(AppResources.Recommend + " " + AppResources.AppName)
                .StartChooser();
            };

            layoutBuyFranchise.Click += delegate
            {
                if (!franchiseBought)
                {
                    if (currentBonus >= franchiseCost)
                    {
                        ShowConfirmDialog(PurchaseType.Franchise);
                    }
                }
            };

            layoutActivateTechSupport.Click += delegate
            {
                if (!servicePacketBought)
                {
                    if (currentBonus >= 100)
                    {
                        ShowConfirmDialog(PurchaseType.SupportPacket);
                    }
                }
            };

            layoutActivateTechPlusSupport.Click += delegate
            {
                if (!servicePacketPlusBought)
                {
                    if (currentBonus >= 300)
                    {
                        ShowConfirmDialog(PurchaseType.SupportPacketPlus);
                    }
                }
            };
        }

        #region Dialogs 

        protected void ShowConfirmDialog(PurchaseType type)
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this.Activity);
            alert.SetTitle(AppResources.Confirmation);

            if (type == PurchaseType.Franchise)
                alert.SetMessage(AppResources.ConfirmFranchiseMessage);
            if (type == PurchaseType.SupportPacket)
                alert.SetMessage(AppResources.ConfirmSupportPacketMessage);
            if (type == PurchaseType.SupportPacketPlus)
                alert.SetMessage(AppResources.ConfirmSupportPacketPlusMessage);

            alert.SetPositiveButton(AppResources.Yes, async (senderAlert, args) =>
            {
                if (type == PurchaseType.Franchise)
                {
                    if (await BuyFranchise(franchiseCost))
                    {
                        LoadBonusData();
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, AppResources.ErrorMessage, ToastLength.Long).Show();
                    }
                }
                if (type == PurchaseType.SupportPacket)
                {
                    if (await BuyTechSupport(false))
                    {
                        LoadBonusData();
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, AppResources.ErrorMessage, ToastLength.Long).Show();
                    }
                }
                if (type == PurchaseType.SupportPacketPlus)
                {
                    if (await BuyTechSupport(true))
                    {
                        LoadBonusData();
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, AppResources.ErrorMessage, ToastLength.Long).Show();
                    }
                }
            });
            alert.SetNegativeButton(AppResources.No, (senderAlert, args) =>
            {
            });

            Android.App.Dialog dialog = alert.Create();
            dialog.Show();
        }

        #endregion

        async Task<bool> BuyFranchise(int sum)
        {
            var entity = new BonusTransaction();
            entity.Contract = SessionManager.СontractData.Id;
            entity.User = SessionManager.UserData.Id;
            entity.Correction = 0;
            entity.TransactionType = "minus";
            entity.Value = sum;
            entity.Description = franchiseReason;

            var result = await APIDataManager.CreateBonusTransaction(entity);
            if (result == null)
                return false;

            return true;
        }

        async Task<bool> BuyTechSupport(bool isPlus)
        {
            var entity = new BonusTransaction();
            entity.Contract = SessionManager.СontractData.Id;
            entity.User = SessionManager.UserData.Id;
            entity.Correction = 0;
            entity.TransactionType = "minus";

            if (isPlus)
            {
                entity.Value = 300;
                entity.Description = _servicePacketPlus;
            }
            else
            {
                entity.Value = 100;
                entity.Description = _servicePacket;
            }

            var result = await APIDataManager.CreateBonusTransaction(entity);
            if (result == null)
                return false;

            return true;
        }
        public override void OnResume()
        {
            base.OnResume();

            if (this.Activity != null && !(this.Activity as MainActivity).isInit)
            {
                if ((this.Activity as MainActivity).ActiveTab != PartialType.Bonuses)
                {
                    (this.Activity as MainActivity).SetMenuSelection(PartialType.Bonuses);
                }
            }
        }

        #region abstract

        protected override int GetLayoutId()
        {
            return Resource.Layout.Bonuses;
        }

        #endregion

        string _servicePacket = "Service Packet ";
        string _servicePacketPlus = "Service Packet +";
        string _franchiseReason = "Redemption Franchise ";
    }

    public enum PurchaseType : int
    {
        Unknown = 0,
        Franchise = 1,
        SupportPacket = 2,
        SupportPacketPlus = 3
    }
}
