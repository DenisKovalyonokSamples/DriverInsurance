package md549054a6be0de4572c64818323caea3c6;


public class CarViewHolder
	extends md5bcbd144a6e92f8d0d188f0bc006b8173.BaseListViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DI.Droid.Adapters.CarViewHolder, DI.Droid", CarViewHolder.class, __md_methods);
	}


	public CarViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == CarViewHolder.class)
			mono.android.TypeManager.Activate ("DI.Droid.Adapters.CarViewHolder, DI.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
