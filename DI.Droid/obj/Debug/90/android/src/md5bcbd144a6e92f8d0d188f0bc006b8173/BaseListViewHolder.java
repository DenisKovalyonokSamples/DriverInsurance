package md5bcbd144a6e92f8d0d188f0bc006b8173;


public class BaseListViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DI.Droid.Adapters.Base.BaseListViewHolder, DI.Droid", BaseListViewHolder.class, __md_methods);
	}


	public BaseListViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == BaseListViewHolder.class)
			mono.android.TypeManager.Activate ("DI.Droid.Adapters.Base.BaseListViewHolder, DI.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
