using Android.Net.Http;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace DI.Droid.Base
{
    public abstract class BaseWebViewActivity : BaseActivity
    {
        protected WebView webView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void InitControls()
        {
            base.InitControls();

            InitWebView();
            InitProgressBars();

            ShowLoadingBar();

            webView.SetWebViewClient(new SmartWebViewClient(this));
            webView.Visibility = ViewStates.Invisible;
            webView.Settings.JavaScriptEnabled = true;

            webView.LoadUrl(Url);
        }

        protected virtual void InitWebView()
        {
            webView = FindViewById<WebView>(Resource.Id.webView);
            webView.SetWebViewClient(new SmartWebViewClient(this));
            webView.Visibility = ViewStates.Invisible;
        }

        #region Progress Bar Loading

        protected ProgressBar _progressBarLoading;
        protected bool loadingFinished = false;

        protected virtual void InitProgressBars()
        {
            _progressBarLoading = FindViewById<ProgressBar>(Resource.Id.progressBarLoading);
        }

        protected virtual void ShowLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.Indeterminate = true;
            _progressBarLoading.Visibility = ViewStates.Visible;
        }

        protected virtual void HideLoadingBar()
        {
            if (_progressBarLoading == null)
                return;

            _progressBarLoading.Visibility = ViewStates.Gone;

        }

        #endregion

        public void OnResume()
        {
            base.OnResume();

            if (!loadingFinished)
            {
                ShowLoadingBar();
                webView.Visibility = ViewStates.Invisible;
                webView.LoadUrl(Url);
            }
        }

        public virtual void ShowContent()
        {
            HideLoadingBar();
            webView.Visibility = ViewStates.Visible;
            loadingFinished = true;
        }

        public string Url { get; set; }
    }

    public class SmartWebViewClient : WebViewClient
    {
        BaseWebViewActivity _parent;

        public SmartWebViewClient(BaseWebViewActivity parent)
        {
            _parent = parent;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            _parent.ShowContent();
        }

        public override void OnReceivedSslError(WebView view, SslErrorHandler handler, SslError error)
        {

        }

        public override void OnReceivedError(WebView view, ClientError errorCode, string description, string failingUrl)
        {

        }
    }
}

