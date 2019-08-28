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
using DI.Shared.Entities.Smooch;
using System;
using DI.Shared.Managers;
using DI.Localization;

namespace DI.Droid.Adapters
{
    public class SupportRVAdapter : RecyclerView.Adapter
    {
        private readonly TypedValue mTypedValue = new TypedValue();
        private int mBackground;
        public List<Message> mValues;
        Resources mResource;
        private Dictionary<int, int> mCalculatedSizes;
        Context currentContext;

        public SupportRVAdapter(Context context, List<Message> items, Resources res)
        {
            currentContext = context;
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

        public override int GetItemViewType(int position)
        {
            if (mValues[position].Role == "appUser")
            {
                if (mValues[position].Text != null)
                {
                    if (mValues[position].Text.Contains("dcre-artifacts.s3.amazonaws.com") || mValues[position].Text.Contains("/storage/exchange/"))
                    {
                        return 3;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if(mValues[position].Role == "appMaker")
            {
                return 1;
            }
            else if(mValues[position].Role == "timeSeparator")
            {
                return 2;
            }
            else if (mValues[position].Role == "LocalFileForUpload")
            {
                return 3;
            }

            return 5;
        }

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var simpleHolder = holder as SupportViewHolder;

            if (simpleHolder.mBoundString != null)
            {
                simpleHolder.mBoundString = mValues[position].Id.ToString();
            }
            if (simpleHolder.UserName != null)
            {
                simpleHolder.UserName.Text = AppResources.AppName; 
            }
            if (simpleHolder.Message != null)
            {
                simpleHolder.Message.Text = mValues[position].Text;
                simpleHolder.Time.Text = DataManager.UnixTimeStampToDateTime(mValues[position].Received).ToString("HH:mm");
            }
            else
            {
                if (mValues[position].Role == "LocalFileForUpload")
                {
                    simpleHolder.ImageFileType.SetImageResource(Resource.Mipmap.ic_upload);
                    simpleHolder.ProgressBarFileLoading.Visibility = ViewStates.Visible;

                    simpleHolder.Time.Text = AppResources.FileUploading;
                    simpleHolder.FileName.Text = mValues[position].Name;
                }
                else if (mValues[position].Text != null && mValues[position].Text != string.Empty 
                    && (mValues[position].Text.Contains("dcre-artifacts.s3.amazonaws.com") || mValues[position].Text.Contains("/storage/exchange/")))
                {
                    if (mValues[position].Text.Contains(".jpg") || mValues[position].Text.Contains(".jpeg"))
                    {
                        simpleHolder.ImageFileType.SetImageResource(Resource.Mipmap.jpg);
                    }
                    else if (mValues[position].Text.Contains(".png"))
                    {
                        simpleHolder.ImageFileType.SetImageResource(Resource.Mipmap.png);
                    }
                    else if (mValues[position].Text.Contains(".pdf"))
                    {
                        simpleHolder.ImageFileType.SetImageResource(Resource.Mipmap.pdf);
                    }
                    else
                    {
                        simpleHolder.ImageFileType.SetImageResource(Resource.Mipmap.docx);
                    }
                    simpleHolder.ProgressBarFileLoading.Visibility = ViewStates.Gone;
                    simpleHolder.Time.Text = DataManager.UnixTimeStampToDateTime(mValues[position].Received).ToString("HH:mm");
                    simpleHolder.FileName.Text = DataManager.GetFileNameFromAmazonUrl(mValues[position].Text.Replace("%20", " "));

                    simpleHolder.LayoutContainer.Click += delegate
                    {
                        var uri = Android.Net.Uri.Parse(mValues[position].Text);
                        var intent = new Intent(Intent.ActionView, uri);
                        currentContext.StartActivity(intent);
                    };
                }
                else
                {
                    simpleHolder.Time.Text = mValues[position].Name;
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = null;

            if (viewType == 0)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_MessagePersonal, parent, false);
            }
            if (viewType == 1)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_MessageReceived, parent, false);
            }
            if (viewType == 2)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_MessagesDate, parent, false);
            }
            if (viewType == 3)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.li_MessageAttachment, parent, false);
            }

            view.SetBackgroundResource(mBackground);
            view.Enabled = false;

            return new SupportViewHolder(view);
        }
    }

    public class SupportViewHolder : BaseListViewHolder
    {
        public readonly TextView UserName;
        public readonly TextView Message;
        public readonly TextView Time;

        public readonly ImageView ImageFileType;
        public readonly ProgressBar ProgressBarFileLoading;
        public readonly TextView FileName;
        public readonly RelativeLayout LayoutContainer;

        public SupportViewHolder(View view) : base(view)
        {
            UserName = view.FindViewById<TextView>(Resource.Id.chatItemReceiverName);
            Message = view.FindViewById<TextView>(Resource.Id.chatItemMessage);
            Time = view.FindViewById<TextView>(Resource.Id.chatItemTime);

            ImageFileType = view.FindViewById<ImageView>(Resource.Id.imageFileType);
            FileName = view.FindViewById<TextView>(Resource.Id.chatItemFileName);
            ProgressBarFileLoading = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            LayoutContainer = view.FindViewById<RelativeLayout>(Resource.Id.layoutContainer);
        }
    }
}
