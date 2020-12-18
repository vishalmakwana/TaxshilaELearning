using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using TaxshilaMobile.Controls;
using TaxshilaMobile.iOS.Renderers;
using Plugin.SharedTransitions.Platforms.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ExtendedNavigationPage), typeof(ExtendedNavigationPageRenderer))]
namespace TaxshilaMobile.iOS.Renderers
{
    public class ExtendedNavigationPageRenderer : SharedTransitionNavigationRenderer
    {
        public ExtendedNavigationPageRenderer()
        {

        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = true;
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                NavigationBar.StandardAppearance.ShadowColor = UIColor.Clear;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }
    }
}