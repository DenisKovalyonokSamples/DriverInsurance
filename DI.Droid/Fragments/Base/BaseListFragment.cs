using System.Threading.Tasks;
using Android.Graphics;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using DI.Shared.Interfaces;
using DI.Droid.Adapters.Callbacks;
using DI.Droid.Extensions;

namespace DI.Droid.Fragments.Base
{
    public abstract class BaseListFragment : BaseFragment
    {
        protected FloatingActionButton _addNewButton;
        protected RecyclerView _recyclerView;
        protected RecyclerView.Adapter _adapter;
        public ISelectable _selectedItem;
        int? edit, del;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);
            _recyclerView = partial.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _progressBarLoading = partial.FindViewById<ProgressBar>(Resource.Id.progressBarLoading);

            _addNewButton = GetFloatingActionButton();

            return partial;
        }

        protected virtual void InitControls()
        {
            _recyclerView.SetLayoutManager(new LinearLayoutManager(_recyclerView.Context));

            _recyclerView.SetItemClickListener((rv, position, view) =>
            {
                Context context = view.Context;
                ItemClickedOn(position);
            });

            ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(_recyclerView.GetAdapter());
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(_recyclerView);
            _recyclerView.SetItemAnimator(new DefaultItemAnimator());
            _recyclerView.SetItemLongClickListener((rv, position, view) =>
            {
                Context context = view.Context;
                _selectedItem = GetSelectedItem(position);
            });

            if (_addNewButton != null)
            {
                _addNewButton.Click += async delegate
                {
                    await AddNewAction();
                };
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnResume()
        {
            base.OnResume();

            if (!this.IsHidden)
                UpdateData();
        }

        protected async void UpdateData()
        {
            ShowLoadingBar();

            await UpdateDataAsync();

            _adapter.NotifyDataSetChanged();
            HideLoadingBar();
        }

        protected virtual TextView GetEmptyListTextView()
        {
            return GetPartialView().FindViewById<TextView>(Resource.Id.emptyListText);
        }

        #region Progress Bar Loading

        protected ProgressBar _progressBarLoading;
        protected bool loadingFinished = false;

        protected virtual void ShowLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.Indeterminate = true;
            _progressBarLoading.Visibility = ViewStates.Visible;
        }

        protected virtual void HideLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.Visibility = ViewStates.Gone;

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

        protected abstract Color GetItemBackgroundColor();

        protected abstract Color GetSelectedItemBackgroundColor();

        #endregion
    }
}
