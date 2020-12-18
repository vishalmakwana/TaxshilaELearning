using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using TaxshilaMobile.iOS.PlatformSpecifics;
using TaxshilaMobile.ServiceBus.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace TaxshilaMobile.iOS.PlatformSpecifics
{
    public class BaseUrl : IBaseUrl
    {
        public string GetDatabasePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }  

            return Path.Combine(libFolder, "TaxshilaMobile.db");
        }
    }
}