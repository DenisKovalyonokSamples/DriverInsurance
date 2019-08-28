package md538b3d6c84150eb4665ddfee414b0b46e;


public class PasswordRestoreActivity
	extends md51ee0e2ee6664306762b0f3749d9f613a.BaseFormActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"";
		mono.android.Runtime.register ("DI.Droid.PasswordRestoreActivity, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PasswordRestoreActivity.class, __md_methods);
	}


	public PasswordRestoreActivity ()
	{
		super ();
		if (getClass () == PasswordRestoreActivity.class)
			mono.android.TypeManager.Activate ("DI.Droid.PasswordRestoreActivity, DI.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onOptionsItemSelected (android.view.MenuItem p0)
	{
		return n_onOptionsItemSelected (p0);
	}

	private native boolean n_onOptionsItemSelected (android.view.MenuItem p0);

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
