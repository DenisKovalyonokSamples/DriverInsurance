using System;
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;

namespace DI.Droid.Adapters.Callbacks
{
    public delegate void ItemSwipedHandler(RecyclerView.ViewHolder viewHolder, int i);

    class SimpleItemTouchHelperCallback : ItemTouchHelper.Callback
    {
        public event ItemSwipedHandler ItemSwiped;
        private bool dragEnabled = true;

        private bool swipeEnabled = true;

        public static float AlphaFull = 1.0f;

        private RecyclerView.Adapter adapter;


        public SimpleItemTouchHelperCallback(RecyclerView.Adapter adapter)
        {
            this.adapter = adapter;
        }

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            return 0;
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder source, RecyclerView.ViewHolder target)
        {
            if (source.ItemViewType != target.ItemViewType)
            {
                return false;
            }
            return false;
        }

        public override void OnSwiped(Android.Support.V7.Widget.RecyclerView.ViewHolder viewHolder, int i)
        {
        }
    }
}
