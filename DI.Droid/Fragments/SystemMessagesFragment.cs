using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DI.Droid.Adapters;
using DI.Droid.Fragments.Base;
using DI.Localization;
using DI.Shared.Enums;
using DI.Shared.Interfaces;
using DI.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DI.Droid.Fragments
{
    public class SystemMessagesFragment : BaseListFragment
    {
        List<SystemMessageViewModel> values = new List<SystemMessageViewModel>();

        ImageView imageViewShowMore;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View partial = base.OnCreateView(inflater, container, savedInstanceState);

            imageViewShowMore = partial.FindViewById<ImageView>(Resource.Id.imageViewShowMore);

            InitControls();

            return partial;
        }

        protected override void InitControls()
        {
            base.InitControls();
            imageViewShowMore.Drawable.SetColorFilter(new Color(ContextCompat.GetColor(this.Activity, Resource.Color.content_green)), PorterDuff.Mode.SrcAtop);

            GetEmptyListTextView().Text = AppResources.NoGridItems;

            _adapter = new SystemMessageRVAdapter(_recyclerView.Context, values, Activity.Resources);
            _recyclerView.SetAdapter(_adapter);

            SetupGestures();
        }

        void SetupGestures()
        {
            imageViewShowMore.Click += delegate
            {
                (this.Activity as MainActivity).SetupFragment(PartialType.CurrentRating);
            };
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

            //TODO: Add your API data

            if (values.Count == 0)
            {
                GetEmptyListTextView().Visibility = ViewStates.Visible;
            }
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

        protected override int GetLayoutId()
        {
            return Resource.Layout.SystemMessages;
        }

        #endregion
    }
}
