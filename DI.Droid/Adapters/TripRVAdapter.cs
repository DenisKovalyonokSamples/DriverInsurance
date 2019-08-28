using Android.Content;
using Android.Content.Res;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using DI.Droid.Adapters.Base;
using DI.Localization;
using DI.Shared.ViewModels;
using DI.Droid;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Droid.Adapters
{
    public class TripRVAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        private List<TripViewModel> mValues;
        Resources mResource;
        private Dictionary<int, int> mCalculatedSizes;

        TextView textReadMoreTitle;

        public TripRVAdapter(Context context, List<TripViewModel> items, Resources res)
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
            var simpleHolder = holder as TripViewHolder;

            simpleHolder.mBoundString = mValues[position].Id.ToString();
            if (mValues[position].Id != 0)
            {
                simpleHolder.TripNumber.Text = AppResources.TripId + " " + mValues[position].TripNumber.ToString();
                simpleHolder.StartAddress.Text = mValues[position].AddressStart;
                simpleHolder.Mileage.Text = mValues[position].Distance.ToString() + " " + AppResources.km.ToLower();
                simpleHolder.MileageTitle.Text = AppResources.Mileage;
                simpleHolder.Time.Text = mValues[position].Duration.ToString() + " " + AppResources.Min.ToLower();
                simpleHolder.TimeTitle.Text = AppResources.Time;
                simpleHolder.AvgSpeed.Text = mValues[position].AvgSpeed.ToString() + " " + AppResources.kmh.ToLower();
                simpleHolder.SpeedTitle.Text = AppResources.AverageSpeed;

                if (mValues[position].SharpAccelCount == 0)
                {
                    simpleHolder.AcceleratesCircle.SetImageResource(Resource.Mipmap.circle_white);
                }
                else
                {
                    simpleHolder.AcceleratesCircle.SetImageResource(Resource.Mipmap.circle_orange);
                }

                if (mValues[position].SharpBrakeCount == 0)
                {
                    simpleHolder.BrakesCircle.SetImageResource(Resource.Mipmap.circle_white);
                }
                else
                {
                    simpleHolder.BrakesCircle.SetImageResource(Resource.Mipmap.circle_orange);
                }

                simpleHolder.Accelerates.Text = mValues[position].SharpAccelCount.ToString();
                simpleHolder.AcceleratesTitle.Text = AppResources.SharpAcceleration;
                simpleHolder.Brakes.Text = mValues[position].SharpBrakeCount.ToString();
                simpleHolder.BrakesTitle.Text = AppResources.SharpBraking;
                simpleHolder.MaxSpeed.Text = mValues[position].MaxSpeed.ToString() + " " + AppResources.kmh.ToLower();
                simpleHolder.MaxSpeedTitle.Text = AppResources.MaximumSpeed;
                simpleHolder.FinishAddress.Text = mValues[position].AddressFinish;
                simpleHolder.TimeStart.Text = mValues[position].TimeStart;
                simpleHolder.TimeFinish.Text = mValues[position].TimeFinish;
            }
            else
            {
                simpleHolder.TripNumber.Text = mValues[position].Date.Date.ToString("dd.MM.yyyy");
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = null;

            if (viewType == 1)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_Time, parent, false);
            }
            if (viewType == 2)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_Track, parent, false);
            }

            view.SetBackgroundResource(mBackground);

            return new TripViewHolder(view);
        }

        public override int GetItemViewType(int position)
        {
            if (mValues[position].Id == 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }

    public class TripViewHolder : BaseListViewHolder
    {
        public readonly TextView TripNumber;
        public readonly TextView StartAddress;
        public readonly TextView Mileage;
        public readonly TextView MileageTitle;
        public readonly TextView Time;
        public readonly TextView TimeTitle;
        public readonly TextView AvgSpeed;
        public readonly TextView SpeedTitle;

        public readonly TextView Accelerates;
        public readonly TextView AcceleratesTitle;
        public readonly TextView Brakes;
        public readonly TextView BrakesTitle;
        public readonly TextView MaxSpeed;
        public readonly TextView MaxSpeedTitle;
        public readonly TextView FinishAddress;

        public readonly TextView TimeStart;
        public readonly TextView TimeFinish;
        public readonly ImageView AcceleratesCircle;
        public readonly ImageView BrakesCircle;

        public TripViewHolder(View view) : base(view)
        {
            TripNumber = view.FindViewById<TextView>(Resource.Id.gridItemTripId);
            StartAddress = view.FindViewById<TextView>(Resource.Id.gridItemStartAddress);
            Mileage = view.FindViewById<TextView>(Resource.Id.gridItemMileage);
            MileageTitle = view.FindViewById<TextView>(Resource.Id.gridItemMileageTitle);
            Time = view.FindViewById<TextView>(Resource.Id.gridItemTime);
            TimeTitle = view.FindViewById<TextView>(Resource.Id.gridItemTimeTitle);
            AvgSpeed = view.FindViewById<TextView>(Resource.Id.gridItemAvgSpeed);
            SpeedTitle = view.FindViewById<TextView>(Resource.Id.gridItemAvgSpeedTitle);
            Accelerates = view.FindViewById<TextView>(Resource.Id.gridItemAccelerates);
            AcceleratesTitle = view.FindViewById<TextView>(Resource.Id.gridItemAcceleratesTitle);
            Brakes = view.FindViewById<TextView>(Resource.Id.gridItemBrakes);
            BrakesTitle = view.FindViewById<TextView>(Resource.Id.gridItemBrakesTitle);
            MaxSpeed = view.FindViewById<TextView>(Resource.Id.gridItemMaxSpeed);
            MaxSpeedTitle = view.FindViewById<TextView>(Resource.Id.gridItemMaxSpeedTitle);
            FinishAddress = view.FindViewById<TextView>(Resource.Id.gridItemFinishAddress);
            TimeStart = view.FindViewById<TextView>(Resource.Id.gridItemTimeStart);
            TimeFinish = view.FindViewById<TextView>(Resource.Id.gridItemTimeFinish);
            AcceleratesCircle = view.FindViewById<ImageView>(Resource.Id.gridItemAcceleratesCircle);
            BrakesCircle = view.FindViewById<ImageView>(Resource.Id.gridItemBrakesCircle);
        }
    }
}
