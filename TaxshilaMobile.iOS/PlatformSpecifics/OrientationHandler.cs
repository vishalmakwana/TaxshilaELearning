using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TaxshilaMobile.iOS.PlatformSpecifics;
using TaxshilaMobile.ServiceBus.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(OrientationHandler))]
namespace TaxshilaMobile.iOS.PlatformSpecifics
{
    public class OrientationHandler : IOrientationHandler
    {
        public void ForceLandscape()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
        }

        public void ForcePortrait()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }
    }
}