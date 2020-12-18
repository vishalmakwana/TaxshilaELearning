using TaxshilaMobile.Droid.PlatformSpecifics;
using TaxshilaMobile.ServiceBus.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace TaxshilaMobile.Droid.PlatformSpecifics
{
    public class BaseUrl : IBaseUrl
    {
        public string GetDatabasePath()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, "JcHardwareMart.db");
        }
    }
}