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
using Android.Graphics;
using Android.Support.V4.Content;

namespace DI.Droid.Adapters
{
    public class EventRVAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        private List<EventViewModel> mValues;
        Resources mResource;
        private Dictionary<int, int> mCalculatedSizes;
        private Context _context;
        TextView textReadMoreTitle;

        public EventRVAdapter(Context context, List<EventViewModel> items, Resources res)
        {
            _context = context;
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
            var simpleHolder = holder as EventViewHolder;

            simpleHolder.mBoundString = mValues[position].Id.ToString();
            simpleHolder.NumberTitle.Text = AppResources.InsuranceBusinessNumber;
            simpleHolder.Number.Text = mValues[position].Number;
            simpleHolder.DateTitle.Text = AppResources.EventDate;
            simpleHolder.Date.Text = mValues[position].Date;
            simpleHolder.StatusTitle.Text = AppResources.Status;
            simpleHolder.Status.Text = mValues[position].Status;

            if (mValues[position].Status == "Under consideration")
            {
                simpleHolder.Status.SetTextColor(new Color(ContextCompat.GetColor(_context, Resource.Color.text_orange)));
            }
            if (mValues[position].Status == "Direction issued")
            {
                simpleHolder.Status.SetTextColor(new Color(ContextCompat.GetColor(_context, Resource.Color.submit_button_color)));
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_Event, parent, false);
            view.SetBackgroundResource(mBackground);

            return new EventViewHolder(view);
        }
    }

    public class EventViewHolder : BaseListViewHolder
    {
        public readonly TextView NumberTitle;
        public readonly TextView Number;
        public readonly TextView DateTitle;
        public readonly TextView Date;
        public readonly TextView StatusTitle;
        public readonly TextView Status;

        public EventViewHolder(View view) : base(view)
        {
            NumberTitle = view.FindViewById<TextView>(Resource.Id.gridItemNumberTitle);
            Number = view.FindViewById<TextView>(Resource.Id.gridItemNumber);
            DateTitle = view.FindViewById<TextView>(Resource.Id.gridItemDateTitle);
            Date = view.FindViewById<TextView>(Resource.Id.gridItemDate);
            StatusTitle = view.FindViewById<TextView>(Resource.Id.gridItemStatusTitle);
            Status = view.FindViewById<TextView>(Resource.Id.gridItemStatus);
        }
    }
}
