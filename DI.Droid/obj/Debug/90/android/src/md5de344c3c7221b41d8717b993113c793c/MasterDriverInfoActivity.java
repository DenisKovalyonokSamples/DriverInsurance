package md5de344c3c7221b41d8717b993113c793c;


public class MasterDriverInfoActivity
	extends md5d4448e07dae895a186301d52dee4c1f6.BaseFormActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("DI.Droid.MasterDriverInfoActivity, DI.Droid", MasterDriverInfoActivity.class, __md_methods);
	}


	public MasterDriverInfoActivity ()
	{
		super ();
		if (getClass () == MasterDriverInfoActivity.class)
			mono.android.TypeManager.Activate ("DI.Droid.MasterDriverInfoActivity, DI.Droid", "", this, new java.lang.Object[] {  });
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
