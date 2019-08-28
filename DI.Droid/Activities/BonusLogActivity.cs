using Android.App;
using Android.Widget;
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
using DI.Droid.Fragments;
using DI.Shared.Managers;
using DI.Shared.DataAccess;

namespace DI.Droid
{
    [Activity(Label = "DI", ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask, Theme = "@style/MasterLayoutTheme", ConfigurationChanges = (ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale))]
    public class BonusLogActivity : BaseListActivity
    {
        List<BonusLogItemViewModel> values = new List<BonusLogItemViewModel>();

        MenuFragment menuFragment;

        TextView textCurrentBonusesTitle;
        TextView textCurrentBonuses;
        TextView textBonusLogTitle;

        LinearLayout layoutSeparator;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BonusLog);
            SetTitleBack();

            layoutSeparator = FindViewById<LinearLayout>(Resource.Id.layoutSeparator);
            textCurrentBonusesTitle = FindViewById<TextView>(Resource.Id.textCurrentBonusesTitle);
            textCurrentBonuses = FindViewById<TextView>(Resource.Id.textCurrentBonuses);
            textBonusLogTitle = FindViewById<TextView>(Resource.Id.textBonusLogTitle);            

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

            textCurrentBonusesTitle.Text = AppResources.CurrentBonusPoints.ToUpper();

            textCurrentBonuses.Text = "0";
            if (SessionManager.СontractData != null)
            {
                textCurrentBonuses.Text = SessionManager.СontractData.Bonus.Replace(".00", ""); 
            }
            GetEmptyListTextView().Text = AppResources.NoGridItems;

            adapter = new BonusLogRVAdapter(recyclerView.Context, values, Resources);
            recyclerView.SetAdapter(adapter);
            SetupGrid();

            SetupGestures();
        }

        void SetupGestures()
        {

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
            return AppResources.Bonuses;
        }

        protected override FloatingActionButton GetFloatingActionButton()
        {
            return null;
        }

        protected override async Task UpdateDataAsync()
        {
            values.Clear();

            if (SessionManager.СontractData != null)
            {
                var contract = await APIDataManager.GetCurrentContract(SessionManager.СontractorData.Id.ToString());
                if (contract != null)
                {
                    textCurrentBonuses.Text = contract.Bonus.Replace(".00", "");
                }
                else
                {
                    textCurrentBonuses.Text = SessionManager.СontractData.Bonus.Replace(".00", "");
                }

                var transactions = await APIDataManager.GetBonusTransactions(SessionManager.СontractData.Id.ToString());
                if (transactions != null)
                {
                    transactions = transactions.OrderByDescending(e => e.CreatedAt).ToList();
                    foreach (var transaction in transactions)
                    {
                        var item = new BonusLogItemViewModel();
                        item.Id = transaction.Id;
                        if (transaction.TransactionType == "minus")
                        {
                            item.Number = "-" + transaction.Value.ToString();
                        }
                        else
                        {
                            item.Number = "+" + transaction.Value.ToString();
                        }
                        item.Date = transaction.CreatedAt.ToLocalTime().ToString("HH:mm dd.MM.yyyy");
                        item.Description = transaction.Description;
                        values.Add(item);
                    }
                }
            }

            if (values.Count == 0)
            {
                GetEmptyListTextView().Visibility = ViewStates.Visible;
            }
            layoutSeparator.Visibility = ViewStates.Visible;
        }

        protected override void ItemClickedOn(int position)
        {
            if (values.Count == 0)
                return;
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
