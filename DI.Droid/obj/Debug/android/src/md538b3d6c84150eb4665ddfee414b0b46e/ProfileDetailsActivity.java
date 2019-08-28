package md538b3d6c84150eb4665ddfee414b0b46e;


public class ProfileDetailsActivity
	extends md51ee0e2ee6664306762b0f3749d9f613a.BaseActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("DI.Droid.ProfileDetailsActivity, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ProfileDetailsActivity.class, __md_methods);
	}


	public ProfileDetailsActivity ()
	{
		super ();
		if (getClass () == ProfileDetailsActivity.class)
			mono.android.TypeManager.Activate ("DI.Droid.ProfileDetailsActivity, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
