using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using DI.Droid.Adapters;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared;
using DI.Shared.Interfaces;
using DI.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DI.Droid.Fragments
{
    public class CarsFragment : BaseListFragment
    {
        List<CarViewModel> values = new List<CarViewModel>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);
            InitControls();

            return partial;
        }

        protected override void InitControls()
        {
            base.InitControls();

            GetEmptyListTextView().Text = AppResources.NoGridItems;
            _adapter = new CarRVAdapter(_recyclerView.Context, values, Activity.Resources);
            _recyclerView.SetAdapter(_adapter);
        }

        #region abstract

        protected override FloatingActionButton GetFloatingActionButton()
        {
            return _partial.FindViewById<FloatingActionButton>(Resource.Id.addItemFab);
        }

        protected override Color GetItemBackgroundColor()
        {
            return Color.White;
        }

        protected override Color GetSelectedItemBackgroundColor()
        {
            return Color.LightGray;
        }

        protected override async Task UpdateDataAsync()
        {
            values.Clear();

            if (values.Count == 0)
            {
                GetEmptyListTextView().Visibility = ViewStates.Visible;
            }
        }

        protected override void ItemClickedOn(int position)
        {
            if (values.Count == 0)
                return;

            CarViewModel model = (CarViewModel)values[position];
            if (model != null)
            {
                var activity = new Intent(this.Context, typeof(CarInfoActivity));
                activity.PutExtra(Constants.ID, model.Id.ToString());
                StartActivity(activity);
            }
        }

        protected override ISelectable GetSelectedItem(int position)
        {
            return null;
        }

        protected override async Task AddNewAction()
        {
            var activity = new Intent(this.Activity, typeof(CarInfoActivity));
            StartActivity(activity);
        }

        protected override async Task EditAction(ISelectable selectedItem)
        {
        }

        protected override async Task DeleteAction(ISelectable selectedItem)
        {
        }

        protected override int GetLayoutId()
        {
            return Resource.Layout.Cars;
        }

        #endregion
    }
}
