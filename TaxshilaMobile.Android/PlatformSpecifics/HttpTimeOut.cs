using TaxshilaMobile.Droid.PlatformSpecifics;
using TaxshilaMobile.ServiceBus.Services;
using System;
using System.Net.Http;
using Xamarin.Android.Net;

[assembly: Xamarin.Forms.Dependency(typeof(HttpTimeOut))]
namespace TaxshilaMobile.Droid.PlatformSpecifics
{
    public class HttpTimeOut : IHttpTimeOut
    {
        public HttpClient GetHttpClient()
        {
            AndroidClientHandler androidClientHandler = new AndroidClientHandler();
            androidClientHandler.ConnectTimeout = new TimeSpan(1, 10, 0);
            return new HttpClient(androidClientHandler);
        }
    }
}