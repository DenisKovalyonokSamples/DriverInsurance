package md5c3f794aca97c55dc0e6d5992e563f700;


public class LocationCallback
	extends com.google.android.gms.location.LocationCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onLocationResult:(Lcom/google/android/gms/location/LocationResult;)V:GetOnLocationResult_Lcom_google_android_gms_location_LocationResult_Handler\n" +
			"n_onLocationAvailability:(Lcom/google/android/gms/location/LocationAvailability;)V:GetOnLocationAvailability_Lcom_google_android_gms_location_LocationAvailability_Handler\n" +
			"";
		mono.android.Runtime.register ("Android.Gms.Location.LocationCallback, Xamarin.GooglePlayServices.Location, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LocationCallback.class, __md_methods);
	}


	public LocationCallback ()
	{
		super ();
		if (getClass () == LocationCallback.class)
			mono.android.TypeManager.Activate ("Android.Gms.Location.LocationCallback, Xamarin.GooglePlayServices.Location, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onLocationResult (com.google.android.gms.location.LocationResult p0)
	{
		n_onLocationResult (p0);
	}

	private native void n_onLocationResult (com.google.android.gms.location.LocationResult p0);


	public void onLocationAvailability (com.google.android.gms.location.LocationAvailability p0)
	{
		n_onLocationAvailability (p0);
	}

	private native void n_onLocationAvailability (com.google.android.gms.location.LocationAvailability p0);

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
