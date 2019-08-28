using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Content.Res;
using DI.Shared.ViewModels;
using DI.Droid;
using DI.Droid.Adapters.Base;
using DI.Localization;

namespace DI.Droid.Adapters
{
    public class BonusLogRVAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        private List<BonusLogItemViewModel> mValues;
        Resources mResource;
        private Dictionary<int, int> mCalculatedSizes;

        TextView textReadMoreTitle;

        public BonusLogRVAdapter(Context context, List<BonusLogItemViewModel> items, Resources res)
        {
            context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
            mBackground = mTypedValue.ResourceId;
            mValues = items;
            mResource = res;

            mCalculatedSizes = new Dictionary<int, int>();
        }

        public override int ItemCount
        {
            get
            {
                return mValues.Count;
            }
        }

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var simpleHolder = holder as BonusLogViewHolder;

            simpleHolder.mBoundString = mValues[position].Id.ToString();
            simpleHolder.Number.Text = mValues[position].Number;
            simpleHolder.Date.Text = mValues[position].Date;
            simpleHolder.Description.Text = mValues[position].Description;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_BonusLogItem, parent, false);

            return new BonusLogViewHolder(view);
        }
    }

    public class BonusLogViewHolder : BaseListViewHolder
    {
        public readonly TextView Number;
        public readonly TextView Date;
        public readonly TextView Description;

        public BonusLogViewHolder(View view) : base(view)
        {
            Number = view.FindViewById<TextView>(Resource.Id.gridItemNumber);
            Date = view.FindViewById<TextView>(Resource.Id.gridItemDate);
            Description = view.FindViewById<TextView>(Resource.Id.gridItemDescription);
        }
    }
}
