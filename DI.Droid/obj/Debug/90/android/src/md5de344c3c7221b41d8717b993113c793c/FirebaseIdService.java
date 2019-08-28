package md5de344c3c7221b41d8717b993113c793c;


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
		mono.android.Runtime.register ("DI.Droid.FirebaseIdService, DI.Droid", FirebaseIdService.class, __md_methods);
	}


	public FirebaseIdService ()
	{
		super ();
		if (getClass () == FirebaseIdService.class)
			mono.android.TypeManager.Activate ("DI.Droid.FirebaseIdService, DI.Droid", "", this, new java.lang.Object[] {  });
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
