using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Foundation;
using TaxshilaMobile.iOS.PlatformSpecifics;
using TaxshilaMobile.ServiceBus.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(HttpTimeOut))]

namespace TaxshilaMobile.iOS.PlatformSpecifics
{
    public class HttpTimeOut : IHttpTimeOut
    {
        public HttpClient GetHttpClient()
        {
            var configuration = NSUrlSessionConfiguration.DefaultSessionConfiguration; // or any other needed
            configuration.TimeoutIntervalForRequest = 4200; // in seconds 
            var handler = new NSUrlSessionHandler(configuration);
            return new HttpClient(handler);
        }
    }
}