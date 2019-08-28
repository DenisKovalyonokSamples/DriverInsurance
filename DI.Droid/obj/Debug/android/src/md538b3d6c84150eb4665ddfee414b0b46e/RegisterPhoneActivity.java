package md538b3d6c84150eb4665ddfee414b0b46e;


public class RegisterPhoneActivity
	extends md51ee0e2ee6664306762b0f3749d9f613a.BaseFormActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("DI.Droid.RegisterPhoneActivity, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RegisterPhoneActivity.class, __md_methods);
	}


	public RegisterPhoneActivity ()
	{
		super ();
		if (getClass () == RegisterPhoneActivity.class)
			mono.android.TypeManager.Activate ("DI.Droid.RegisterPhoneActivity, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
