package md5d4448e07dae895a186301d52dee4c1f6;


public class SmartWebViewClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageFinished:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnPageFinished_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_onReceivedSslError:(Landroid/webkit/WebView;Landroid/webkit/SslErrorHandler;Landroid/net/http/SslError;)V:GetOnReceivedSslError_Landroid_webkit_WebView_Landroid_webkit_SslErrorHandler_Landroid_net_http_SslError_Handler\n" +
			"n_onReceivedError:(Landroid/webkit/WebView;ILjava/lang/String;Ljava/lang/String;)V:GetOnReceivedError_Landroid_webkit_WebView_ILjava_lang_String_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("DI.Droid.Base.SmartWebViewClient, DI.Droid", SmartWebViewClient.class, __md_methods);
	}


	public SmartWebViewClient ()
	{
		super ();
		if (getClass () == SmartWebViewClient.class)
			mono.android.TypeManager.Activate ("DI.Droid.Base.SmartWebViewClient, DI.Droid", "", this, new java.lang.Object[] {  });
	}

	public SmartWebViewClient (md5d4448e07dae895a186301d52dee4c1f6.BaseWebViewActivity p0)
	{
		super ();
		if (getClass () == SmartWebViewClient.class)
			mono.android.TypeManager.Activate ("DI.Droid.Base.SmartWebViewClient, DI.Droid", "DI.Droid.Base.BaseWebViewActivity, DI.Droid", this, new java.lang.Object[] { p0 });
	}


	public void onPageFinished (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onPageFinished (p0, p1);
	}

	private native void n_onPageFinished (android.webkit.WebView p0, java.lang.String p1);


	public void onReceivedSslError (android.webkit.WebView p0, android.webkit.SslErrorHandler p1, android.net.http.SslError p2)
	{
		n_onReceivedSslError (p0, p1, p2);
	}

	private native void n_onReceivedSslError (android.webkit.WebView p0, android.webkit.SslErrorHandler p1, android.net.http.SslError p2);


	public void onReceivedError (android.webkit.WebView p0, int p1, java.lang.String p2, java.lang.String p3)
	{
		n_onReceivedError (p0, p1, p2, p3);
	}

	private native void n_onReceivedError (android.webkit.WebView p0, int p1, java.lang.String p2, java.lang.String p3);

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
