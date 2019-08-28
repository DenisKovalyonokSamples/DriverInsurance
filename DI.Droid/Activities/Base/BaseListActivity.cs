using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using DI.Shared.Interfaces;
using DI.Droid.Extensions;
using Android.Content;
using System.Threading.Tasks;
using Android.Support.V7.Widget.Helper;
using DI.Droid.Adapters.Callbacks;
using DI.Localization;

namespace DI.Droid.Base
{
    public abstract class BaseListActivity : BaseActivity
    {
        protected FloatingActionButton addNewButton;
        protected RecyclerView recyclerView;
        protected RecyclerView.Adapter adapter;
        public ISelectable selectedItem;
        int? edit, del;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void InitControls()
        {
            base.InitControls();

            if (GetEmptyListTextView() != null)
            {
                GetEmptyListTextView().Text = AppResources.NoGridItems;
            }

            InitGrid();
            InitProgressBars();           
        }

        protected virtual void SetupGrid()
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

            if (addNewButton != null)
            {
                addNewButton.Click += async delegate
                {
                    await AddNewAction();
                };
            }
        }

        protected virtual void InitGrid()
        {
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            addNewButton = GetFloatingActionButton();
        }

        protected override void OnResume()
        {
            base.OnResume();

            UpdateData();
        }

        private async void UpdateData()
        {
            ShowLoadingBar();

            await UpdateDataAsync();

            adapter.NotifyDataSetChanged();
            HideLoadingBar();
        }

        protected virtual TextView GetEmptyListTextView()
        {
            return FindViewById<TextView>(Resource.Id.emptyListText);
        }

        #region Progress Bar Loading

        protected ProgressBar _progressBarLoading;
        protected bool loadingFinished = false;

        protected virtual void InitProgressBars()
        {
            _progressBarLoading = FindViewById<ProgressBar>(Resource.Id.progressBarLoading);
        }

        protected virtual void ShowLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.Indeterminate = true;
            _progressBarLoading.Visibility = ViewStates.Visible;
        }

        protected bool DontHideProgressBar = false;
        protected virtual void HideLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            if (!DontHideProgressBar)
            {
                _progressBarLoading.Visibility = ViewStates.Gone;
            }

        }

        #endregion

        #region abstract

        protected abstract FloatingActionButton GetFloatingActionButton();

        protected abstract Task UpdateDataAsync();

        protected abstract Task EditAction(ISelectable selectedItem);

        protected abstract Task DeleteAction(ISelectable selectedItem);

        protected abstract Task AddNewAction();

        protected abstract void ItemClickedOn(int position);

        protected abstract ISelectable GetSelectedItem(int position);

        #endregion
    }
}
