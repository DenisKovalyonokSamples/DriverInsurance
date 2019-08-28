package md52c8515cff2994e8e000bffb185dd2767;


public class ChartObservableArrayList
	extends md52c8515cff2994e8e000bffb185dd2767.ObservableArrayList
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Charts.ChartObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", ChartObservableArrayList.class, __md_methods);
	}


	public ChartObservableArrayList ()
	{
		super ();
		if (getClass () == ChartObservableArrayList.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public ChartObservableArrayList (int p0)
	{
		super (p0);
		if (getClass () == ChartObservableArrayList.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public ChartObservableArrayList (java.util.Collection p0)
	{
		super (p0);
		if (getClass () == ChartObservableArrayList.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", "System.Collections.ICollection, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
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
