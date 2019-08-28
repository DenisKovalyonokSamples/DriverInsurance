package mono.com.google.android.gms.measurement;


public class AppMeasurement_OnEventListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.measurement.AppMeasurement.OnEventListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onEvent:(Ljava/lang/String;Ljava/lang/String;Landroid/os/Bundle;J)V:GetOnEvent_Ljava_lang_String_Ljava_lang_String_Landroid_os_Bundle_JHandler:Android.Gms.Measurement.AppMeasurement/IOnEventListenerInvoker, Xamarin.Firebase.Analytics.Impl\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Measurement.AppMeasurement+IOnEventListenerImplementor, Xamarin.Firebase.Analytics.Impl", AppMeasurement_OnEventListenerImplementor.class, __md_methods);
	}


	public AppMeasurement_OnEventListenerImplementor ()
	{
		super ();
		if (getClass () == AppMeasurement_OnEventListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Gms.Measurement.AppMeasurement+IOnEventListenerImplementor, Xamarin.Firebase.Analytics.Impl", "", this, new java.lang.Object[] {  });
	}


	public void onEvent (java.lang.String p0, java.lang.String p1, android.os.Bundle p2, long p3)
	{
		n_onEvent (p0, p1, p2, p3);
	}

	private native void n_onEvent (java.lang.String p0, java.lang.String p1, android.os.Bundle p2, long p3);

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
