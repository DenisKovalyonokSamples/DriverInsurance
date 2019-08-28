package md5beec6a4956d23ff6a6195461adb2b838;


public class XamarinRecyclerViewOnScrollListener
	extends android.support.v7.widget.RecyclerView.OnScrollListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onScrolled:(Landroid/support/v7/widget/RecyclerView;II)V:GetOnScrolled_Landroid_support_v7_widget_RecyclerView_IIHandler\n" +
			"";
		mono.android.Runtime.register ("DI.Droid.Fragments.XamarinRecyclerViewOnScrollListener, DI.Droid", XamarinRecyclerViewOnScrollListener.class, __md_methods);
	}


	public XamarinRecyclerViewOnScrollListener ()
	{
		super ();
		if (getClass () == XamarinRecyclerViewOnScrollListener.class)
			mono.android.TypeManager.Activate ("DI.Droid.Fragments.XamarinRecyclerViewOnScrollListener, DI.Droid", "", this, new java.lang.Object[] {  });
	}

	public XamarinRecyclerViewOnScrollListener (android.support.v7.widget.LinearLayoutManager p0)
	{
		super ();
		if (getClass () == XamarinRecyclerViewOnScrollListener.class)
			mono.android.TypeManager.Activate ("DI.Droid.Fragments.XamarinRecyclerViewOnScrollListener, DI.Droid", "Android.Support.V7.Widget.LinearLayoutManager, Xamarin.Android.Support.v7.RecyclerView", this, new java.lang.Object[] { p0 });
	}


	public void onScrolled (android.support.v7.widget.RecyclerView p0, int p1, int p2)
	{
		n_onScrolled (p0, p1, p2);
	}

	private native void n_onScrolled (android.support.v7.widget.RecyclerView p0, int p1, int p2);

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
