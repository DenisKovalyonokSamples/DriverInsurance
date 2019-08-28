package md52c8515cff2994e8e000bffb185dd2767;


public class ObservableArrayList
	extends java.util.ArrayList
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_add:(Ljava/lang/Object;)Z:GetAdd_Ljava_lang_Object_Handler\n" +
			"n_remove:(Ljava/lang/Object;)Z:GetRemove_Ljava_lang_Object_Handler\n" +
			"n_remove:(I)Ljava/lang/Object;:GetRemove_IHandler\n" +
			"n_add:(ILjava/lang/Object;)V:GetAdd_ILjava_lang_Object_Handler\n" +
			"n_clear:()V:GetClearHandler\n" +
			"n_set:(ILjava/lang/Object;)Ljava/lang/Object;:GetSet_ILjava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Charts.ObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", ObservableArrayList.class, __md_methods);
	}


	public ObservableArrayList ()
	{
		super ();
		if (getClass () == ObservableArrayList.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public ObservableArrayList (int p0)
	{
		super (p0);
		if (getClass () == ObservableArrayList.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", "System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public ObservableArrayList (java.util.Collection p0)
	{
		super (p0);
		if (getClass () == ObservableArrayList.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Charts.ObservableArrayList, Syncfusion.SfChart.Android, Version=15.4451.0.20, Culture=neutral, PublicKeyToken=null", "System.Collections.ICollection, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0 });
	}


	public boolean add (java.lang.Object p0)
	{
		return n_add (p0);
	}

	private native boolean n_add (java.lang.Object p0);


	public boolean remove (java.lang.Object p0)
	{
		return n_remove (p0);
	}

	private native boolean n_remove (java.lang.Object p0);


	public java.lang.Object remove (int p0)
	{
		return n_remove (p0);
	}

	private native java.lang.Object n_remove (int p0);


	public void add (int p0, java.lang.Object p1)
	{
		n_add (p0, p1);
	}

	private native void n_add (int p0, java.lang.Object p1);


	public void clear ()
	{
		n_clear ();
	}

	private native void n_clear ();


	public java.lang.Object set (int p0, java.lang.Object p1)
	{
		return n_set (p0, p1);
	}

	private native java.lang.Object n_set (int p0, java.lang.Object p1);

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
