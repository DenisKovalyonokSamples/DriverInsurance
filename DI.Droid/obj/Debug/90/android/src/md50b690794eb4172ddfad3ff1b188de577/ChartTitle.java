package md50b690794eb4172ddfad3ff1b188de577;


public class ChartTitle
	extends android.widget.TextView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTextChanged:(Ljava/lang/CharSequence;III)V:GetOnTextChanged_Ljava_lang_CharSequence_IIIHandler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Charts.ChartTitle, Syncfusion.SfChart.Android", ChartTitle.class, __md_methods);
	}


	public ChartTitle (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ChartTitle.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartTitle, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ChartTitle (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ChartTitle.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartTitle, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ChartTitle (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ChartTitle.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartTitle, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public ChartTitle (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == ChartTitle.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ChartTitle, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onTextChanged (java.lang.CharSequence p0, int p1, int p2, int p3)
	{
		n_onTextChanged (p0, p1, p2, p3);
	}

	private native void n_onTextChanged (java.lang.CharSequence p0, int p1, int p2, int p3);

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
