using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using TaxshilaMobile.Controls;
using TaxshilaMobile.Droid.Renderers;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace TaxshilaMobile.Droid.Renderers
{
    public class CustomWebViewRenderer: ViewRenderer<CustomWebView, Android.Webkit.WebView>
    {
        const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
        Context _context;
        public string BaseUrl = "file:///android_asset/";
        public CustomWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Source")
            {

                Control.LoadDataWithBaseURL(BaseUrl, Element.Source, "text/html", "UTF-8", null);

            }
        }
        protected override void OnElementChanged(ElementChangedEventArgs<CustomWebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as CustomWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var webView = new Android.Webkit.WebView(_context);
                    webView.Settings.JavaScriptEnabled = true;
                    webView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavascriptFunction}"));
                    SetNativeControl(webView);
                }
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.LoadDataWithBaseURL(BaseUrl, Element.Source, "text/html", "UTF-8", null);
            }
        }
    }
    public class JavascriptWebViewClient : WebViewClient
    {
        string _javascript;

        public JavascriptWebViewClient(string javascript)
        {
            _javascript = javascript;
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript, null);
        }
    }

    public class JSBridge : Java.Lang.Object
    {
        readonly WeakReference<CustomWebViewRenderer> hybridWebViewRenderer;

        public JSBridge(CustomWebViewRenderer hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<CustomWebViewRenderer>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            CustomWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                hybridRenderer.Element.InvokeAction(data);
            }
        }
    }

}