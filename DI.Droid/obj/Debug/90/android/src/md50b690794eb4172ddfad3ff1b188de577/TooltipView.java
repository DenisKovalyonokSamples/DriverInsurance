package md50b690794eb4172ddfad3ff1b188de577;


public class TooltipView
	extends android.widget.FrameLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Charts.TooltipView, Syncfusion.SfChart.Android", TooltipView.class, __md_methods);
	}


	public TooltipView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == TooltipView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.TooltipView, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public TooltipView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == TooltipView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.TooltipView, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public TooltipView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == TooltipView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.TooltipView, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public TooltipView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == TooltipView.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.TooltipView, Syncfusion.SfChart.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);

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
