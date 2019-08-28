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
    public class SystemMessageRVAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        private List<SystemMessageViewModel> mValues;
        Resources mResource;
        private Dictionary<int, int> mCalculatedSizes;

        TextView textReadMoreTitle;

        public SystemMessageRVAdapter(Context context, List<SystemMessageViewModel> items, Resources res)
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
            var simpleHolder = holder as SystemMessageViewHolder;

            simpleHolder.mBoundString = mValues[position].Id.ToString();
            simpleHolder.Description.Text = mValues[position].Description;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_SystemMessage, parent, false);

            return new SystemMessageViewHolder(view);
        }
    }

    public class SystemMessageViewHolder : BaseListViewHolder
    {
        public readonly TextView Description;

        public SystemMessageViewHolder(View view) : base(view)
        {
            Description = view.FindViewById<TextView>(Resource.Id.gridItemDescription);
        }
    }
}
