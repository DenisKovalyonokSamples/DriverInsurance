using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace DI.Droid.Adapters.Base
{
    public class BaseListViewHolder : RecyclerView.ViewHolder
    {
        public string mBoundString;
        public readonly View mView;
        public ImageView Icon;

        public BaseListViewHolder(View view) : base(view)
        {
            mView = view;
        }
    }
}
