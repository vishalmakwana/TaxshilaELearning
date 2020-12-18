using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using TaxshilaMobile.Controls;
using TaxshilaMobile.iOS.Renderers;
using UIKit;
using WebKit;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]

namespace TaxshilaMobile.iOS.Renderers
{
    //public class CustomWebViewRenderer : WebViewRenderer
    //{
    //    protected override void OnElementChanged(VisualElementChangedEventArgs e) {
    //      base.OnElementChanged(e);
    //      var webView = this;
    //      webView.ScrollView.ScrollEnabled = false;
    //      webView.ScrollView.Bounces = false;
    //  }
    //}

    public class CustomWebViewRenderer : ViewRenderer<CustomWebView, WKWebView>, IWKScriptMessageHandler
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Source")
            {
                if (Element.Source != null)
                {
                    string contentDirectoryPath = Path.Combine(NSBundle.MainBundle.BundlePath);
                    Control.LoadHtmlString(Element.Source, new NSUrl(contentDirectoryPath, true));
                }
                //Control.LoadHtmlString(new NSString(Element.Source), new NSUrl("http://home.boxerproperty.com"));
            }
        }
        protected override void OnElementChanged(ElementChangedEventArgs<CustomWebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                var hybridWebView = e.OldElement as CustomWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    userController = new WKUserContentController();
                    var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                    userController.AddUserScript(script);
                    userController.AddScriptMessageHandler(this, "invokeAction");

                    var config = new WKWebViewConfiguration { UserContentController = userController };
                    var webView = new WKWebView(Frame, config);
                    SetNativeControl(webView);
                }
                if (Element.Source != null)
                {
                    string contentDirectoryPath = Path.Combine(NSBundle.MainBundle.BundlePath);
                    Control.LoadHtmlString(Element.Source, new NSUrl(contentDirectoryPath, true));
                }
                //string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", Element.Source));
                // Control.LoadHtmlString(new NSString(Element.Source), new NSUrl("http://home.boxerproperty.com"));
                //Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (message.Body != null)
            {
                Element.InvokeAction(message.Body.ToString());
            }
        }
    }
}