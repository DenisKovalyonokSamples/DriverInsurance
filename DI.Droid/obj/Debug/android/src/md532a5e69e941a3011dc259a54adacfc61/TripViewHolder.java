package md532a5e69e941a3011dc259a54adacfc61;


public class TripViewHolder
	extends md53af0358604fc26ef603a3c049b94bf50.BaseListViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("DI.Droid.Adapters.TripViewHolder, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", TripViewHolder.class, __md_methods);
	}


	public TripViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == TripViewHolder.class)
			mono.android.TypeManager.Activate ("DI.Droid.Adapters.TripViewHolder, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
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
