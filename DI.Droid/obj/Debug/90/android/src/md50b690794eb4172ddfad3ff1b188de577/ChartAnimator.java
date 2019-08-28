package md50b690794eb4172ddfad3ff1b188de577;


public class ChartAnimator
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Charts.ChartAnimator, Syncfusion.SfChart.Android", ChartAnimator.class, __md_methods);
	}


	public ChartAnimator ()
	{
		super ();
		if (getClass () == ChartAnimator.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartAnimator, Syncfusion.SfChart.Android", "", this, new java.lang.Object[] {  });
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
