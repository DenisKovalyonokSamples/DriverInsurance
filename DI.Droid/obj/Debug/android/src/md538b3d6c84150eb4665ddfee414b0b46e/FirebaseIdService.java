package md538b3d6c84150eb4665ddfee414b0b46e;


public class FirebaseIdService
	extends com.google.firebase.iid.FirebaseInstanceIdService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("DI.Droid.FirebaseIdService, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FirebaseIdService.class, __md_methods);
	}


	public FirebaseIdService ()
	{
		super ();
		if (getClass () == FirebaseIdService.class)
			mono.android.TypeManager.Activate ("DI.Droid.FirebaseIdService, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

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
