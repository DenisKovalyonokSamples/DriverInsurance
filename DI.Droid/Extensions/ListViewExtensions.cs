using System;
using Android.Views;
using Android.Support.V7.Widget;

namespace DI.Droid.Extensions
{
    public static class ListViewExtensions
    {
        public static void SetItemClickListener(this RecyclerView rv, Action<RecyclerView, int, View> action)
        {
            rv.AddOnChildAttachStateChangeListener(new AttachStateChangeListener(rv, action));
        }

        public static void SetItemLongClickListener(this RecyclerView rv, Action<RecyclerView, int, View> action)
        {
            rv.AddOnChildAttachStateChangeListener(new AttachLongStateChangeListener(rv, action));
        }
    }

    public class AttachLongStateChangeListener : Java.Lang.Object, RecyclerView.IOnChildAttachStateChangeListener
    {
        private RecyclerView mRecyclerview;
        private Action<RecyclerView, int, View> mAction;

        public AttachLongStateChangeListener(RecyclerView rv, Action<RecyclerView, int, View> action) : base()
        {
            mRecyclerview = rv;
            mAction = action;
        }

        public void OnChildViewAttachedToWindow(View view)
        {
            view.LongClick += View_LongClick;
        }

        public void OnChildViewDetachedFromWindow(View view)
        {
            view.LongClick -= View_LongClick;
        }

        private void View_LongClick(object sender, EventArgs e)
        {
            RecyclerView.ViewHolder holder = mRecyclerview.GetChildViewHolder(((View)sender));
            mAction.Invoke(mRecyclerview, holder.AdapterPosition, ((View)sender));
        }
    }

    public class AttachStateChangeListener : Java.Lang.Object, RecyclerView.IOnChildAttachStateChangeListener
    {
        private RecyclerView mRecyclerview;
        private Action<RecyclerView, int, View> mAction;

        public AttachStateChangeListener(RecyclerView rv, Action<RecyclerView, int, View> action) : base()
        {
            mRecyclerview = rv;
            mAction = action;
        }

        public void OnChildViewAttachedToWindow(View view)
        {
            view.Click += View_Click;
        }

        public void OnChildViewDetachedFromWindow(View view)
        {
            view.Click -= View_Click;
        }

        private void View_Click(object sender, EventArgs e)
        {
            RecyclerView.ViewHolder holder = mRecyclerview.GetChildViewHolder(((View)sender));
            mAction.Invoke(mRecyclerview, holder.AdapterPosition, ((View)sender));
        }
    }
}
